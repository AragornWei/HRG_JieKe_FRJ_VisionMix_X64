using System;
using System.IO.Ports;
using Yoga.Common.Basic;
using System.Threading;

namespace Yoga.Common.Helpers
{
    public class PortDataReciveEventArgs : EventArgs
    {
        public PortDataReciveEventArgs(string data)
        {
            this.data = data;
        }

        private string data;

        public string Data
        {
            get { return data; }
        }
    }

    public class SerialHelper
    {
        private SerialPort com = new SerialPort();
        bool isLink = false;
        //bool isTimeOutAlarm;
        string strReceivedData = "";
        public event EventHandler<PortDataReciveEventArgs> PortDataReceiveEvent;        
        public event SerialErrorReceivedEventHandler SerialErrorReceivedEvent;

        public bool IsLink
        {
            get
            {
                return isLink;
            }

            set
            {
                isLink = value;
            }
        }

        public SerialPort Com
        {
            get
            {
                if (com==null)
                {
                    com = new SerialPort();
                }
                return com;
            }

            set
            {
                com = value;
            }
        }
        public string StrReceivedData
        {
            get
            {
                return strReceivedData;
            }

            set
            {
                strReceivedData = value;
            }
        }
        /// <summary>
        /// 串口初始化
        /// </summary>
        public void InitSerial(CommunicationParam rs232Param)
        {
            try
            {
                if (Com.IsOpen)
                {
                    Close();
                }
                isLink = false;                              
                Com.PortName = rs232Param.ComName;
                Com.BaudRate = Convert.ToInt32(rs232Param.BaudRate);
                Com.Parity = (Parity)Convert.ToInt32(rs232Param.Parity);
                Com.DataBits = Convert.ToInt32(rs232Param.DataBits);
                Com.StopBits = (StopBits)Convert.ToInt32(rs232Param.StopBits);
                Com.ReadTimeout = 200;
                Com.WriteTimeout = 200;
                //Com.NewLine = "\r\n";
                //Com.NewLine = "\r";
                Com.Open();
                isLink = true;                

                Com.DataReceived += new SerialDataReceivedEventHandler(this.OnDataReceived);
                Com.ErrorReceived += new SerialErrorReceivedEventHandler(this.OnErrorReceived);                   
            }
            catch (Exception ex)
            {
                IsLink = false;
                Util.Notify(Level.Err, string.Format("串口{0}打开失败,",Com.PortName) + ex.Message);
                //throw new ApplicationException("外部通信串口打开失败," + ex.Message);
                //throw new Exception("外部通信串口打开失败," + ex.Message); //
                throw;
            }
        }
        /// <summary>
        /// 写入数据到串口
        /// </summary>
        /// <param name="str">待写入字符串-无校验</param>
        /// <returns></returns>
        public void WriteDataToSerial(string str)
        {
            try
            {
                if (Com.IsOpen == false)
                {
                    Util.Notify(Level.Err, "串口未打开");
                    //throw new ApplicationException("串口未打开");
                    return;
                }
                if(Com.BytesToWrite >0)
                {
                    bool iswritebufferempty = false;
                    int num = 0;
                    while (!iswritebufferempty && num < 5)
                    {
                        num++;
                        Thread.Sleep(10);
                        if (Com.BytesToWrite == 0)
                            iswritebufferempty = true;
                    }
                    if(!iswritebufferempty)
                    {
                        Util.Notify(Level.Err, "串口写缓冲区已满，发送数据超时");
                        return;
                    }
                }
                Com.Write(str);                
            }
            catch(Exception ex)
            {                
                Util.Notify(Level.Err, "串口写数据异常:" + ex.Message);
                //IsLink = false;
                //throw new ApplicationException("串口设置异常");
            }
        }
        //private object portLockObj = new object();
        /// <summary>
        /// 串口接收到数据引发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("串口反馈线程编号为:" + System.Threading.Thread.CurrentThread.ManagedThreadId);
            
            StrReceivedData = "";
            try
            {
                StrReceivedData = Com.ReadExisting();
                //StrReceivedData = Com.ReadLine();
            }
            catch (Exception ex)
            {
                Util.Notify(Level.Err, "串口读数据异常:" + ex.Message);
                return;
            }

            if (PortDataReceiveEvent != null && StrReceivedData.Length>0)
            {
                PortDataReceiveEvent(this, new PortDataReciveEventArgs(StrReceivedData));
            }
        }
        public void Close()
        {
            if (Com != null && Com.IsOpen)
            {
                try
                {
                    Com.DataReceived -= new SerialDataReceivedEventHandler(this.OnDataReceived);
                    Com.ErrorReceived -= new SerialErrorReceivedEventHandler(this.OnErrorReceived);
                    Com.Close();
                    Util.Notify(string.Format("串口{0}已关闭:", Com.PortName));
                }
                catch(Exception ex)
                {
                    Util.Notify(Level.Err, string.Format("串口{0}关闭异常:" ,Com.PortName)+ ex.Message);
                }
                
            }
                
            Com = null;
            IsLink = false;
        }
       
        private void OnErrorReceived(Object sender,SerialErrorReceivedEventArgs e)
        {
            if(SerialErrorReceivedEvent!=null)
            {
                SerialErrorReceivedEvent(sender, e);
            }
        }
    }
}
