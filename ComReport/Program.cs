using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;


namespace ComReport
{
    static class Program
    {


        //将其更改为全局变量
       static Comm comm;

        
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //初始化COMM管理
            comm= new Comm();
           SerialPort sp= comm.serialPort;
            sp.PortName = "com1";
            //波特率
            sp.BaudRate = 9600;
            //数据位
            sp.DataBits = 8;
            //两个停止位
            sp.StopBits = System.IO.Ports.StopBits.One;
            //无奇偶检验位
            sp.Parity = System.IO.Ports.Parity.None;
            sp.ReadTimeout = 100;
            sp.WriteTimeout = -1;

            comm.Open();
            if (comm.isOpen)
            {
                //进行将委托的事件传递给当前的事务
                //comm.DataReceived += new Comm.EventHandle(comm_DataReceived);
            }



            //默认的窗体
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        //发卡到机口
        private static void SendCardToOut()
        {


        }
        //设置数据 收取指令，并进行解析
        static void comm_DataReceived(byte[] readBudder)
        {
            if (readBudder.Length == 1)
            {
                
            }


        }

    }

}
