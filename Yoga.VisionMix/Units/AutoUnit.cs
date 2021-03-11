using System;
using System.Drawing;
using System.Windows.Forms;
using HalconDotNet;
using Yoga.VisionMix.Frame;
using Yoga.Tools;
using Yoga.Common;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using System.IO.Ports;
using Yoga.Camera;
using Yoga.ImageControl;
using Yoga.VisionMix.Scheduling;
using Yoga.VisionMix.AppManger;
using Yoga.Common.Helpers;
using Yoga.Common.Basic;
using Yoga.Tools.Factory;


namespace Yoga.VisionMix.Units
{
    public partial class AutoUnit : UserControl
    {
        #region 类的字段
        FrmMain mainframe;
        bool isTestMode = false;
        bool isAdministrator;
        public Dictionary<int, CameraShow> CameraShowUnitDic = new Dictionary<int, CameraShow>();
        private CameraBase cameraSelect;   //抽象类不能通过new实例化。
        private int currentSettingIndex = 1;
        //创建一个空表
        DataTable dtResult = new DataTable();   //为了保存时提供使用，要借助这个表。
        DataTable dtResultShow = new DataTable();
        UpdateUI updateUI;//表格、运行状态、工具总览、图像显示窗口相机信息。'
        MessageQueue messageQueue;//log显示
        int cameraCount;
        /// <summary>
        /// 相机图像处理流程,其以相机编号为key 1开头
        /// </summary>
        public Dictionary<int, RunTheadData> runTheadDataDic = new Dictionary<int, RunTheadData>();
        TableLayoutPanel tabPanel;
        //Wrapper.UtilManaged utilManagedCli;
        List<int> CameraFindIndexList = new List<int>();
        RunComWriteDataThread runCommWriteDataThread;
        RunComReadDataThread runComReadDataThread;

        List<string> alarmtextlist = new List<string>();
        System.Windows.Forms.Timer timerAlarmTextCycle = new System.Windows.Forms.Timer();
        int alarmtextindex = 0;
        object objlock = new object();

        #endregion

        #region 类的属性
        public int CurrentSettingIndex
        {
            get
            {
                return currentSettingIndex;
            }

            set
            {
                currentSettingIndex = value;
            }
        }
        public bool IsTestMode
        {
            get
            {
                return isTestMode;
            }       
        }
        public RunComWriteDataThread RunCommWriteDataThread
        {
            get
            {
                return runCommWriteDataThread;
            }
        }
        public RunComReadDataThread RunCommReadDataThread
        {
            get
            {
                return runComReadDataThread;
            }
        }
        public bool IsAdministrator
        {
            get
            {
                return isAdministrator;
            }
        }
        public DataTable DtResultShow
        {
            get
            {
                return dtResultShow;
            }

            private set
            {
                dtResultShow = value;
            }
        }

        public List<string> AlarmTextList
        {
            get
            {   lock(objlock)
                {
                    return alarmtextlist;
                }                           
            }
        }


        public enum enTest
        {
            [EnumDescription("max")]
            最大,
            [EnumDescription("min")]
            最小
        }
        #endregion

        #region 构造函数
        public AutoUnit(FrmMain mainframe)
        {
            InitializeComponent();
            this.mainframe = mainframe;
            runComReadDataThread = new RunComReadDataThread();
            runCommWriteDataThread = new RunComWriteDataThread();
            runComReadDataThread.PortDataReceiveEvent += App_portDataReciveEvent;

            timerAlarmTextCycle.Interval = 1500;
            timerAlarmTextCycle.Tick += TimerAlarmTextCycle_Tick;

            updateUI = new UpdateUI(this);
            messageQueue = new MessageQueue(this.txtLog);
            Util.MessageEvent += Util_MessageEvent;
            cameraCount = IniStatus.Instance.CamearCount;
            CommHandle.Instance.SerialHelper.SerialErrorReceivedEvent += SerialErrorReceived_Event;
            CommHandle.Instance.IsLinkEvent += IsLink_Event;

            if (cameraCount > 4)
            {
                tabPanel = tableLayoutPanelMax8;
            }
            else if(cameraCount >1)
            {
                tabPanel = tableLayoutPanelHWnd;
            }
            else
            {
                panelHWndMax.Dock = DockStyle.Fill;                
                panelHWndMax.BringToFront();
                tabPanel = tableLayoutPanelHWnd;
                tabPanel.Dock = DockStyle.None;
            }
            if (cameraCount == 1)
            {
                //相机选择按钮
                tscobCameraSelect.Visible = false;
                //显示所有按钮
                tsbShowAll.Visible = false;
            }
        }
        #endregion

        #region 窗体及项目加载区域

        private void AutoUnit_Load(object sender, EventArgs e)
        {
            StatusManger statusManger = StatusManger.Instance;
            statusManger.RuningStatus = RuningStatus.初始化;
            toolShowUnit1.ToolGroupsChangeEvent += ToolGroupsChange_Event;
            timerInit.Enabled = true;
        }
        /// <summary>
        /// 工具栏右击菜单显示设定
        /// </summary>
        /// <param name="isLogin"></param>
        public void LoginSetting(bool isLogin)
        {
            //this.groupBoxUserSetting.Enabled = isLogin;
            //this.btnRun.Enabled = isLogin;
            //this.btnInterlocking.Enabled = isLogin;
            //this.toolShowUnit1.LoginSetting(isLogin);
            //toolStrip1.Enabled = isLogin;
            this.isAdministrator = isLogin;
            //工具栏右击菜单显示设定
            toolShowUnit1.LoginSetting(isLogin);
        }
        /// <summary>
        /// 初始化相机绑定显示窗口和图像处理线程，显示设定初始化,加载工程文件，初始化工具栏，相机参数设定。
        /// </summary>
        /// <param name="loadTool"></param>
        /// <param name="frmLoad"></param>
        public void LoadProject(bool loadTool, Common.UI.FrmLoad frmLoad = null)
        {
            InitRunData();
            /*if(CameraFindIndexList.Count==1)
                ChangeSize(CameraFindIndexList[0]);
            else
                ChangeSize(1); */
            if (cameraCount > 0 && cameraCount < 10)
                ChangeSize(-1);
            else
            {
                CommHandle.Instance.IsLink = false;
                Util.Notify(Level.Err, "相机设置个数超出1-9范围");
                return;
            }

            try
            {
                if (loadTool)
                {
                    ProjectData.Instance.ReadProject(UserSetting.Instance.ProjectPath);
                }
            }
            catch (Exception)
            {
                //Util.WriteLog(this.GetType(), ex);
                StatusManger.Instance.RuningStatus = RuningStatus.系统异常;
                Util.Notify(Level.Err, "项目加载异常");
            }
            frmLoad?.UpdateProgress("显示数据初始化...", 50, 70, 500);
            InitResultTable();
            try
            {
                toolShowUnit1.InitTreeView();
            }
            catch (Exception /*ex*/)
            {
                ;
            }
            frmLoad?.UpdateProgress("相机参数设置...", 70, 80, 500);
            foreach (var item in CameraManger.CameraDic)
            {
                CameraPram cameraPram = ProjectData.Instance.GetCameraPram(item.Key);

                item.Value.ShuterCur = cameraPram.Shutter;
                item.Value.GainCur = cameraPram.Gain;
                item.Value.TriggerDelayAbs = cameraPram.TriggerDelayAbs;
                item.Value.LineDebouncerTimeAbs = cameraPram.LineDebouncerTimeAbs;
                item.Value.OutLineTime = cameraPram.OutLineTime;
                item.Value.ImageAngle = cameraPram.ImageAngle;
                Thread.Sleep(1);
            }

            if (CameraManger.CameraDic.Count > 0)
            {
                Util.Notify("相机参数设置完成");
            }
            StatusManger.Instance.RuningStatus = RuningStatus.等待运行;
            StatusManger.Instance.TestInitFinish = true;
            UserSetting.Instance.SaveSetting();               //主要目的是把打开过的项目路径保存下来
            Initial();
        }

        public void Initial()
        {
            int count = ToolsFactory.ToolsDic.Count;
            for (int i = 1; i <= count; i++)
            {
                List<ToolBase> runToolList = ToolsFactory.GetToolList(i);

                foreach (var item in runToolList)
                {
                    if (item is IToolRun)
                    {
                        try
                        {
                            //item.Run();
                            item.RunRef();
                        }
                        catch
                        {

                        }
                    }
                }
            }

        }

        /// <summary>
        /// CameraShowUnitDic显示窗口，关闭运行图像处理线程，
        /// </summary>
        private void InitRunData()
        {
            tabPanel.BringToFront();
            tabPanel.Dock = DockStyle.Fill;    //覆盖了其它TableLayoutPanel控件。
            CameraShowUnitDic.Clear();
            //必须手动释放显示控件以清除native的图像集合数据
            foreach (var item in this.tabPanel.Controls)
            {
                UserControl control = item as UserControl;

                if (control != null)
                {
                    control.Dispose();
                }
            }
            foreach (var item in this.panelHWndMax.Controls)
            {
                UserControl control = item as UserControl;

                if (control != null)
                {
                    control.Dispose();
                }
            }

            this.tabPanel.Controls.Clear();
            this.panelHWndMax.Controls.Clear();

            if (runTheadDataDic.Count > 0)
            {
                foreach (var item in runTheadDataDic.Values)
                {
                    item.Stop();
                }
            }
            runTheadDataDic.Clear();
            //runThreadDic.Clear();
            InitResultTable();
            //清空相机选择控件
            tscobCameraSelect.Items.Clear();
            //清空相机list
            CameraFindIndexList.Clear();
            //相机与显示图像窗口绑定，与图像处理线程绑定
            for (int i = 0; i < cameraCount; i++)
            {
                tscobCameraSelect.Items.Add(string.Format("相机{0}", i + 1));

                CameraShow showUnit = new CameraShow();
                showUnit.Init(i + 1);
                CameraShowUnitDic.Add(i + 1, showUnit);
                showUnit.CameraShowMouseDoubleClick += HWndCtrl_MouseDoubleClick;
                runTheadDataDic.Add(i + 1, new RunTheadData(this, i + 1));
                if (CameraManger.CameraDic.ContainsKey(i + 1))
                {
                    runTheadDataDic[i + 1].BindCameraEvent(CameraManger.CameraDic[i + 1]);
                    CameraFindIndexList.Add(i + 1);
                    CameraManger.CameraDic[i + 1].CameraLineOffEvent += CameraLineOff_Event;
                }
                else
                {
                    showUnit.HWndUnit.SetTextBackColor();
                }

                //runThreadDic.Add(i + 1, new Thread(new ThreadStart(runTheadDataDic[i + 1].Run)));
                //runThreadDic[i + 1].Start();
                if (cameraCount < 10)
                {
                    tabPanel.RowCount = 2;
                }
                if (cameraCount < 5)//小于等于4个相机的处理
                {
                    switch (i + 1)
                    {
                        case 1:
                            this.tabPanel.Controls.Add(showUnit.HWndUnit, 0, 0);
                            if (cameraCount == 2)
                            {
                                tabPanel.SetRowSpan(showUnit.HWndUnit, 2);
                            }
                            break;
                        case 2:
                            this.tabPanel.Controls.Add(showUnit.HWndUnit, 1, 0);
                            if (cameraCount == 2)
                            {
                                tabPanel.SetRowSpan(showUnit.HWndUnit, 2);
                            }
                            break;
                        case 3:
                            this.tabPanel.Controls.Add(showUnit.HWndUnit, 0, 1);
                            break;
                        case 4:
                            this.tabPanel.Controls.Add(showUnit.HWndUnit, 1, 1);
                            break;
                        default:
                            break;
                    }
                }
                else//大于4个相机的处理
                {
                    switch (i + 1)
                    {
                        case 1:
                            this.tabPanel.Controls.Add(showUnit.HWndUnit, 0, 0);
                            break;
                        case 2:
                            this.tabPanel.Controls.Add(showUnit.HWndUnit, 1, 0);
                            break;
                        case 3:
                            this.tabPanel.Controls.Add(showUnit.HWndUnit, 2, 0);
                            break;
                        case 4:
                            this.tabPanel.Controls.Add(showUnit.HWndUnit, 0, 1);
                            break;
                        case 5:
                            this.tabPanel.Controls.Add(showUnit.HWndUnit, 1, 1);
                            break;
                        case 6:
                            this.tabPanel.Controls.Add(showUnit.HWndUnit, 2, 1);
                            break;
                        case 7:
                            this.tabPanel.Controls.Add(showUnit.HWndUnit, 0, 2);
                            break;
                        case 8:
                            this.tabPanel.Controls.Add(showUnit.HWndUnit, 1, 2);
                            break;
                        case 9:
                            this.tabPanel.Controls.Add(showUnit.HWndUnit, 2, 2);
                            break;
                        default:
                            break;
                    }
                }
            }
            if (CameraFindIndexList.Count > 0)
            {
                tscobCameraSelect.SelectedIndex = CameraFindIndexList[0] - 1;
            }
            else
            {
                tscobCameraSelect.SelectedIndex = 0;
            }
            //panelDgvTools.Controls.Add(CameraShowUnitDic[1].DgvTool);
            //CameraShowUnitDic[1].InitDgvShow();
        }
        /// <summary>
        /// 初始化显示表格。
        /// </summary>
        private void InitResultTable()
        {
            dtResult.Columns.Clear();
            dtResult.Columns.Add("相机", typeof(string));
            dtResult.Columns.Add("时间", typeof(string));

            dtResult.Columns.Add("正常计数", typeof(string));
            dtResult.Columns.Add("失败计数", typeof(string));
            dtResult.Columns.Add("异常率", typeof(string));
            dtResult.Columns.Add("节拍时间", typeof(string));

            DtResultShow = dtResult.Clone();
            DtResultShow.Rows.Clear();
            for (int i = 0; i < cameraCount; i++)
            {
                DataRow dr = DtResultShow.NewRow();
                dr["正常计数"] = 0;
                dr["失败计数"] = 0;
                dr["相机"] = "相机" + (i + 1);
                DtResultShow.Rows.Add(dr);
            }
            dgvResultShow.DataSource = DtResultShow;
        }
        /// <summary>
        /// 显示窗口设定、选择
        /// </summary>
        /// <param name="index"></param>
        private void ChangeSize(int index)
        {
            if (cameraCount == 1)
            {
                foreach (var item in CameraShowUnitDic.Values)
                {
                    item.HWndUnit.Parent = panelHWndMax;
                }
                panelHWndMax.Dock = DockStyle.Fill;
                tabPanel.Dock = DockStyle.None;
                panelHWndMax.BringToFront();      //放在该位置屏幕切换时闪烁会小些。
                //显示刷新
                foreach (var item in CameraShowUnitDic.Values)
                {
                    item.HWndUnit.HWndCtrl.Repaint();
                }
                if (CameraManger.CameraDic.ContainsKey(1))
                {
                    cameraSelect = CameraManger.CameraDic[1];
                }

                return;
            }


            if (index == -1)
            {
                foreach (var item in CameraShowUnitDic.Values)
                {
                    item.HWndUnit.Parent = tabPanel;
                }
                tabPanel.Dock = DockStyle.Fill;
                //panelHWndMax.Dock = DockStyle.None;
                tabPanel.BringToFront();            //放在该位置屏幕切换时闪烁会小些。

                panelHWndMax.Controls.Clear();
                panelHWndMax.Dock = DockStyle.None;

                //显示刷新
                foreach (var item in CameraShowUnitDic.Values)
                {
                    item.HWndUnit.HWndCtrl.Repaint();
                }
            }
            else
            {
                if (CameraShowUnitDic.ContainsKey(index) == false)
                {
                    MessageBox.Show(string.Format("相机窗口{0}未初始化", index), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //currentSettingIndex = index;   //选择当前哪个工具组。
                //删除控件在另一个容器中的目的是不让控件同时在两个容器中执行。
                this.tabPanel.Controls.Remove(CameraShowUnitDic[index].HWndUnit); //为什么把它删除了，但还是可以正常使用，很奇怪。//当index=-1时，通过指定Parent又加进去了
                CameraShowUnitDic[index].HWndUnit.Parent = panelHWndMax;

                CameraShowUnitDic[index].HWndUnit.BringToFront();  //这一行是不能取消的，不然不能显示了。
                panelHWndMax.Dock = DockStyle.Fill;
                tabPanel.Dock = DockStyle.None;
                panelHWndMax.BringToFront();                //放在该位置屏幕切换时闪烁会小些。

                if (CameraManger.CameraDic.ContainsKey(index))           //Default:1st Camera
                    cameraSelect = CameraManger.CameraDic[index];
                else
                    cameraSelect = null;

                // CameraShowUnitDic[index].InitDgvShow();   //已经取消的函数。
                foreach (var item in CameraShowUnitDic.Values)
                {
                    item.HWndUnit.HWndCtrl.Repaint();
                }
            }
        }
        #endregion

        #region 自定义事件响应区域
        private void Util_MessageEvent(object sender, MessageEventArgs e)
        {
            SetNotifyMessage(e.Message);

            if (e.level == Level.Err)
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (AlarmTextList.FindIndex(x => x == e.Message) > -1)
                        return;
                    AlarmTextList.Add(e.Message);
                    if(AlarmTextList.Count == 1)
                    {
                        toolStripLabelAlarmDisplay.Text = AlarmTextList[0];
                        toolStripLabelAlarmDisplay.Visible = true;
                    }
                    else if (AlarmTextList.Count == 2)
                    {
                        if (!timerAlarmTextCycle.Enabled)
                            timerAlarmTextCycle.Start();                        
                    }
                }));
            }
        }

        private void App_portDataReciveEvent(object sender, PortDataReciveEventArgs e)
        {
            if (isTestMode)
            {
                Util.Notify(Level.Err, "---收到触发命令,测试模式下不响应");
                return;
            }
            string code = e.Data.ToLower();
            int settingIndexRec = 0;
            //System.Diagnostics.Debug.WriteLine("网络线程:" + Thread.CurrentThread.ManagedThreadId);
            if (code == "start")
            {
                Util.Notify("---收到触发命令,工具组1开始运行");
                int cameraindex = toolShowUnit1.GetCameraIndex(1);
                if (cameraindex < 1 || CameraManger.CameraDic.ContainsKey(cameraindex) == false)
                {
                    Util.Notify(Level.Err, $"收到触发工具组{settingIndexRec}命令,但对应相机不存在");
                    return;
                }
                else if (CameraManger.CameraDic.ContainsKey(cameraindex))
                {
                    if (CameraManger.CameraDic[cameraindex].IsLink == false)
                    {
                        Util.Notify(Level.Err, $"收到触发工具组{settingIndexRec}命令,但对应相机已离线");
                        return;
                    }
                }
                CameraManger.CameraDic[cameraindex].GrabImage(1050, 1);
                //runTheadDataDic[cameraindex].TrigerRun(1, false); //三个参数的TrigerRun都是在该函数体内强制isExtTrigger为True,在RunAllTool函数中按外部触发模式来处理。
            }
            else if (int.TryParse(code, out settingIndexRec))
            {
                if (!ToolsFactory.ToolsDic.ContainsKey(settingIndexRec))
                {
                    Util.Notify(Level.Err, $"收到触发工具组{settingIndexRec}命令,但该工具组不存在");
                    return;
                }
                Util.Notify($"---收到触发命令,工具组{settingIndexRec}开始运行");
                int cameraindex = toolShowUnit1.GetCameraIndex(settingIndexRec);
                if(cameraindex < 1 || CameraManger.CameraDic.ContainsKey(cameraindex) == false)
                {                 
                    Util.Notify(Level.Err,$"收到触发工具组{settingIndexRec}命令,但对应相机不存在");
                    return;
                }
                else if(CameraManger.CameraDic.ContainsKey(cameraindex))
                {
                    if(CameraManger.CameraDic[cameraindex].IsLink == false)
                    {
                        Util.Notify(Level.Err, $"收到触发工具组{settingIndexRec}命令,但对应相机不存在");
                        return;
                    }
                }
                CameraManger.CameraDic[cameraindex].GrabImage(1050, settingIndexRec);
                //runTheadDataDic[cameraindex].TrigerRun(settingIndexRec, false);
            }
            else
            {
                Util.Notify(Level.Err, string.Format("收到异常触发相机命令：{0}", e.Data));
            }          
        }

        private void SerialErrorReceived_Event(object sender, SerialErrorReceivedEventArgs e)
        {
            switch(e.EventType)
            {
                case SerialError.Frame:
                    Util.Notify(Level.Err, "串口硬件检测到一个帧错误");
                    break;
                case SerialError.Overrun:
                    Util.Notify(Level.Err, "发送字符缓冲区超限，下个字符会丢失");
                    break;
                case SerialError.RXOver:
                    Util.Notify(Level.Err, "输入缓冲区溢出，或者在EOF后面还有一个字符");
                    break;
                case SerialError.RXParity:
                    Util.Notify(Level.Err, "硬件侦测到一个校验错误");
                    break;
                case SerialError.TXFull:
                    Util.Notify(Level.Err, "当输出缓冲区满时，应用程序仍触发写指令");
                    break;
                default:
                    break;
            } 
        }

        /// <summary>
        /// 处理当前运行状态变化事件
        /// </summary>
        private void IsLink_Event(object sender, IsLinkEventArgs ie)
        {
            if (ie.Islink)
            {
                if(CameraManger.CameraDic.Count>0)
                {
                    btnRun.BackColor = Color.Lime;
                    btnRun.Text = "运行中";
                    mainframe.ShowCommStatus(true);
                    isTestMode = false;
                    toolShowUnit1.CloseOffLineMode();
                    toolShowUnit1.IsRunning = true;
                    tscobToolGroupsSelect.Enabled = false;
                    if (CommHandle.Instance.CommunicationParam.IsExtTrigger)
                    {
                        StartExtTrigger();
                    }
                    else
                    {
                        StopExtTrigger();
                    }
                }               
                //toolShowUnit1.InitTreeView(); //在运行函数响应中已经通过LoadProject()调用，这里已没有必要。
            }
            else
            {
                this.Invoke(new Action(() =>  //一定要用Invoke:因为相机断线通知事件在不同线程，在哪里mainCommHandler.IsLink = false;然后导通该事件
                {
                    btnRun.BackColor = Control.DefaultBackColor;
                    btnRun.Text = "运行";
                    mainframe.ShowCommStatus(false);
                    toolShowUnit1.IsRunning = false;
                    tscobToolGroupsSelect.Enabled = true;
                }));
            }
        }

        private void HWndCtrl_MouseDoubleClick(object sender,HWndUnitMouseEventArgs e)
        {
            if (cameraCount == 1)
                return;
            if (CameraShowUnitDic.ContainsKey(e.CameraIndex))
            {
                if (CameraShowUnitDic[e.CameraIndex].HWndUnit.Parent == panelHWndMax)
                {
                    ChangeSize(-1);
                }
                else
                {
                    if (tscobCameraSelect.SelectedIndex == e.CameraIndex - 1)
                        ChangeSize(e.CameraIndex);               //通过直接调用ChangeSize()改变窗口
                    else
                        tscobCameraSelect.SelectedIndex = e.CameraIndex - 1;  //通过触发tscobCameraSelect_SelectedIndexChanged()函数调用ChangeSize()改变窗口。
                }
            }
        }
        
        private void CameraLineOff_Event(object sender, EventArgs e)
        {
            CameraBase camera = sender as CameraBase;
            Util.Notify(Level.Err, $"相机{camera.CameraIndex}断线请检查");
            if(CameraManger.CameraDic.Count == 1)
            {
                CommHandle.Instance.IsLink = false;
            }
            if(CameraManger.CameraDic.ContainsKey(camera.CameraIndex))
            {
                if (cameraSelect == CameraManger.CameraDic[camera.CameraIndex])
                    cameraSelect = null;
                CameraShowUnitDic[camera.CameraIndex].HWndUnit.SetTextBackColor();
                try
                {
                    CameraManger.CameraDic[camera.CameraIndex].Close();  //在这里已经中断退出，后面的不能执行

                }
                catch
                {
                    CameraManger.CameraDic.Remove(camera.CameraIndex);
                }           
                CameraManger.CameraDic.Remove(camera.CameraIndex);
            }              
        }

        /// <summary>
        /// 点击鼠标左键，工具栏添加工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolGroupsChange_Event(object sender,ToolGroupsChangeEventArgs e)
        {
            if (e.ToolGroupsNum == 0)
                tscobToolGroupsSelect.Items.Clear();
            if (e.ToolGroupsNum > 0)
            {
                string strtemp = string.Format("工具组{0}", e.ToolGroupsNum);
                int findindex = tscobToolGroupsSelect.FindString(strtemp);
                if (findindex < 0)
                {
                    tscobToolGroupsSelect.Items.Add(string.Format("工具组{0}", e.ToolGroupsNum));                    
                }
                else
                {
                    tscobToolGroupsSelect.SelectedIndex = findindex;
                    return;
                }
                tscobToolGroupsSelect.SelectedIndex = 0;

            }       
        }

        /// <summary>显示来自通信处理的消息</summary>
        /// <param name="message">要显示的消息</param>
        private void SetNotifyMessage(string message)
        {
            messageQueue.ShowMessage(message);
        }

        #endregion
               
        #region 按钮及组合框控件响应区域
        /// <summary>
        /// 删除统计结果，原是按钮，现改为主框架窗口的下拉菜单项控制
        /// </summary>
        public void btnDeleteResult_Click()
        {
            DialogResult result = MessageBox.Show("是否清除统计结果", "提示",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result != DialogResult.OK)
            {
                return;
            }

            try
            {
                save_data_result();
                foreach (var item in CameraManger.CameraDic.Values)
                {
                    item.ResetFps();
                }

                InitResultTable();
            }
            catch (Exception)
            {
                //Util.WriteLog(this.GetType(), ex);
                SetNotifyMessage(string.Format("运行结果保存出现异常"));
            }
        }
        public void save_data_result()
        {
            DateTime dt = DateTime.Now;
            string timeNow = dt.ToString();
            string dayNow = dt.ToString("yyyy_MM_dd");
            string project = System.IO.Path.GetFileNameWithoutExtension(UserSetting.Instance.ProjectPath);
            foreach (int key in runTheadDataDic.Keys)
            {
                string files = string.Format("..\\data\\{0}\\清零保存\\{0}_相机{1}_{2}.csv", project, key, dayNow);

                runTheadDataDic[key].ResetCount();

                SaveResult(key - 1, files);
            }
        }

        /// <summary>
        /// 切换运行状态
        /// </summary>
        public void btnRun_Click(object sender, EventArgs e)
        {
            int cameraCount = IniStatus.Instance.CamearCount;
            Dictionary<int, CameraBase> cameraDic = CameraManger.CameraDic;
            try
            {
                if (CommHandle.Instance.IsLink == false)
                {
                    /*StatusManger status = StatusManger.Instance;
                    if (status.RuningStatus == RuningStatus.系统异常)
                    {
                        MessageBox.Show("系统出现异常,请检查设置", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }*/

                    if (CameraManger.CameraDic.Count > 0)
                    {                   
                         CameraManger.Close();                        
                    }

                    if (CameraManger.Open() == false)
                    {
                        MessageBox.Show("没有相机连接,无法启动", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    for (int i = 0; i < cameraCount; i++)
                    {
                        if (cameraDic.ContainsKey(i + 1) == false)
                        {
                            //Util.Notify(string.Format("请注意相机{0}未连接", i+1));
                            MessageHelper.ShowError(string.Format("相机{0}没有找到", i + 1));
                        }
                    }

                    foreach (int key in cameraDic.Keys)
                    {
                        if (key > cameraCount || key < 1)
                        {
                            cameraDic.Remove(key);
                            Util.Notify(Level.Err, $"相机{key}名称ID超出相机个数设定范围,请用相机软件重新设定");
                            return;
                        }
                    }
                    Util.Notify("相机打开完成");

                    foreach (var item in ToolsFactory.ToolsDic.Values)
                    {
                        Tools.CreateImage.CreateImageTool createImageTool = item[0] as Tools.CreateImage.CreateImageTool;
                        createImageTool.OffLineMode = false;
                    }

                    LoadProject(false);
                    CommHandle.Instance.CommunicationParam = UserSetting.Instance.MainDeviceComParam;  //两对象同是CommunicationParam类型。
                    CommHandle.Instance.Open();

                }
                else
                {
                    if (MessageBox.Show("确定中止运行吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        CommHandle.Instance.IsLink = false;
                        foreach (var item in CameraManger.CameraDic.Values)
                        {
                            item.ContinuousShotStop();
                            if (item.IsContinuousShot)
                            {
                                item.ContinuousShotStop();
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {

            }
            
        }

        private void tsbCamera1Setting_Click(object sender, EventArgs e)
        {
            if (!IsAdministrator)
            {
                Util.Notify(Level.Err, "非管理员不能操作");
                return;
            }

            if (cameraSelect == null)
            {
                MessageBox.Show("当前无相机选择/当前相机未连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //if (cameraSelect is Camera.DirectShowCamera)
            //{
            //    MessageBox.Show("usb相机无参数设置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            frmCameraSetting settingForm = new frmCameraSetting(cameraSelect);
            DialogResult result = settingForm.ShowDialog();
            CameraPram cameraPram = ProjectData.Instance.GetCameraPram(cameraSelect.CameraIndex); //这里对ProjectData中的cameraPramDic进行更改
            if (result == DialogResult.OK)
            {
                cameraPram.Shutter = cameraSelect.ShuterCur;
                cameraPram.Gain = cameraSelect.GainCur;
                cameraPram.TriggerDelayAbs = cameraSelect.TriggerDelayAbs;
                cameraPram.LineDebouncerTimeAbs = cameraSelect.LineDebouncerTimeAbs;
                cameraPram.OutLineTime = cameraSelect.OutLineTime;
                cameraPram.ImageAngle = cameraSelect.ImageAngle;
                //camera1.SetExtTrigger();
                try
                {
                    ProjectData.Instance.SaveProject(UserSetting.Instance.ProjectPath);
                    UserSetting.Instance.SaveSetting();
                    SetNotifyMessage(string.Format("相机设置已保存"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存失败:" + ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                cameraSelect.ShuterCur = cameraPram.Shutter;
                cameraSelect.GainCur = cameraPram.Gain;
                cameraSelect.TriggerDelayAbs = cameraPram.TriggerDelayAbs;
                cameraSelect.LineDebouncerTimeAbs = cameraPram.LineDebouncerTimeAbs;
                cameraSelect.OutLineTime = cameraPram.OutLineTime;
                cameraSelect.ImageAngle = cameraPram.ImageAngle;
                //camera1.SetExtTrigger();
            }
        }

        private void tsbVido_Click(object sender, EventArgs e)
        {
            if (CommHandle.Instance.IsLink)
            {
                Util.Notify(Level.Err, "正在运行中不能连续采集图像!");
                return;
            }
            if (isTestMode == true)
            {
                //不能弹窗,会造成窗体不响应
                //MessageBox.Show("连续测试中 ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cameraSelect == null || !cameraSelect.IsLink)
            {
                MessageBox.Show($"相机{tscobCameraSelect.SelectedIndex + 1}未连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            //修改关联的运行按钮状态
            btnRun.Text = "运行";
            btnRun.BackColor = SystemColors.Control;
            btnRun.Visible = true;
            //cameraSelect.OneShot();
            //return;
            if (cameraSelect.IsContinuousShot)
            {
                cameraSelect.ContinuousShotStop();
                //camera1.SetExtTrigger();
                timerContinuousShotEnd.Stop();
            }
            else
            {
                if (CameraShowUnitDic.ContainsKey(cameraSelect.CameraIndex))
                    CameraShowUnitDic[cameraSelect.CameraIndex].HWndUnit.IsResultLabelDisplay = false;
                cameraSelect.ContinuousShot();
                if (timerContinuousShotEnd.Enabled == false)
                    timerContinuousShotEnd.Start();
            }
        }

        private void tsbShowAll_Click(object sender, EventArgs e)
        {
            if (CameraShowUnitDic[tscobCameraSelect.SelectedIndex+1].HWndUnit.Parent == panelHWndMax)
                ChangeSize(-1);
            else
                ChangeSize(tscobCameraSelect.SelectedIndex + 1);
        }

        private void btnAlarmDelete_Click(object sender, EventArgs e)
        {
            if (timerAlarmTextCycle.Enabled)
                timerAlarmTextCycle.Stop();
            alarmtextindex = 0;
            AlarmTextList.Clear();
            this.BeginInvoke(new Action(() =>
            {
                toolStripLabelAlarmDisplay.Text = "";
                toolStripLabelAlarmDisplay.Visible = false;
            }));
        }
        /// <summary>
        /// 选择相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscobCameraSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectIndex = tscobCameraSelect.SelectedIndex + 1;
            ChangeSize(selectIndex);
        }
        /// <summary>
        /// 测试、取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbCamera_Click(object sender, EventArgs e)
        {
            if (CommHandle.Instance.IsLink)
            {
                Util.Notify(Level.Err, "正在运行中不能测试！");
                return;
            }
            if (StatusManger.Instance.TestInitFinish == false)
            {
                MessageBox.Show("请等待初始化完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Tools.CreateImage.CreateImageTool createImageTool = ToolsFactory.GetToolList(currentSettingIndex)[0] as Tools.CreateImage.CreateImageTool;
            int cameraIndex = createImageTool.CameraIndex;
            //如果是连续拍照，则停止连续拍照并返回。
            if(CameraManger.CameraDic.ContainsKey(cameraIndex))
            {
                if (CameraManger.CameraDic[cameraIndex].IsContinuousShot)
                {
                    CameraManger.CameraDic[cameraIndex].ContinuousShotStop();
                    isTestMode = false;
                    timerContinuousShotEnd.Stop();
                    //tscobToolGroupsSelect.Enabled = true;
                    return;
                }
            }
            //判断是否是离线测试。判断对应相机对应runTheadData是否是离线测试模式。如果已处于离线测试模式，则停止离线测试。如不是则判断对应路径是否有图片，有则运行离线图片测试。
            if (ToolsFactory.ToolsDic.ContainsKey(currentSettingIndex))
            {                
                if (createImageTool != null && createImageTool.OffLineMode == true)
                {
                    if (runTheadDataDic.ContainsKey(cameraIndex))
                    {
                        if (runTheadDataDic[cameraIndex].IsOffLineMode)    //表示已经开始执行离线程序，这时要执行关闭离线执行。
                        {
                            runTheadDataDic[cameraIndex].IsOffLineMode = false;
                            createImageTool.IsStopOffLineTest = true;
                            return;
                        }
                        else                                              //表示还没有开始执行离线模式，这时执行之前，要判断图像列表是否为空。
                        {
                            if (createImageTool.GetOffLineImageFileSum() == 0)
                            {
                                Util.Notify(Level.Err, string.Format("工具组{0}没有加载离线图像文件", currentSettingIndex));
                                return;
                            }
                            createImageTool.AllReadFinish = false;
                            createImageTool.IsStopOffLineTest = false;
                            runTheadDataDic[cameraIndex].TrigerRun(currentSettingIndex, true);                  //开始执行离线模式触发。
                            return;
                        }
                    }
                    else
                    {
                        Util.Notify(Level.Err, string.Format("工具组{0}对应相机没有分配显示窗口!", currentSettingIndex));
                        return;
                    }
                }
            }
            //在线测试模式，相机已连接，连续拍照，放大窗口，开启计时。
            if (CameraManger.CameraDic.ContainsKey(cameraIndex))
            {
                if(CameraManger.CameraDic[cameraIndex].IsLink)
                {
                    if (CameraManger.CameraDic[cameraIndex].IsContinuousShot == false)
                    {
                        //runTheadDataDic[currentSettingIndex].TrigerRun(currentSettingIndex, false);//单次测试触发运行用，这时cameraSelect.ContinuousShot()要注释掉
                        tscobCameraSelect.SelectedIndex = cameraIndex - 1;
                        CameraManger.CameraDic[cameraIndex].ContinuousShot();
                        isTestMode = true;
                        //tscobToolGroupsSelect.Enabled = false;
                        if (timerContinuousShotEnd.Enabled == false)
                            timerContinuousShotEnd.Start();
                    }
                }
                else
                {
                    Util.Notify(Level.Err, string.Format("工具集{0}对应相机{1}已断线，请检查", currentSettingIndex, cameraIndex));
                    return;
                }
            }
            else if(cameraIndex > cameraCount)
            {
                Util.Notify(Level.Err, string.Format("工具组{0}对应相机没有分配显示窗口!", currentSettingIndex));
            }
            else
            {
                Util.Notify(Level.Err, string.Format("工具集{0}对应相机没有连接", currentSettingIndex));
            }
        }
        /// <summary>
        /// 抓拍图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSnap_Click(object sender, EventArgs e)
        {
            if (CommHandle.Instance.IsLink)
            {
                Util.Notify(Level.Err, "正在运行中不能抓图！");
                return;
            }

            if (StatusManger.Instance.TestInitFinish == false)
            {
                MessageBox.Show("请等待初始化完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cameraSelect == null)
            {
                MessageBox.Show($"相机{tscobCameraSelect.SelectedIndex + 1}未连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cameraSelect.IsContinuousShot)
            {
                cameraSelect.ContinuousShotStop();
                return;
            }

            if (CameraShowUnitDic.ContainsKey(cameraSelect.CameraIndex))
                CameraShowUnitDic[cameraSelect.CameraIndex].HWndUnit.IsResultLabelDisplay = false;

            HImage ho_image = new HImage();
            ho_image.Dispose();           
            try
            {                
                ho_image = cameraSelect.GrabImage(1500);
            }
            catch
            {
                Util.Notify(Level.Err, "采集图像异常");
            }

            foreach (int key in CameraManger.CameraDic.Keys)
            {
                if (cameraSelect == CameraManger.CameraDic[key])
                {
                    if (CameraShowUnitDic.ContainsKey(key))
                    {
                        try
                        {
                            CameraShowUnitDic[key].HWndUnit.HWndCtrl.AddIconicVar(ho_image);
                            CameraShowUnitDic[key].HWndUnit.HWndCtrl.Repaint();
                        }
                        catch
                        {
                            Util.Notify(Level.Err, "图像显示异常");
                        }
                    }

                    break;
                }
            }
        }
        /// <summary>
        /// 选择工具组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscobToolGroupsSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tscobToolGroupsSelect.Items.Count > 0)
            {
                try
                {
                    string str = tscobToolGroupsSelect.SelectedItem.ToString().Substring(3);
                    int toolsettingkey = Convert.ToInt16(str);
                    if (toolsettingkey > 0)
                        currentSettingIndex = toolsettingkey;
                }
                catch
                {
                    Util.Notify(Level.Err, "工具组选择框当前项转换失败");
                }
            }
        }
        #endregion

        #region 其它功能辅助函数
        private void StopExtTrigger()
        {
            //StatusManger status1 = StatusManger.Instance;
            foreach (var item in CameraManger.CameraDic.Values)
            {
                item.ContinuousShotStop();
            }

            //btnRun.Text = "运行";
            //btnRun.BackColor = SystemColors.Control;
            //status1.RuningStatus = RuningStatus.等待运行;
            Util.Notify("外触发关闭");
            //Thread.Sleep(100);
        }

        private void StartExtTrigger()
        {
            StatusManger status1 = StatusManger.Instance;
            foreach (var item in CameraManger.CameraDic.Values)
            {
                item.SetExtTrigger();
            }
            CameraBase camera = CameraManger.CameraDic.Values.ToList().Find(x => x.IsExtTrigger == false);
            if (camera == null && CameraManger.CameraDic.Count > 0)
            {
                //btnRun.Text = "运行中";
                //btnRun.BackColor = Color.Green;
                //status1.RuningStatus = RuningStatus.系统运行中;
                SetNotifyMessage(string.Format("等待外部触发"));
            }
            else
            {
                if (camera == null)
                {
                    SetNotifyMessage(string.Format("相机未连接,无法开始运行"));
                }
                else
                {
                    SetNotifyMessage(string.Format("相机{0}开始外触发运行异常", camera.CameraIndex));
                }

                btnRun.Text = "运行";
                btnRun.BackColor = Control.DefaultBackColor;
            }
        }

        //public void ShowErr(string msg, bool isErase = false)
        //{
        //    if (isErase)
        //    {
        //        toolStripAlarmDisplay.Text = "";
        //        toolStripAlarmDisplay.Visible = false;
        //        return;
        //    }
        //    toolStripAlarmDisplay.Text = msg;
        //    toolStripAlarmDisplay.Visible = true;
        //}

        public void RefreshUI(RunStatus runStatus)
        {
            //ShowStatus( runStatus);
            updateUI.Update(runStatus);
        }
        private void SaveResult(int index, string path)
        {
            HTuple start;
            HOperatorSet.CountSeconds(out start);
            dtResult.Rows.Clear();

            try
            {
                DataRow dr = dtResult.NewRow();
                dr["时间"] = DtResultShow.Rows[index]["时间"]; //通过名称赋值
                dr["相机"] = DtResultShow.Rows[index]["相机"];
                dr["正常计数"] = DtResultShow.Rows[index]["正常计数"];
                dr["失败计数"] = DtResultShow.Rows[index]["失败计数"];
                dr["异常率"] = DtResultShow.Rows[index]["异常率"];
                dr["节拍时间"] = DtResultShow.Rows[index]["节拍时间"];

                dtResult.Rows.Add(dr);
                Common.FileAct.CsvFiles.AppendWriteCSV(dtResult, path);

                //showTime(index, start, "数据保存完成");
            }
            catch (Exception ex)
            {
                Util.WriteLog(this.GetType(), ex);
                SetNotifyMessage(string.Format("运行结果保存出现异常"));
            }
        }

        #endregion

        #region 定时器响应函数区域
        private void timerInit_Tick(object sender, EventArgs e)
        {
            if (cameraCount != 1)
            {
                ChangeSize(-1);
            }
            timerInit.Enabled = false;
            //pictureUnit1.ShowScreenshots();
        }
        private void timerContinuousShotEnd_Tick(object sender, EventArgs e)
        {
            timerContinuousShotEnd.Stop();  //定时器中止一定要放在定时器函数的最顶端，否则有可能会引起死循环。

            cameraSelect.ContinuousShotStop();
            isTestMode = false;                      
        }
        private void TimerAlarmTextCycle_Tick(object sender, EventArgs e)
        {
            timerAlarmTextCycle.Stop();
            int i = AlarmTextList.Count;
            if (i > 0)
            {
                alarmtextindex++;
                if (alarmtextindex >= i || alarmtextindex < 0)
                    alarmtextindex = 0;
                this.BeginInvoke(new Action(() =>
                {
                    toolStripLabelAlarmDisplay.Text = AlarmTextList[alarmtextindex];
                    toolStripLabelAlarmDisplay.Visible = true;
                }));
                timerAlarmTextCycle.Start();
            }
            else
            {
                alarmtextindex = 0;
                toolStripLabelAlarmDisplay.Owner.BeginInvoke(new Action(() =>
                {
                    toolStripLabelAlarmDisplay.Text = "";
                    toolStripLabelAlarmDisplay.Visible = false;
                }));
            }
        }

        #endregion

    } //class end;
}  //namespace end;
