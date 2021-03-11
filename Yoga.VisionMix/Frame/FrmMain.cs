using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yoga.Camera;
using Yoga.Common;
using Yoga.Common.Basic;
using Yoga.Common.Helpers;
using Yoga.VisionMix.Units;
using System.Threading;
using Yoga.VisionMix.AppManger;

namespace Yoga.VisionMix.Frame
{
    public partial class FrmMain : Form
    {
        #region 构造函数
        AutoUnit autoUnit;
        bool isAdminstrator;                
        public FrmMain()
        {
            InitializeComponent();
            Util.WriteLog("Log测试-information");
            Util.WriteLog(typeof(FrmMain), "Log测试-error");
            this.Size = new System.Drawing.Size(IniStatus.Instance.WindowWidth, IniStatus.Instance.WindowHeigth);

            //this.StartPosition = FormStartPosition.Manual; //窗体的位置由Location属性决定
            //this.Location = (Point)new Size(0, 0);         //窗体的起始位置为(x,y)            

            autoUnit = new AutoUnit(this);
            lblCommStatus.Text = "";
            lblUserMode.Text = "";
            autoUnit.BorderStyle = BorderStyle.None;
            autoUnit.Dock = DockStyle.Fill;
            this.panelMain.Controls.Add(autoUnit);

            string path = Environment.CurrentDirectory + "\\project\\";
            FileInfo fi = new FileInfo(path);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            openProjectFileDialog.InitialDirectory = path;
            saveProjectFileDialog.InitialDirectory = path;
            tsCuurrentProject.Text = Path.GetFileNameWithoutExtension(UserSetting.Instance.ProjectPath);

            bool login = false;
            bool isDebug = false;
            isAdminstrator = false;
#if DEBUG
            login = true;
            isDebug = true;
            isAdminstrator = true;
#endif
            LoginSetting(login);
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string time = File.GetLastWriteTime(this.GetType().Assembly.Location).ToString("yyyy_MM_dd");

            string v1 = " V" + version + "_" + time;
            string debugFlag = isDebug ? "测试版" : "";
            this.Text = $"HRG VisionMix {v1} {debugFlag}";
        }
        
        #endregion

        #region 窗体加载函数
        /// <summary>
        /// 打开相机，加载工程文件，通信参数设定，打开通讯。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            //Common.UI.FrmLoad frmLoad = new Common.UI.FrmLoad(Application.StartupPath + @"\Res\Loading\");
            //Common.UI.Parameter.Name = frmAbout.AssemblyProduct;
            //Common.UI.Parameter.Year = DateTime.Now.ToString("yyyy");
            //Common.UI.Parameter.Version = frmAbout.AssemblyVersion;
            //Common.UI.Parameter.CopyRight = frmAbout.AssemblyCopyright;
            //Common.UI.Parameter.CompanyName = frmAbout.AssemblyCompany;
            //Common.UI.Parameter.Status = "初始化中...";
            //Common.UI.Parameter.StatusNum = 0;
            //frmLoad.Show();

            //frmLoad.UpdateProgress("相机加载...", 0, 30, 500);
            //frmWait.AsyncMethod += ((obj, args) =>
            {

                //注册认证
#if !DEBUG
            
            //string key = "";
            //key = Common.RegisterHelper.getMNum();
            // if (UserSetting.Instance.SoftKey == null || UserSetting.Instance.SoftKey != key)
            //{
            //    if (UserSetting.Instance.SoftKey == null)
            //    {
            //        MessageBox.Show("验证码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //        //      Common.Util.Notify("验证码错误");
            //    }
            //    else
            //    {
            //        // MessageBox.Show(str, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //    frmRegistered registered = new frmRegistered();
            //    registered.ShowDialog();
            //}
            //if (UserSetting.Instance.SoftKey == null || UserSetting.Instance.SoftKey != key)
            //{
            //    // 终止此进程并为基础操作系统提供指定的退出代码。
            //    System.Environment.Exit(1);
            //}
#endif
                bool isCameraOpenSuccess=false;
                
                try
                {
                    //打开相机
                    if (CameraManger.Open())
                    {                        
                        int cameraCount = IniStatus.Instance.CamearCount;
                        Dictionary<int, CameraBase> cameraDic = CameraManger.CameraDic;
                        for (int i = 0; i < cameraCount; i++)
                        {
                            if (cameraDic.ContainsKey(i+1) == false)
                            {
                                MessageHelper.ShowError(string.Format("相机{0}没有找到", i + 1));                             
                            }
                        }
                        //查询相机名称是不是超出有效范围，如果超出，将其从字典中清除。
                        foreach (int key in cameraDic.Keys)
                        {
                            if (key > cameraCount || key<1)
                            {
                                //cameraDic.Remove(key);
                                Util.Notify(Level.Err,$"相机{key}已连接，但其名称ID超出相机个数设定范围，请通过相机软件修改");

                            }
                        }
                        isCameraOpenSuccess = true;
                        Util.Notify("相机打开完成");
                    }
                    else
                    {
                        StatusManger.Instance.RuningStatus = RuningStatus.系统异常;
                        Util.Notify(Level.Err, "相机打开失败");
                    }
                }
                catch (Exception)
                {
                    MessageHelper.ShowError("相机打开出现异常");
                    StatusManger.Instance.RuningStatus = RuningStatus.系统异常;
                    Util.Notify(Level.Err, "相机打开出现异常");
                }
                //加载工程文件
                autoUnit.LoadProject(true);     //放在此位置很重要，不能像以前一样放在最后。

                try
                {
                    //通信参数设定
                    CommHandle.Instance.CommunicationParam = UserSetting.Instance.MainDeviceComParam;  //两对象同是CommunicationParam类型。
                    //打开通讯
                    if (isCameraOpenSuccess)
                        CommHandle.Instance.Open();
                }
                catch (Exception)
                {
                    StatusManger.Instance.RuningStatus = RuningStatus.系统异常;
                    Util.Notify(Level.Err, "通信端口打开异常");
                }

                this.WindowState = FormWindowState.Maximized;
            }
        }
        #endregion

        #region 窗体关闭函数
        private void MainFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(isAdminstrator)
            {
                DialogResult result = MessageBox.Show("即将退出软件,是否保存当前设置?", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                //新增取消软件关闭功能
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (result == DialogResult.Yes)
                {
                    try
                    {
                        AppManger.ProjectData.Instance.SaveProject(UserSetting.Instance.ProjectPath);
                    }
                    catch (Exception ex)
                    {
                        //Util.WriteLog(this.GetType(), ex);
                        string message = ex.Message;
                        MessageBox.Show("工程数据保存失败" + message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("确定要退出软件吗?", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }                    
            }
            autoUnit.save_data_result();
            //Application.Exit();
            CameraManger.Close();
            CommHandle.Instance.Close();
            // PLC.PanasonicPLCComm.Instance.CloseCom();
            //关闭所有相机处理线程
            foreach (var item in autoUnit.runTheadDataDic.Values)
            {
                item.Stop();
            }            
            autoUnit.RunCommWriteDataThread.Stop();
            autoUnit.RunCommReadDataThread.Stop();
            Thread.Sleep(30);
            //关闭所有线程-此处有点暴力-待删除
            Process.GetCurrentProcess().Kill();
            Application.ExitThread();
        }
        #endregion

        #region 事件响应函数
        /// <summary>
        /// 窗体中KeyDown响应事件函数(键盘Ctrl+S)
        /// </summary>
        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
            {
                e.Handled = true;       //将Handled设置为true，指示已经处理过KeyDown事件   
                保存项目ToolStripMenuItem.PerformClick(); //执行单击button1的动作   
            }
        }

        /// <summary>
        /// U盘拔响应事件函数--必须放在主Form中
        /// </summary>
        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == 0x0219)
        //    {//设备被拔出
        //        if (m.WParam.ToInt32() == 0x8004)   //usb串口拔出
        //        {
        //            if(CommHandle.Instance.SerialHelper != null)
        //            {
        //                if (!CommHandle.Instance.SerialHelper.Com.IsOpen
        //                && CommHandle.Instance.IsLink)        //判断是不是串口关闭
        //                {
        //                    CommHandle.Instance.IsLink = false;
        //                    Util.Notify(Level.Err, "串口被拔掉请重新连接");
        //                }
        //            }                  
        //        }
        //    }
        //    base.WndProc(ref m);
        //}
        #endregion

        #region 状态信息显示函数
        public void ShowErr(string msg,bool isErase=false)
        {
            if(isErase)
            {
                tsErrMessage.Text = "";
                tsErrMessage.BackColor = DefaultBackColor;
                return;
            }
            tsErrMessage.Text = msg;
            tsErrMessage.BackColor = System.Drawing.Color.Red;
        }

        public void ShowCommStatus(bool islink)
        {
            string interlockMode = EnumHelper.GetDescription(CommHandle.Instance.CommunicationParam.InterlockMode);
            if (CommHandle.Instance.CommunicationParam.IsExtTrigger)
            {
                lblCommStatus.Text = string.Format("|通信模式:{0}|相机外触发", interlockMode);
            }
            else
            {
                lblCommStatus.Text = string.Format("|通信模式:{0}", interlockMode);
            }
            if (islink)
                lblCommStatus.BackColor = System.Drawing.Color.Lime;
            else
                lblCommStatus.BackColor = System.Drawing.Color.Red;
        }
        #endregion

        #region 定时器-时间刷新函数
        private void timerTick_Tick(object sender, EventArgs e)
        {
            tsTime.Text = DateTime.Now.Date.ToString("yyyy/MM/dd") + " " + DateTime.Now.ToLongTimeString().ToString();
        }
   
        #endregion

        #region 下拉菜单响应函数
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }

        private void 打开项目ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CommHandle.Instance.IsLink)
            {
                Util.Notify(Level.Err, "运行中不能打开新项目");
                return;
            }
            if (autoUnit.IsTestMode)
            {
                Util.Notify(Level.Err, "正在测试中不能打开新项目");
                return;
            }
            string files;          
            if (openProjectFileDialog.ShowDialog() == DialogResult.OK)
            {
                files = openProjectFileDialog.FileName;
                UserSetting.Instance.ProjectPath = files;
                tsCuurrentProject.Text = Path.GetFileNameWithoutExtension(UserSetting.Instance.ProjectPath);
                autoUnit.LoadProject(true);
                autoUnit.btnRun_Click(null, null);
            }
        }
        private async void 项目另存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CommHandle.Instance.IsLink)
            {
                Util.Notify(Level.Err, "运行中不能另存项目");
                return;
            }
            if (autoUnit.IsTestMode)
            {
                Util.Notify(Level.Err, "正在测试中不能另存项目");
                return;
            }

            string files;
            if (saveProjectFileDialog.ShowDialog() == DialogResult.OK)
            {
                files = saveProjectFileDialog.FileName;
                await Task.Run(() =>
                {
                    try
                    {
                        AppManger.ProjectData.Instance.SaveProject(files);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("保存失败:" + ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });
                UserSetting.Instance.ProjectPath = files;
                tsCuurrentProject.Text = Path.GetFileNameWithoutExtension(UserSetting.Instance.ProjectPath);
                autoUnit.LoadProject(false);
                autoUnit.btnRun_Click(null, null);
            }
        }
        private async void 保存项目ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CommHandle.Instance.IsLink)
            {
                Util.Notify(Level.Err, "运行中不能保存项目");
                return;
            }
            if (autoUnit.IsTestMode)
            {
                Util.Notify(Level.Err, "正在测试中不能保存项目");
                return;
            }
            if (!autoUnit.IsAdministrator)
            {
                Util.Notify(Level.Err, "非管理员不能操作");
                return;
            }
            await Task.Run(() =>
            {
                try
                {
                    AppManger.ProjectData.Instance.SaveProject(UserSetting.Instance.ProjectPath);
                    //LoadProject(true);
                    //ToolsFactory.ReadTools(UserSetting.Instance.ProjectPath);

                    this.Invoke(
                  (MethodInvoker)delegate
                  {
                      UserSetting.Instance.SaveSetting();
                      //InitResultTablte();
                      MessageBox.Show("数据保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  }
                            );
                }
                catch (Exception ex)
                {
                    Util.WriteLog(this.GetType(), ex);
                    string message = ex.Message;
                    MessageBox.Show(message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }/* if */

        private void 通信设定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!autoUnit.IsAdministrator)
            {
                Util.Notify(Level.Err, "非管理员不能操作");
                return;
            }
            frmCommSetting frm = new frmCommSetting();
            frm.ShowDialog();
            //autoUnit.RefreshBtnRunVisible();
        }

        private void 登陆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isAdminstrator = false;
            frmLogin Frame_Login = new frmLogin();
            bool isLogin = false;
            if (Frame_Login.ShowDialog() != DialogResult.OK)
            {
                isLogin = false;
                isAdminstrator = false;
                //_pMainFrame.Frame_Show.FrameAuto.LoginSetting(false);
                //btnSetting.Enabled = false;
                //return;
            }
            else
            {
                isLogin = true;
                isAdminstrator = true;
                //timerLogin.Interval = 1000 * 60 * IniStatus.Instance.LogionDelay;
                //timerLogin.Enabled = true;
            }
            LoginSetting(isLogin);
        }

        private void LoginSetting(bool isLogin)
        {
            autoUnit.LoginSetting(isLogin);
            设置ToolStripMenuItem.Enabled = isLogin;
            string mode = isLogin ? "管理员" : "操作员";
            lblUserMode.Text = $"|权限:{mode}";
            lblUserMode.BackColor = isLogin ? System.Drawing.Color.Lime : System.Drawing.Color.Yellow;
        }

        private void 项目管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!autoUnit.IsAdministrator)
            {
                Util.Notify(Level.Err, "非管理员不能操作");
                return;
            }
            string prjOldName = UserSetting.Instance.ProjectPath;
            frmProjectManger frm = new frmProjectManger();
            frm.ShowDialog();
            if (UserSetting.Instance.ProjectPath == prjOldName)
            {
                return;
            }

            autoUnit.LoadProject(true);
            tsCuurrentProject.Text = Path.GetFileNameWithoutExtension(UserSetting.Instance.ProjectPath);
        }

        private void 清除统计数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!autoUnit.IsAdministrator)
            {
                Util.Notify(Level.Err, "非管理员不能操作");
                return;
            }
            if (autoUnit.IsTestMode)
            {
                Util.Notify(Level.Err, "正在离线测试中不能清除数据");
                return;
            }
            if (CommHandle.Instance.IsLink)
            {
                Util.Notify(Level.Err, "正在运行中不能清除数据");
                return;
            }
            autoUnit.btnDeleteResult_Click();
        }
        #endregion
    }
}
