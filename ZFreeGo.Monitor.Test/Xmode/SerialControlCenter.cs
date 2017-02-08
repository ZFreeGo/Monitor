using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;


namespace ZFreeGo.Monitor.Test.Xmode
{

    public class SerialControlCenter
    {
        private SerialPort serialPort; //串口实例
        private bool portState = false; //串口状态
        private Thread readThread;


        /// <summary>
        /// 串口数据到达
        /// </summary>
        public event EventHandler<SerialDataEvent> SerialDataArrived;//事件

        /// <summary>
        /// 获取串口状态
        /// </summary>
        public bool CommState
        {
            get { return portState; }
        }
        /// <summary>
        /// 获取串口
        /// </summary>
        public SerialPort SerialPort
        {
            get { return serialPort; }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void CloseCenter()
        {
            if (serialPort != null)
            {

                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }

            }
            if (readThread != null)
            {
                CloseSerialPort();
            }
        }
        void InitSerialPort()
        {
            serialPort = new SerialPort();

            serialPort.ReadTimeout = 500;
            serialPort.WriteTimeout = 500;

            serialPort.DtrEnable = true;
            serialPort.ReadBufferSize = 2 * 20 * 1024;//默认值4096
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns>true-打开成功，false-失败</returns>
        public bool OpenSerialPort()
        {
            try
            {
                serialPort.Open();


                readThread = new Thread(ThreadSerialRead);
                readThread.Priority = ThreadPriority.AboveNormal;
                readThread.Start();
                portState = true;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        private bool CloseSerialPort()
        {
            portState = false;
            readThread.Join(500);
            readThread.Abort();
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            return true;
        }
        /// <summary>
        /// 串口中心初始化
        /// </summary>
        public SerialControlCenter()
        {


            InitSerialPort();

        }

        /// <summary>
        /// 串口读取线程
        /// </summary>
        private void ThreadSerialRead()
        {
            try
            {

                while (portState)
                {
                    try
                    {
                        try
                        {
                            if (serialPort.BytesToRead != 0)
                            {
                                int len = serialPort.BytesToRead;
                                var data = new byte[len];
                                serialPort.Read(data, 0, len);
                                this.SerialDataArrived(serialPort, new SerialDataEvent(data));
                            }

                            Thread.Sleep(20);

                        }
                        catch (TimeoutException)
                        {

                        }
                    }
                    catch (ThreadAbortException ex)
                    {
                        Thread.ResetAbort();
                        Trace.WriteLine("串口接收进程::" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("串口接收进程::" + ex.Message);


            }
        }

        /// <summary>
        /// 发送带有长度标识的字节数组
        /// </summary>
        /// <param name="sendData">字节数组</param>
        /// <param name="len">发送的数组长度</param>
        public void SendMessage(byte[] sendData, int len)
        {

            serialPort.Write(sendData, 0, len);

        }
        /// <summary>
        /// 发送数据--字节数组
        /// </summary>
        /// <param name="sendData">字节数组</param>
        public void SendMessage(byte[] sendData)
        {
            serialPort.Write(sendData, 0, sendData.Length);
        }
        /// <summary>
        /// 发送数据-自付出
        /// </summary>
        /// <param name="msg">字符串</param>
        public void SendMessage(string msg)
        {
            byte[] temp = Encoding.Unicode.GetBytes(msg); // 获得缓存        
            serialPort.Write(temp, 0, temp.Length);
        }
       
  


    

    }
}
