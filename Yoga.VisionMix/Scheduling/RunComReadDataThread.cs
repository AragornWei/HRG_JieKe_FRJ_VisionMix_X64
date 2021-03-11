using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yoga.Common.Helpers;
using System.Threading;
using Yoga.VisionMix.AppManger;

namespace Yoga.VisionMix.Scheduling
{
    public class RunComReadDataThread
    {
        CommHandle commHandler;
        private string messageRecevied;
        private string messageRemained;
        Thread commReadThread;
        bool isRun = true;
        AutoResetEvent startReadSigal = new AutoResetEvent(false);
        string receiveCode;
        string messagetemp;
        //bool isReceive;
        public event EventHandler<PortDataReciveEventArgs> PortDataReceiveEvent;
        object m_objLock = new object();
         
        public RunComReadDataThread()
        {
            this.commHandler = CommHandle.Instance;
            this.commHandler.PortDataReceiveEvent += PortDataReceive_Event;
            commReadThread = new Thread(new ThreadStart(Run));
            commReadThread.IsBackground = true;
            commReadThread.Start();
        }

        public void Run()
        {
            while (isRun)
            {
                startReadSigal.WaitOne();
                if (isRun)
                {
                    OnDataReceive();
                }
            }
        }

        private void OnDataReceive()
        {
            MessageManager(false);         
            int end = messageRemained.IndexOf("#");//尾巴
            //int messagestringlenth;
            while (end > -1)
            {
                string dat = messageRemained.Substring(0, end);
                int st = dat.IndexOf("*");//头
                if (st > -1 && st < dat.Length - 1)
                {
                    //截取收到的数据
                    receiveCode = dat.Substring(st + 1, dat.Length - st - 1);
                    if (PortDataReceiveEvent != null && receiveCode != "")
                    {
                        PortDataReceiveEvent(this, new PortDataReciveEventArgs(receiveCode));
                    }
                }
                if (end == messageRemained.Length - 1)
                {
                    messageRemained = "";
                }
                else if (end < messageRemained.Length - 1)
                {
                    //截取end以后的数据重新接收数据
                    messageRemained = messageRemained.Substring(end + 1, messageRemained.Length - end - 1);
                }
                end = messageRemained.IndexOf("#");//尾巴
            }
            if(messageRemained.Length>10)
            {
                Common.Util.Notify(Common.Basic.Level.Err, "连续接收到太多错误指令");
                messageRemained = "";
            }
        }

        private void PortDataReceive_Event(object sender, PortDataReciveEventArgs e)
        {
            messagetemp = e.Data;
            //isReceive = true;
            MessageManager(true);
            messagetemp = "";
            startReadSigal.Set();
        }
        private void MessageManager(bool isReceive)
        {
            lock (m_objLock)
            {
                if(isReceive)
                {
                    messageRecevied += messagetemp;
                    //isReceive = false;
                }
                else
                {
                    messageRemained += messageRecevied;
                    messageRecevied = "";
                }
            }
        }
        public void Stop()
        {
            isRun = false;
            //stopWatch.Reset();
            startReadSigal.Set();    //当停机时避免让其处于无限等待中，所以要让其处于信号状态。
        }
    }
}
