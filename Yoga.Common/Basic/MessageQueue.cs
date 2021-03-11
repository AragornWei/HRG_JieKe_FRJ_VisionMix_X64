using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yoga.Common.Basic
{
    public class MessageQueue
    {
        private ConcurrentQueue<string> ListQueue = new ConcurrentQueue<string>();

        TextBox txtShow;
        object objLock = new object();

        int messageCount = 0;
        private int InShowMessage = 0;

        public MessageQueue(TextBox txtShow)
        {
            this.txtShow = txtShow;
        }
        //int x;
        async void UIDoWork()
        {
            ShowMessage();
            await Task.Delay(10);

            Interlocked.Exchange(ref InShowMessage, 0);
            if ((ListQueue.IsEmpty == false))
            {
                TriggerShow();
            }
        }

        private void ShowMessage()
        {
            StringBuilder strb = new StringBuilder(); //这样字符串赋新值时，就不用重新申请新字符串变量内存。
            while (ListQueue.IsEmpty == false)
            {
                try
                {
                    string message = string.Empty;

                    //从队列中取出
                    if (ListQueue.TryDequeue(out message) == false)
                    {
                        continue;
                    }

                    if (message == string.Empty)
                    {
                        continue;
                    }
                    messageCount++;
                    strb.Append(message);
                    strb.Append(Environment.NewLine);
                }
                catch (Exception /*ex*/)
                {
                }
            }
            txtShow.AppendText(strb.ToString());
            //超过1000行就删除500行
            if (messageCount > 1000)
            {
                string[] lines = txtShow.Lines;
                List<string> a = lines.ToList();
                a.RemoveRange(0, 500);           //删除之前必须转换成链表。
                txtShow.Lines = a.ToArray();
                messageCount = 500;
            }
            txtShow.ScrollToCaret();
        }

        private void TriggerShow()
        {
            if (Interlocked.CompareExchange(ref InShowMessage, 1, 0) == 0)    //如果InShowMessage!=0,就等下次触发，反正已经存入ListQueue中。
            {
                //启动显示
                if (txtShow.InvokeRequired)
                {
                    txtShow.BeginInvoke(new Action(UIDoWork));
                }
                else
                {
                    UIDoWork();
                }
            }
        }
        public void ShowMessage(string message)  //这个是起始，最初开始调用这个函数。
        {
            //AddCount();
            ListQueue.Enqueue($"{DateTime.Now.ToString("HH:mm:ss")} {message}");  //字符串里面用变量时，必须用{}括起来。
            TriggerShow();
        }
    }
}
