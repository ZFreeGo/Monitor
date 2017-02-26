using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransmissionProtocols.Helper;



namespace ZFreeGo.Monitor.DASModel.Helper
{
    public class OldCalibration
    {
        /// 接收队缓冲 一级缓冲
        /// </summary>
        public Queue<byte> ReciveQuene;
        /// <summary>
        /// 缓冲队列 二级缓冲
        /// </summary>
        public Queue<byte> ReciveQueneBuffer;

        /// <summary>
        /// 暂存一帧
        /// </summary>
        private Queue<byte> FrameQueneBuffer;
        /// <summary>
        /// 接收线程
        /// </summary>
        private Thread readThread;

        /// <summary>
        /// 校准信息功能
        /// </summary>
        public event EventHandler<OldCalibrationEventArgs> CalibrationMessageArrived;



        public bool IsRealUpdate = false;
        public int  UpdateIndex = 0;

     
        /// <summary>
        /// 处理步骤
        /// </summary>
        private int FlowStep = 1;
        /// <summary>
        /// 最新长度
        /// </summary>
        private int dataLen = 0;


        /// <summary>
        /// 帧数据
        /// </summary>
        private byte[] dataArray;



        /// <summary>
        /// 检测提取帧
        /// </summary>
        /// <param name="quene">数组</param>
        private bool GetAFrame(Queue<byte>  quene, ref byte[] dataArray)
        {
           
            switch (FlowStep)
            {
                case 1:
                    {
                        if (quene.Count < 9)
                        {
                            return false;
                        }
                        FlowStep = 2;

                        break;
                    }
                case 2:
                    {

                        if (quene.Dequeue() != 0x68) //首字符
                        {
                             FlowStep = 1;
                            return false;
                        }
                        byte len = quene.Dequeue();
                        if (len != quene.Dequeue()) //二者长度是否唯一
                        {
                            FlowStep = 1;
                            return false;
                        }
                        if (quene.Dequeue() != 0x68) //0x68
                        {
                             FlowStep = 1;
                            return false;
                        }
                        dataLen = len;
                        FlowStep = 3;
                       


                        break;
                    }
                case 3:
                    {
                        if (quene.Count  >=  dataLen + 2) //判断是否满足长度要求 :4 + 数据长度() + 校验+ 技结束字符（0x16）
                        {
                            dataArray = new byte[4 + dataLen + 2];
                            dataArray[0] = 0x68;
                            dataArray[1] = (byte)dataLen;
                            dataArray[2] = (byte)dataLen;
                            dataArray[3] = 0x68;


                            UInt16 checkSum = 0;
         

                            for(int i = 0; i< dataLen; i++)
                            {
                                dataArray[4 + i] = quene.Dequeue();
                                checkSum += dataArray[4 + i];
                            }
                            byte check = quene.Dequeue();                            
                           
                            if (check != (byte)checkSum)
                            {
                                return false;
                            }
                            //结束字符判断
                            byte end = quene.Dequeue();
                            if (end == 0x16)
                            {
                                dataArray[4 + dataLen] = check;
                                dataArray[5 + dataLen] = end;

                                FlowStep = 1;
                                return true;
                            }
                            FlowStep = 1;
                        }
                        break;
                    }
                default:
                        {
                            return false;
                        }
            }
            return false;
            

            
        }
        /// <summary>
        /// 获取校准信息
        /// </summary>
        /// <param name="dataArray">数据数组</param>
        void GetCalbrationMessage(byte[] dataArray)
        {
            int len = dataArray[2];
            int count = len / 4;
            var array = new UInt32[count ];
            for (int i = 0; i < count; i++) //大端模式
            {
                array[i] = ((UInt32)dataArray[4*i + 7]<<24) + ((UInt32)dataArray[4*i + 8]<<16) + 
                    ((UInt32)dataArray[4*i + 9]<<8) + (UInt32)dataArray[4*i + 10];
            }
            CalibrationAction property;
            switch (dataArray[5])
            {
                case 0x40:
                    {
                        property = CalibrationAction.FactorCall;
                        CalibrationMessageArrived(this, new OldCalibrationEventArgs(dataArray, array, property));
                        break;
                    }
                case 0x41:
                    {
                        property = CalibrationAction.FactorDownload;
                        CalibrationMessageArrived(this, new OldCalibrationEventArgs(dataArray, array, property));
                        break;
                    }
                case 0x42:
                    {
                        property = CalibrationAction.FactorFix;
                        CalibrationMessageArrived(this, new OldCalibrationEventArgs(dataArray, array, property));
                        break;
                    }
               
                default:
                        {
                            property = CalibrationAction.Null;
                            break;
                        }
            }




           
        }
        /// <summary>
        /// 主要处理步骤
        /// </summary>
        void MainCheckStep()
        {
 
            bool result = GetAFrame(ReciveQueneBuffer,ref dataArray);
            if (result)
            {
                GetCalbrationMessage(dataArray);
            }

        }
        /// <summary>
        /// 校准帧事件 
        /// </summary>
        public OldCalibration()
        {
            
             ReciveQuene = new Queue<byte>();
             ReciveQueneBuffer = new Queue<byte>();

             FrameQueneBuffer = new Queue<byte>();

            
             

             readThread = new Thread(TcpReadThread);
             readThread.Priority = ThreadPriority.AboveNormal;
             readThread.Name = "数据解码";
             readThread.Start();
        }
        /// <summary>
        /// 结束接收线程，用于最后注销时
        /// </summary>
        public void Close()
        {
            if (readThread != null)
            {
                readThread.Join(500);
                readThread.Abort();
                readThread = null;
            }
        }

        /// <summary>
        /// TcpRead进程,从Tcp连接中读取顺序
        /// </summary>
        private void TcpReadThread()
        {
            try
            {
                try
                {
                    while (true)
                    {
                        //二级缓存数据为空，再从一级缓存转存数据

                        lock (ReciveQuene)
                        {
                            while (ReciveQuene.Count > 0)  //转存到二级缓冲
                            {
                                ReciveQueneBuffer.Enqueue(ReciveQuene.Dequeue());
                            }

                        }
                                                
                        if (ReciveQueneBuffer.Count > 0)
                        {
                            MainCheckStep();
                        }

                        Thread.Sleep(10);
                    }
           
                }
                catch (ThreadAbortException )
                {
                    Thread.ResetAbort();
                }

            }
            catch (Exception ex)
            {
                while (true)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(100);
                }
            }
        }

        

        /// <summary>
        /// 生成系数召唤数据
        /// </summary>
        /// <returns>字节数组</returns>
        public byte[] MakeFactorCallData()
        {
            return new byte[8] {0x68, 0x02, 0x02, 0x68, 0x01, 0x40, 0x41, 0x16 };
        }

        /// <summary>
        /// 生成系数下载数据
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="len">数据长度</param>
        /// <returns>字节数组</returns>
        public byte[] MakeFactorDownloadData(UInt32[] array, int len)
        {
            int alllen = 7 + len * 4 + 2;
            var data = new byte[alllen];
            int  i = 7;
            byte a = 0, b = 0, c = 0, d = 0;
            UInt16 checkSum = 0;
            var datalen = len * 4 + 3;
            data[0] = 0x68;
            data[1] = (byte)datalen;
            data[2] = (byte)datalen;
            data[3] = 0x68;
            data[4] = 0x01;
            data[5] = 0x41;
            data[6] = 0x90;
            checkSum += data[4];
            checkSum += data[5];
            checkSum += data[6];
            //大端模式
            foreach (var m in array)
            {

                a = data[i++] = ElementTool.GetBit31_24(m);
                b = data[i++] = ElementTool.GetBit23_16(m);
                c = data[i++] = ElementTool.GetBit15_8(m);
                d = data[i++] = ElementTool.GetBit7_0(m);
                checkSum += (UInt16)(a + b + c + d);

            }
            data[alllen - 2] = ElementTool.GetBit7_0(checkSum);
            data[alllen - 1] = 0x16;
            return data;
        }
        /// <summary>
        /// 轮训录波数据
        /// </summary>
        /// <returns></returns>
        public byte[]  MakeRecordCallData()
        {
            return new byte[8] { 0x68, 0x02, 0x02, 0x68, 0x01, 0x5F, 0x60, 0x16 };
        }
        /// <summary>
        /// 生成系数固化指令 
        /// </summary>
        /// <returns>字节数组</returns>
        public byte[] MakeFactorFix()
        {
            return new byte[8] { 0x68, 0x02, 0x02, 0x68, 0x01, 0x42, 0x43, 0x16 };
        }
        
    }
}
