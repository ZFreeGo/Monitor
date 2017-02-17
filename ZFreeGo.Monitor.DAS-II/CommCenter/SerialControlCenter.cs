using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;


namespace ZFreeGo.Monitor.CommCenter
{
    
    public class SerialControlCenter
    {
        private SerialPort serialPort; //串口实例
        public bool portState = false; //串口状态
        private Thread readThread;

       

       // public event EventHandler<RtuFrameArrivedEventArgs> RtuFrameArrived;//事件

        public bool CommState
        {
            get { return portState;}
        }
        public SerialPort SerialPort
        {
            get { return serialPort; }
        }
       
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
                readThread.Join(500);
                readThread.Abort();
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
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

        public bool OpenSerialPort()
        {
            try
            {
                serialPort.Open();

                
                readThread = new Thread(SerialRead);
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
         public bool CloseSerialPort()
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

         public SerialControlCenter()
         {
      
            
             InitSerialPort();

         }
        //接收若有超时溢出，会有异常抛出
        private void SerialRead()
        {
           
            while (portState)
            {
                try
                {
                    try
                    {
                        
                        Thread.Sleep(10);

                    }
                    catch (TimeoutException)
                    {
   
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("串口接收进程::" + ex.Message);
                }
            }
        }
        Queue<byte> queueReciveBuffer = new Queue<byte>();

       


        public void SendMessageToDowncomputer(byte addr, byte funcode)
        {
            if (portState)
            {
                
            }
            else
            {
                throw new Exception("未开启串口");
            }
        }
      

    
    }
}
