using System;
using System.Timers;
using Yoga.Common;
using Yoga.Common.Helpers;
using System.Threading;
using Yoga.VisionMix.AppManger;

namespace Yoga.VisionMix.Scheduling
{
    public class RunComWriteDataThread
    {
        CommHandle commHandler;
        Thread commWriteThread;
        bool isRun = true;
        AutoResetEvent startWriteSigal = new AutoResetEvent(false);
        AutoResetEvent writeDoneSigal = new AutoResetEvent(true);
        string strwritedata;
        bool isPause;
        bool isTimeOutAlarm = true;  //如果该程序作为客户端，该变量初始化为true;
        bool isTiming;
        private System.Timers.Timer feedbacktimeout;    //不能用静态的，否则每个实例共用一个定时器，会出错的。
        public event EventHandler CommFeedbackTimeOut;
        int errorcount;
        const int maxerrornum = 3;

        public RunComWriteDataThread()
        {
            this.commHandler = CommHandle.Instance;
            this.commHandler.PortDataReceiveEvent += PortDataReceive_Event;
            if (commHandler != null)
            {
                commWriteThread = new Thread(new ThreadStart(Run));
                commWriteThread.IsBackground = true;
                commWriteThread.Start();

                feedbacktimeout = new System.Timers.Timer(500);
                feedbacktimeout.AutoReset = false;                //false:表示只产生一次Elapsed事件；true:表示重复的产生该事件
                feedbacktimeout.Elapsed += feedbacktimeout_Tick;                
            }
            else
            {
                Util.Notify(Common.Basic.Level.Err, "写线程传入的AppInterlockHelper实例为null");
            }            
        }
        public void Run()
        {
            while (isRun)
            {
                startWriteSigal.WaitOne();
                writeDoneSigal.Reset();
                if (isRun)
                {                    
                    commHandler.WriteData(strwritedata);
                    
                    //if (!isPause && !feedbacktimeout.Enabled)
                    //{
                    //    //Thread.Sleep(20);
                    //    //startWriteSigal.Set();
                    //}
                    writeDoneSigal.Set();
                }
            }
        }
        public void WriteData(string strwritedata)
        {
            isPause = true;  //放在最前面很关键，主要通过它来禁止Run中startWriteSigal自动置位
            startWriteSigal.Reset();  //这里起到第二层作用：在这一次的While循环中，startWriteSigal刚置位，而且是在下次的While循环之前。才有用                 
            if (strwritedata == string.Empty)
                return;
            if(writeDoneSigal.WaitOne(1000)) //确保上次的写完成了，才能发最新的指令数据。
            {
                this.strwritedata = strwritedata;
                startWriteSigal.Set();
                //isPause = false;    //本机当作客户端，并且需要连续采集时再启用。
            }
            else
            {
                Util.Notify(Common.Basic.Level.Err, "向线程写入新数据超时");
                return;
            }
            if (!isTiming && isTimeOutAlarm)
            {
                feedbacktimeout.Start();
                isTiming = true;
            }                
        }
        private void PortDataReceive_Event(object sender, PortDataReciveEventArgs e)
        {
            if(isTiming)
            {
                isTiming = false;
                feedbacktimeout.Stop();
            }
            errorcount = 0;            
        }
        private void feedbacktimeout_Tick(object sender, ElapsedEventArgs e)
        {
            feedbacktimeout.Stop();
            errorcount++;
            if(errorcount >= maxerrornum)
            {
                isPause = true;  //中断连续采集数据的WriteData()指令。
                if (CommFeedbackTimeOut != null)
                    CommFeedbackTimeOut(null, null);
                else
                    Util.Notify(Common.Basic.Level.Err, "通信反馈超时");
                
                return;
            }
            isTiming = false;
            //startWriteSigal.Set();
        }
        public void Stop()
        {
            isRun = false;
            //stopWatch.Reset();
            startWriteSigal.Set();    //当停机时避免让其处于无限等待中，所以要让其处于信号状态。
        }
    }
}
