using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//定义所需要的线程
using System.Threading;
//生命Comm口所需要的生命变量的位置
using System.IO.Ports;
//windows窗体事件
using System.Windows.Forms;

namespace ComReport
{
    class Comm
    {

        //定义一个委托方法
        public delegate void EventHandle(byte[] readBuffer);
        public event EventHandler DataReceived;
        //定义com口的session
        public SerialPort serialPort;
        Thread thread;
        volatile bool _keepReading;

        //初始化
        public Comm()
        {
            serialPort = new SerialPort();
            thread = null;
            _keepReading = false;
        }

        public bool isOpen
        {
            get
            {
                //判断当前的端口是否打开
                return serialPort.IsOpen;
            }
        }

        private void StartReading()
        {
            if (!_keepReading)
            {
                _keepReading = true;
                //开启线程进行轮训播放
                thread = new Thread(new ThreadStart(ReadPort));
                thread.Start();
            }
        }


        private void StopReading()
        {
            if (_keepReading)
            {
                _keepReading = false;
                //阻塞当前的线程，并进行销毁
                thread.Join();
                thread = null;
            }
        }

        private void ReadPort()
        {
            while (_keepReading)
            {
                //设定当前端口读取的字节数
                int count = serialPort.BytesToRead;
                if (count > 0)
                {
                    byte[] readBuffer = new byte[count];
                    try
                    {
                        Application.DoEvents();

                        //从Com口中读取数据
                        serialPort.Read(readBuffer, 0, count);
                        //设置委托代理  如果不为空，则将数据传递给代理对象
                        DataReceived?.DynamicInvoke(readBuffer);
                        Thread.Sleep(100);
                    }
                    catch(TimeoutException)
                    {

                    }
                   
                }

            }
        }
        public void Open()
        {
            //首先进行检测是否开启，如果开启就先进性关闭操作
            Close();
            //开启COM口
            serialPort.Open();
            if (serialPort.IsOpen)
            {
                //如果COM口开启则进行录入
                StartReading();
            }
            else
            {
                //如果没有开启，则进行弹出串口打开失败
                MessageBox.Show("串口打开失败！");
            }


        }
        public void Close()
        {   //停止当前从COM口进行的读取
            StopReading();
            //关闭当前的COM口
            serialPort.Close();
        }
        //如果com当前是打开的，则可以向com口进行写入数据
        public void WritePort(byte[] send,int offSet,int count)
        {
            if (isOpen)
            {
                serialPort.Write(send, offSet, count);
            }
        }







    }
}
