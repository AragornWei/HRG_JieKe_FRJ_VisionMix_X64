using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yoga.Camera;
using Yoga.Common;
using Yoga.Common.Basic;
using Yoga.ImageControl;
using Yoga.Tools;
using Yoga.Tools.Factory;
using Yoga.VisionMix.AppManger;

namespace Yoga.VisionMix.Scheduling
{
    public class RunTheadData
    {
        Units.AutoUnit autoUnit;
        ImageEventArgs imageData;
        Thread CameraRunThread;

        bool isRun = true;
        bool isOffLineMode = false;
        bool isExtTrigger = false;
        bool isGrabber = false;

        double startExTime = 0;
        protected Fps fps = new Fps();
        private Stopwatch stopWatch = new Stopwatch();

        /// <summary>
        /// 控制线程运行标志
        /// </summary>
        AutoResetEvent threadRunSignal = new AutoResetEvent(false);

        int settingIndex = 1;
        int cameraIndex = 1;
        AutoResetEvent imageRunFinishSignal = new AutoResetEvent(true);

        AutoResetEvent imageRunFinishSignalFlow = new AutoResetEvent(true);
        /// <summary>
        /// 指示运行结果是否存在ng
        /// </summary>
        private bool runningResultFlag = false;

        Units.CameraShow cameraShowUnit;

        int ok = 0, ng = 0;

        ToolBase toolErr;
        private object lockObj = new object();
        const string toolDelimiter = " || ";
        public RunTheadData(Units.AutoUnit autoUnit,int cameraIndex)
        {
            this.autoUnit = autoUnit;
            this.cameraIndex = cameraIndex;
            
            settingIndex = cameraIndex;  //先暂时默认工具集号和相机号相同，实际是根据赋值。
            cameraShowUnit = autoUnit.CameraShowUnitDic[cameraIndex];
            CameraRunThread = new Thread(new ThreadStart(Run));
            CameraRunThread.IsBackground = true;
            CameraRunThread.Start();
            stopWatch.Reset();
        }
        public void BindCameraEvent(CameraBase camera)
        {
            camera.ImageEvent += Camera_ImageEvent;
        }
        public void ResetCount()
        {
            ok = 0;
            ng = 0;
            fps.Reset();
        }
        /// <summary>
        /// 当前图像处理完成标志信号
        /// </summary>
        public AutoResetEvent ImageRunFinishSignal
        {
            get
            {
                return imageRunFinishSignal;
            }
            protected set
            {
                imageRunFinishSignal = value;
            }
        }

        public ImageEventArgs ImageData
        {
            get
            {
                lock (lockObj)
                {
                    return imageData;
                }
            }

            set
            {
                lock (lockObj)
                {
                    imageData = value;
                }
            }
        }

        public bool IsOffLineMode
        {
            get
            {
                return isOffLineMode;
            }
            set
            {
                isOffLineMode = value;
            }
        }

        /// <summary>
        /// 当IO触发或(在线测试)相机连续触发采集到图像时，开始执行图像处理
        /// </summary>
        public void TrigerRun(ImageEventArgs imageData, int waitTime, int settingIndex, bool isShowTimeOut)
        {
            this.settingIndex = settingIndex;
            //超时检测
            bool isDelay = (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 200);
            if (isDelay)
            {
                stopWatch.Restart();
            }
            //硬件触发模式不检测超时；在线检测模式检测是否超时(硬件触发isShowTimeOut= true直接跳过;在线检测为isShowTimeOut=false，判断时间间隔是否大于200ms，若大于200ms，则跳过执行下一步)。
            if (isShowTimeOut == false && isDelay==false)
            {
                return;
            }
            if (imageRunFinishSignalFlow.WaitOne(waitTime))
            {
                startExTime = imageData.StartTime.D;
                //清理图像内存
                if (this.ImageData != null)
                {
                    this.ImageData.Dispose();
                }
                this.ImageData = imageData;
                this.isOffLineMode = false;
                isExtTrigger = true;      //只要调用该函数，包括测试模式时，也是按外触发来执行的。
                threadRunSignal.Set();
            }
            else
            {
                imageData.Dispose();
                if (isShowTimeOut)
                {
                    string message = string.Format("工具组{0}图像处理超时发生,请检查程序运行节拍", settingIndex);
                    Util.Notify(Level.Err, message);
                }
                else
                {
                    //Thread.Sleep(50);
                }
            }
        }

        /// <summary>
        /// 离线模式时开始执行下张图片的处理
        /// </summary>
        public void TrigerRun(int settingKey, bool isOffLineMode)
        {
            HTuple time_temp;
            HOperatorSet.CountSeconds(out time_temp );
            startExTime = time_temp.D;
            if (imageRunFinishSignalFlow.WaitOne(1000))
            {
                settingIndex = settingKey;
                this.isOffLineMode = isOffLineMode;                
                isExtTrigger = false;
                threadRunSignal.Set();
            }
            else
            {
                string message = string.Format("工具组{0}图像处理超时发生,请检查程序运行节拍", settingIndex);
                Util.Notify(Level.Err, message);
            }
        }

        /// <summary>
        /// 当软触发(外部通信)采集到图像时，开始执行处理图像
        /// </summary>
        public void TriggerRun(ImageEventArgs imageData, int settingIndex,int waitTime)
        {
            if (this.ImageData != null)
            {
                this.ImageData.Dispose();
            }
            this.imageData = imageData;
            isGrabber = true;
            this.settingIndex = settingIndex;
            this.cameraIndex = imageData.CameraIndex;
            if (imageRunFinishSignalFlow.WaitOne(waitTime))
            {
                startExTime = imageData.StartTime.D;
                threadRunSignal.Set();
            }
         }

        public void Stop()
        {
            isRun = false;
            stopWatch.Reset();
            threadRunSignal.Set();    //当停机时避免让其处于无限等待中，所以要让其处于信号状态。
        }
        public void Run()
        {
            while (isRun)
            {
                threadRunSignal.WaitOne();
                if (isRun)
                {
                    RunAllTool();
                }
            }
        }

        public void ShowResult(HWndCtrl hWndCtrl, List<ToolBase> runToolList, bool runningResultFlag)
        {
            if (runningResultFlag == false)
            {
                hWndCtrl.ShowNg();
                if (IniStatus.Instance.SaveNgImage == 1)
                {
                    DateTime dt = DateTime.Now;
                    string timeNow = dt.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                    string project = Path.GetFileNameWithoutExtension(UserSetting.Instance.ProjectPath);
                    string NGImagePath = "D:\\data\\" + project + "\\NgImage\\" + "\\工具组" + settingIndex + "\\";
                    SaveImage(NGImagePath + timeNow + ".png", runToolList[0].ImageTestOut);
                }
            }
            else
            {
                hWndCtrl.ShowOK();
                if (IniStatus.Instance.SaveOkImage == 1)
                {
                    DateTime dt = DateTime.Now;
                    string timeNow = dt.ToString("yyyy_MM_dd_HH_mm_ss_fff");
                    string project = Path.GetFileNameWithoutExtension(UserSetting.Instance.ProjectPath);
                    string NGImagePath = "D:\\data\\" + project + "\\OKImage\\" + "\\工具组" + settingIndex + "\\";
                    SaveImage(NGImagePath + timeNow + ".png", runToolList[0].ImageTestOut);
                }
            }
            HTuple showStart1;
            HOperatorSet.CountSeconds(out showStart1);
            hWndCtrl.ChangeGraphicSettings(Mode.LINESTYLE, new HTuple());
            hWndCtrl.AddIconicVar(runToolList[0].ImageTestOut);
            foreach (ToolBase item in runToolList)
            {
                try
                {
                    item.ShowResult(hWndCtrl);
                    if (item.Message != null && item.Message.Length > 0)
                    {
                        Util.Notify(string.Format("工具{0}_{1}", item.Name, item.Message));
                    }
                }
                catch (Exception ex)
                {
                    Util.Notify(string.Format("工具{0}显示异常", item.Name)); //("工具{0}显示异常{1}", item.Name，e.message));
                    Util.WriteLog(this.GetType(), ex);
                }
            }            
            hWndCtrl.Repaint();
        }
        int fpsCount = 0;
        public void RunAllTool()
        {
            HTuple /*start = null, */end = null;
            //帧率统计增加
            fps.IncreaseFrameNum();
            fpsCount++;
            if (fpsCount > 10 )
            {
                fps.UpdateFps();
                fpsCount = 0;
            }
            RunStatus runStatus = new RunStatus(settingIndex, cameraIndex);
            runStatus.FpsStr = string.Format("FPS:{0:F1}|帧:{1}|", fps.GetFps(), fps.GetTotalFrameCount());

            List<ToolBase> runToolList = ToolsFactory.GetToolList(settingIndex);
            Tools.CreateImage.CreateImageTool createImageTool = runToolList[0] as Tools.CreateImage.CreateImageTool;
            HWndCtrl hWndCtrl = cameraShowUnit.HWndUnit.HWndCtrl;
            try
            {
                HTuple toolStart = new HTuple(), toolEnd = new HTuple();
                StatusManger statusManger = StatusManger.Instance;
                statusManger.RuningStatus = RuningStatus.图像检测中;
                runningResultFlag = false;
                toolErr = null;

                //开始运行所有工具
                HOperatorSet.CountSeconds(out toolStart);
                //外部触发处理
                if (isExtTrigger || isGrabber)
                {
                    createImageTool.SettExtTriggerData(ImageData);
                    isGrabber = false;
                }
                else
                {
                    createImageTool.SetExtTriggerDataOff();
                }
                StringBuilder MyStringBuilder = new StringBuilder();
                //string yy = MyStringBuilder.ToString();
                string datSend = "";

                foreach (var item in runToolList)
                {
                    if (item is IToolRun)
                    {
                        try
                        {
                            item.Run();
                            string result = item.IsOk ? "OK" : "NG";
                            Util.Notify(string.Format("{0}_{1} T={2:f2}ms,结果: {3}", item.Name, result, item.ExecutionTime, item.Result));
                            MyStringBuilder.Append(string.Format("{0}_{1}_T={2:f2}ms\r\n", item.Name, result, item.ExecutionTime));
                            if (item.IsOutputResults)
                            {
                                string dat = item.GetSendResult();
                                if (dat != string.Empty)
                                {
                                    datSend += dat;
                                    datSend += toolDelimiter;
                                }
                            }
                            runStatus.RunStatusList.Add(item.IsOk);
                        }
                        catch (Exception ex)
                        {
                            //Util.WriteLog(this.GetType(), ex);
                            Util.Notify(Level.Err, string.Format("工具{0}运行出现异常{1}", item.Name, ex.Message));
                            runStatus.RunStatusList.Add(false);
                        }
                    }
                    else
                    {
                        runStatus.RunStatusList.Add(true);
                    }
                }
                runStatus.ResultMessage = MyStringBuilder.ToString();

                //时间统计
                HOperatorSet.CountSeconds(out toolEnd);
                double toolTime = (toolEnd - toolStart) * 1000.0;   //toolStart) * 1000.0;
                Util.Notify(string.Format("工具组{0}图像处理用时{1:f2}ms", settingIndex, toolTime));
                #region 3 查找是否存在运行错误的工具
                toolErr = runToolList.Find(x => x.IsOk == false && x is IToolRun);

                if (toolErr == null && ToolsFactory.ToolsDic.Count > 0)
                {
                    runningResultFlag = true;
                    ok++;
                }
                else
                {
                    runningResultFlag = false;
                    ng++;
                    Util.Notify(string.Format("工具{0}图像处理检测到异常", toolErr.Name));
                }

                if (runningResultFlag == true)
                {
                    datSend = Util.TrimEndString(datSend, toolDelimiter);
                    datSend = Util.TrimStartString(datSend, toolDelimiter);
                }
                else
                {
                    datSend = ("Image" + Environment.NewLine + "Done" + Environment.NewLine); 
                }

                if (isOffLineMode == false)         //这个变量与AutoUnit中的isTestMode不是同一个，这里是指AutoUnit的测试模式与离线模式。
                {
                    //SerialHelper.Instance.WriteCommdToSerial(datSend);
                    //非相机输出模式下就直接输出文本信息
                    if (!CommHandle.Instance.CommunicationParam.IsCamIOOutput
                        && datSend != string.Empty)
                    {
                        Util.Notify(string.Format("发送结果: {0}", datSend));
                        autoUnit.RunCommWriteDataThread.WriteData(datSend);
                    }
                    else
                    {
                        if (StatusManger.Instance.IsInterlocking && CameraManger.CameraDic.ContainsKey(settingIndex) && runningResultFlag == false)
                        {
                            CameraManger.CameraDic[settingIndex].Output();   //结果NG时相机外部输出信号导通。
                        }
                    }
                }
                else
                {

                    if (runningResultFlag == false)
                    {
                        Util.Notify(string.Format("测试结果:{0}", "NG"));
                    }
                    else
                    {
                        Util.Notify(string.Format("测试结果:{0}", "OK"));
                    }

                }
                #endregion
                #region 4 显示所有的图形
                //HTuple showStart;
                //HOperatorSet.CountSeconds(out showStart);
                autoUnit.Invoke(new Action<HWndCtrl, List<ToolBase>, bool>((h, l, f) =>
              {
                  ShowResult(h, l, f);

              }), hWndCtrl, runToolList, runningResultFlag);

                #endregion

                HTuple end1;
                HOperatorSet.CountSeconds(out end1);
                double time1 = (end1 - toolEnd) * 1000.0;
                Util.Notify(string.Format("工具组{0}分析显示用时{1:f2}ms", settingIndex, time1));
            }
            catch (Exception)
            {
                //Util.WriteLog(this.GetType(), ex);
                Util.Notify(string.Format("图像处理异常"));
            }
            finally
            {
                HOperatorSet.CountSeconds(out end);
                double runTime = 0;
                runTime = (end - startExTime) * 1000.0;

                runStatus.OKCount = ok;
                runStatus.NgCount = ng;
                runStatus.CylceTime = runTime;
                RunStatus runStatusTmp = runStatus.DeepClone();
                autoUnit.RefreshUI(runStatusTmp);

                //指示可以来图像处理
                Util.Notify(string.Format("---工具组{0}运行完成,用时{1:f2}ms\r\n", settingIndex, runTime));
                imageRunFinishSignalFlow.Set();
                //离线模式
                if (isOffLineMode&&
                    createImageTool.OffLineMode == true&&
                    createImageTool.AllReadFinish==false)
                {

                    Task.Run(async delegate
                   {
                       await Task.Delay(1000);
                       if(isOffLineMode)
                           TrigerRun(this.settingIndex, true);   //离线测试模式时，接着触发下次离线测试。
                    });
                }
                else
                {
                    isOffLineMode = false;   //将该对象的离线模式标志复位，不再执行离线。
                }
            }
        }

        private void SaveImage(string files, HImage ngImage)
        {

            if (ngImage == null || ngImage.IsInitialized() == false)
            {
                Util.WriteLog(this.GetType(), "异常图像数据丢失");
                Util.Notify("异常图像数据丢失");
                return;
            }
            HImage imgSave = ngImage.CopyImage();
            Task.Run(() =>
            {
                try
                {
                    FileInfo fi = new FileInfo(files);
                    if (!fi.Directory.Exists)
                    {
                        fi.Directory.Create();
                    }

                    Common.FileAct.FileManger.DeleteOverflowFile(Path.GetDirectoryName(files), IniStatus.Instance.NgImageCount);
                    imgSave.WriteImage("png", 0, files);
                    imgSave.Dispose();
                }
                catch (Exception ex)
                {
                    Util.WriteLog(this.GetType(), ex);
                    Util.Notify(string.Format("相机{0}异常图像保存异常", settingIndex));
                }
            });

        }

        private void Camera_ImageEvent(object sender, ImageEventArgs e)
        {

            if (e.Command == Command.ExtTrigger)
            {
                //硬件触发
                TrigerRun(e, 1000, e.CameraIndex, true);
            }
            else if (e.Command == Command.Grab)
            {  //软触发
                TriggerRun(e, e.SettingIndex, 1000);
            }
            else
            {
                //#if DEBUG
                //  Util.Notify(string.Format("显示图像线程ID:{0}", Thread.CurrentThread.ManagedThreadId)); 
                //#endif
                //测试模式---如果换了工具组，就将上个正在测试的工具关闭。---很重要。
                if (autoUnit.IsTestMode)
                {
                    if (e.CameraIndex != autoUnit.toolShowUnit1.GetCameraIndex(autoUnit.CurrentSettingIndex))
                    {
                        if (CameraManger.CameraDic.ContainsKey(e.CameraIndex))
                        {
                            if (CameraManger.CameraDic[e.CameraIndex].IsContinuousShot)
                            {
                                CameraManger.CameraDic[e.CameraIndex].ContinuousShotStop();
                            }
                        }
                        return;
                    }
                    TrigerRun(e, 1, autoUnit.CurrentSettingIndex, false);                       
                }//在线测试模式
                else///连续采集图像，直接显示   
                {
                    if (ImageRunFinishSignal.WaitOne(1))
                    {
                        autoUnit.Invoke(new Action(() =>
                        {
                            //显示图像
                            HWndCtrl hWndCtrl = cameraShowUnit.HWndUnit.HWndCtrl;
                            HTuple t0;
                            HOperatorSet.CountSeconds(out t0);
                            hWndCtrl.ClearList();
                            hWndCtrl.AddIconicVar(e.CameraImage);
                            hWndCtrl.Repaint();
                            //Application.DoEvents();//不要使用liscence,否则不对就会造成卡顿严重//高帧率下显示会卡顿

                            string cameraInfo = CameraManger.CameraDic[e.CameraIndex].GetCameraAcqInfo();
                            cameraShowUnit.HWndUnit.CameraMessage = string.Format("相机{0} {1}", e.CameraIndex, cameraInfo);

                            ImageRunFinishSignal.Set();

                            e.Dispose();
                        }));
                    }
                    else
                    {
                        e.Dispose();
                        //if (this.InvokeRequired)
                        //{
                        //    Thread.Sleep(10);
                        //}
                    }
                }//连续采集图像，直接显示   
            }
        }

    }
}
