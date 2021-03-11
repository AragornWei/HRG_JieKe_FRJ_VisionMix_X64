
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace JSONviaUDP

{
    public  class UDPHelper
    {

        private UdpClient udpcSend;//接受
        private UdpClient udpcRecv;//發送

        //private void UDP_SEND(byte[] sendbyte)
        //{
        //    //udpcSend = new UdpClient(0);             // 匿名發送，自动分配本地IPv4地址
        //    IPEndPoint localIpep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345); // 本机IP，指定的端口号
        //    udpcSend = new UdpClient(localIpep);
        //    Thread thrSend = new Thread(SendMessage);
        //    thrSend.Start(sendbyte);
        //}
/// <summary>
/// 發送信息
/// </summary>
/// <param name="TargetIP">目標IP</param>
/// <param name="sendbytes">發送數據</param>
        private void SendMessage(string TargetIP,byte[] sendbytes)
        {   
            IPEndPoint remoteIpep = new IPEndPoint(IPAddress.Parse(TargetIP), 6688); // 发送到的IP地址和端口号
            udpcSend.Send(sendbytes, sendbytes.Length, remoteIpep);
            udpcSend.Close();
        }
        /// <summary>
        /// 开关：在监听UDP报文阶段为true，否则为false
        /// </summary>
        bool IsUdpcRecvStart = false;
        /// <summary>
        /// 线程：不断监听UDP报文
        /// </summary>
        Thread thrRecv;

        private void ReceiveContinuously(string LocalIP,int port)
        {
            //if (!IsUdpcRecvStart) // 未监听的情况，开始监听
            //{
            //    IPEndPoint localIpep = new IPEndPoint(IPAddress.Parse(LocalIP), port); // 本机IP和监听端口号
            //    udpcRecv = new UdpClient(localIpep);
            //    thrRecv = new Thread(ReceiveMessage);
            //    thrRecv.Start();
            //    IsUdpcRecvStart = true;
            //    Console.WriteLine(txtRecvMssg, "UDP监听器已成功启动");
            //}
            //else // 正在监听的情况，终止监听
            //{
            //    thrRecv.Abort(); // 必须先关闭这个线程，否则会异常
            //    udpcRecv.Close();
            //    IsUdpcRecvStart = false;
            //    ShowMessage(txtRecvMssg, "UDP监听器已成功关闭");
            //}
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="obj"></param>
        private IPEndPoint ReceiveMessage(out byte[] bytRecv)
        {
            IPEndPoint remoteIpep = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    bytRecv = udpcRecv.Receive(ref remoteIpep);
                }
                catch (Exception )
                {
                    bytRecv =new byte[0];
                    break;
                }
            }
            return remoteIpep;
        }

       // 向RichTextBox中添加文本
        //delegate void ShowMessageDelegate(RichTextBox txtbox, string message);


        //private void ShowMessage(RichTextBox txtbox, string message)
        //{
        //    if (txtbox.InvokeRequired)
        //    {
        //        ShowMessageDelegate showMessageDelegate = ShowMessage;
        //        txtbox.Invoke(showMessageDelegate, new object[] { txtbox, message });
        //    }
        //    else
        //    {
        //        txtbox.Text += message + "\r\n";
        //    }
        //}

        //// 清空指定RichTextBox中的文本
        //delegate void ResetTextBoxDelegate(RichTextBox txtbox);
        //private void ResetTextBox(RichTextBox txtbox)
        //{
        //    if (txtbox.InvokeRequired)
        //    {
        //        ResetTextBoxDelegate resetTextBoxDelegate = ResetTextBox;
        //        txtbox.Invoke(resetTextBoxDelegate, new object[] { txtbox });
        //    }
        //    else
        //    {
        //        txtbox.Text = "";
        //    }
        //}

    }
}