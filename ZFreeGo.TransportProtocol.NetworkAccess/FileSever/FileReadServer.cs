using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransportProtocol.NetworkAccess.BasicElement;

namespace ZFreeGo.TransportProtocol.NetworkAccess.FileSever
{

     public class FileReadServer : FileTransmissionServer
    {

         /// <summary>
         /// 传输阶段
         /// </summary>
        private enum TransmitStage
        {
            /// <summary>
            /// 激活阶段
            /// </summary>
            Activity = 1,
            /// <summary>
            /// 传输阶段
            /// </summary>
            Transmission = 2,
        }

        /// <summary>
        /// 读文件激活数据包
        /// </summary>
        private FileReadPacket readFileAcitivtyPacket;

        /// <summary>
        /// 读文件激活确认数据包
        /// </summary>
        private FilePacket readFileActivityAckPacket;

        /// <summary>
        /// 读文件激活附加数据包
        /// </summary>
       // private FileReadActivityPacket callPacket;

        /// <summary>
        /// 读文件激活确认数据包
        /// </summary>
        private FileReadActivityAckPacket ackPacket;


         /// <summary>
         /// 读文件传输确认包
         /// </summary>
        private FilePacket readFileTransmitAckPacket;

        /// <summary>
        /// 读文件传输包
        /// </summary>
        private FilePacket readFileTransmitPacket;

        /// <summary>
        /// 读文件传输数据包
        /// </summary>
        private FileDataThransmitPacket transmitPacket;


        /// <summary>
        /// 包管理器
        /// </summary>
        private FilePacketManager packetManager;

        /// <summary>
        /// 超时时间
        /// </summary>
        private int mOverTime;

        /// <summary>
        /// 重试次数
        /// </summary>
        private int mRepeatCount;

        /// <summary>
        /// 最大重试次数
        /// </summary>
        private readonly int mRepeatMaxCount;

        /// <summary>
        /// 发送报数据数据
        /// </summary>
        public Action<FilePacket> SendPacket;

        /// <summary>
        /// 接收线程
        /// </summary>
        protected Thread mServerThread;

        /// <summary>
        /// 读文件激活应答
        /// </summary>
        public event EventHandler<FileServerEventArgs<FileReadActivityAckPacket>> ReadFileActivityAckEvent;
        /// <summary>
        /// 文件传输应答
        /// </summary>
        public event EventHandler<FileServerEventArgs<FileReadThransmitAckPacket>> ReadFileTransmitAckEvent;
         /// <summary>
         /// 传输阶段
         /// </summary>
        private TransmitStage stage;

        /// <summary>
        /// 校准服务
        /// </summary>
        public FileReadServer()
            : base()
        {
            mOverTime = 5000;
            mRepeatMaxCount = 3;
            initData();
        }
        private void initData()
        {
            mRepeatCount = 0;           
            mRequestData = new ManualResetEvent(false);
            mExistData = new ManualResetEvent(false);
            serverState = false;
            stage = TransmitStage.Activity;
            
        }
        
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="inSendDataDelegate">发送数据委托</param>
        /// <param name="packet">包数据</param>
        /// <param name="attribute">文件属性</param>
        public void StartServer(Action<FilePacket> inSendDataDelegate,FileReadPacket packet,FileAttribute attribute)
        {

            initData();
            packetManager = new FilePacketManager(attribute);
            readFileAcitivtyPacket = packet;


            mReadThread = new Thread(FilePackeReciveThread);
            mReadThread.Priority = ThreadPriority.Normal;
            mReadThread.Name = "FilePackeReciveThread线程数据";
            mReadThread.Start();

            mServerThread = new Thread(ServeThread);
            mServerThread.Priority = ThreadPriority.Normal;
            mServerThread.Name = "FileServerThread线程";
            mServerThread.Start();
            serverState = true;
            SendPacket = inSendDataDelegate;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void StopServer()
        {
            mRequestData.Close();
            mExistData.Close();
            if (mReadThread != null)
            {
                mReadThread.Join(500);
                mReadThread.Abort();

            }
            if (mServerThread != null)
            {
                mServerThread.Join(500);
                mServerThread.Abort();
            }
            serverState = false;
        }

        /// <summary>
        /// 召唤文件目录服务进程
        /// </summary>
        private void ServeThread()
        {
            try
            {
                try
                {
                    TransmitData();              
                    var statTime = DateTime.Now;
                    var responseTime = DateTime.Now;
                    do
                    {
                        mRequestData.Set();//发送请求数据信号

                        if (mExistData.WaitOne(mOverTime))     //等待有数据信号
                        {
                            mRequestData.Reset();//中断获取数据

                            //超时检测--处理有接收数据但不符合要求的情况
                            responseTime = DateTime.Now;
                            var diffTime = responseTime - statTime;
                            if (diffTime.TotalMilliseconds > mOverTime)
                            {
                                if (AckOverTime())
                                {
                                    break;
                                }
                            }
                            //检测数据不符合应答规范则返回
                            if (!CheckData())
                            {
                                continue;
                            }
                            if (AckOnTime())
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (AckOverTime())
                            {
                                break;
                            }
                        }
                        TransmitData();
                        statTime = DateTime.Now; //设置开始时间
                    } while (true);
                }
                catch (ObjectDisposedException ex)
                {
                    serverState = false;
                    Console.WriteLine("FileServerThread:" + ex.Message);
                    Thread.Sleep(100);
                }
                catch (ThreadAbortException ex)
                {
                    serverState = false;
                    Console.WriteLine("FileServerThread:" + ex.Message);
                    Thread.ResetAbort();

                }
            }
            catch (Exception ex)
            {
                serverState = false;
                Console.WriteLine("FileServerThread:" + ex.Message);
            }

        }

        /// <summary>
        /// 发送数据
        /// </summary>
        private void TransmitData()
        {
            switch(stage)
            {
                case TransmitStage.Activity:
                    {
                        SendPacket(readFileAcitivtyPacket);
                        break;
                    }
                case TransmitStage.Transmission:
                    {
                        SendPacket(readFileTransmitAckPacket);
                        break;
                    }
            }
            
        }
        /// <summary>
        /// 检测数据
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            try
            {
                var item = mReciveQuene.Dequeue();
                if (item.ASDU.InformationObject[2] != 2)//是否为文件传输包
                {
                    Console.WriteLine("不是文件传输包");
                    return false;
                }
               
                switch (stage)
                {
                    case TransmitStage.Activity:
                        {
                            if (item.ASDU.InformationObject[3] != 4)//是否为读文件激活确认
                            {
                                Console.WriteLine("不是读文件激活确认");
                                return false;
                            }
                            var packet = new FileReadActivityAckPacket(item.PacketData, 0, (byte)item.PacketData.Length);
                            if (packet.Name == packetManager.Attribute.Name)
                            {
                                readFileActivityAckPacket = item;
                                ackPacket = packet;

                                return true;
                            }
                            else
                            {
                                Console.WriteLine("名称不一致");
                                return false;
                            }                            
                        }
                    case TransmitStage.Transmission:
                        {
                            if (item.ASDU.InformationObject[3] != 5)//是否为读文件传输
                            {
                                Console.WriteLine("不是读文件传输");
                                return false;
                            }
                            var packet = new FileDataThransmitPacket(item.PacketData, 0, (byte)item.PacketData.Length);
                            if (packet.FileID == packetManager.Attribute.ID)
                            {
                                readFileTransmitPacket = item;
                                transmitPacket = packet;
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("ID不一致");
                                return false;
                            }                        
                        }
                }

                return false;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 时间内完成调用
        /// </summary>
        /// <returns>true--结束本次传输</returns>
        private bool AckOnTime()
        {
            switch (stage)
            {
                case TransmitStage.Activity:
                    {
                        if (ReadFileActivityAckEvent != null)
                        {
                            var e = new FileServerEventArgs<FileReadActivityAckPacket>("从机应答", OperatSign.ReadFileActivityACK,
                                readFileActivityAckPacket, ackPacket);
                            ReadFileActivityAckEvent(Thread.CurrentThread, e);
                        }
                        mRepeatCount = 0;
                        stage = TransmitStage.Transmission;
                        return false;                     
                    }
                case TransmitStage.Transmission:
                    {
                        var result = packetManager.AddPacketData(transmitPacket);
                        if (result == FileTransmitDescription.Success)
                        {
                            var addPacket = new FileReadThransmitAckPacket(packetManager.Attribute.ID,
                                packetManager.CurrentPacket.FragmentNum, packetManager.CurrentPacket.Fllow);
                            readFileTransmitAckPacket = new FilePacket(0, 0, CauseOfTransmissionList.ActivationACK,
                                0, addPacket.GetPacketData());
                        }
                        else
                        {
                            if (ReadFileTransmitAckEvent != null)
                            {
                                var e = new FileServerEventArgs<FileReadThransmitAckPacket>("从机应答:" + result.ToString(), OperatSign.ReadFileDataResponseACK,
                                    readFileActivityAckPacket, null);
                                ReadFileTransmitAckEvent(Thread.CurrentThread, e);
                            }
                        }
                        mRepeatCount = 0;
                        return false;
                    }
            }
          
            


            return true;

        }
        /// <summary>
        /// 超时调用
        /// </summary>
        /// <returns>true-- 结束本次传输</returns>
        private bool AckOverTime()
        {
            string str = "";
            bool state = true;
            if(++mRepeatCount > mRepeatMaxCount)
            {
                str = string.Format("从机应答超时,准备第{0}次重试.", mRepeatCount);                
                state =  true;
            }
            else
            {
                str = string.Format("从机应答超时,重试{0}次,达到最大重试次数.", mRepeatCount); 
                state =  false;
            }
            switch(stage)
            {
                case TransmitStage.Activity:
                    {
                        if (ReadFileActivityAckEvent != null)
                        {
                            var e = new FileServerEventArgs<FileReadActivityAckPacket>(str, OperatSign.ReadFileActivityACK,
                                readFileActivityAckPacket, ackPacket);
                            ReadFileActivityAckEvent(Thread.CurrentThread, e);
                        }
                        break;
                    }
                case TransmitStage.Transmission:
                    {
                        if (ReadFileTransmitAckEvent != null)
                        {
                            var e = new FileServerEventArgs<FileReadThransmitAckPacket>(str, OperatSign.ReadFileDataResponseACK,
                                readFileActivityAckPacket, null);
                            ReadFileTransmitAckEvent(Thread.CurrentThread, e);
                        }
                        break;
                    }
            }
         
            return state;
        }
    }
}
