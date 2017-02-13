using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileSever
{

     public class FileWriteServer : FileTransmissionServer
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
        /// 写文件激活数据包
        /// </summary>
        private FileReadPacket writeFileAcitivtyPacket;

        /// <summary>
        /// 写文件激活确认数据包
        /// </summary>
        private FilePacket writeFileActivityAckPacket;

        /// <summary>
        /// 写文件激活附加数据包
        /// </summary>
        private FileNr.FileWriteActivityPacket callPacket;

        /// <summary>
        /// 写文件激活确认数据包
        /// </summary>
        private FileNr.FileWriteActivityAckPacket ackPacket;


         /// <summary>
         /// 写文件传输确认包
         /// </summary>
        private FilePacket writeFileTransmitAckPacket;

        /// <summary>
        /// 写文件传输包
        /// </summary>
        private FilePacket writeFileTransmitPacket;

        /// <summary>
        /// 写文件传输数据包
        /// </summary>
        private FileNr.FileDataThransmitPacket transmitPacket;

        /// <summary>
        /// 写文件传输数据确认包
        /// </summary>
        private FileNr.FileWriteThransmitAckPacket transmitAckPacket;

        /// <summary>
        /// 包管理器
        /// </summary>
        private FilePacketManager packetManager;

         /// <summary>
         /// 确认数量
         /// </summary>
         private int ackCount;


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
        /// 写文件激活应答
        /// </summary>
        public event EventHandler<FileServerEventArgs<FileNr.FileWriteActivityAckPacket>> WriteFileActivityAckEvent;
        /// <summary>
        /// 文件传输应答
        /// </summary>
        public event EventHandler<FileServerEventArgs<FileNr.FileWriteThransmitAckPacket>> WriteFileTransmitAckEvent;
         /// <summary>
         /// 传输阶段
         /// </summary>
        private TransmitStage stage;

        /// <summary>
        /// 校准服务
        /// </summary>
        public FileWriteServer()
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
            ackCount = 0;
        }
        
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="inSendDataDelegate">发送数据委托</param>
        /// <param name="packet">包数据</param>
        /// <param name="attribute">文件属性</param>
        /// <param name="fileData">文件数据</param>
        public void StartServer(Action<FilePacket> inSendDataDelegate,FileReadPacket packet,FileNr.FileAttribute attribute, byte[] fileData)
        {

            initData();
            packetManager = new FilePacketManager(attribute, fileData);
            writeFileAcitivtyPacket = packet;


            mReadThread = new Thread(FilePackeReciveThread);
            mReadThread.Priority = ThreadPriority.Normal;
            mReadThread.Name = "FilePackeSendThread线程数据";
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
                        SendPacket(writeFileAcitivtyPacket);
                        break;
                    }
                case TransmitStage.Transmission:
                    {
                        SendPacket(writeFileTransmitPacket);
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
                            if (item.ASDU.InformationObject[3] != 8)//是否为读文件激活确认
                            {
                                Console.WriteLine("不是写文件激活确认");
                                return false;
                            }
                            var packet = new FileNr.FileWriteActivityAckPacket(item.PacketData, 0, (byte)item.PacketData.Length);
                            if (packet.Name != packetManager.Attribute.Name)
                            {
                                Console.WriteLine("名称不一致");
                                return false;
                            }
                            if (packet.FileID != packetManager.Attribute.ID)
                            {
                                Console.WriteLine("ID不一致");
                                return false;
                            }
                            writeFileActivityAckPacket = item;
                            ackPacket = packet;
                            return true;

                        }
                    case TransmitStage.Transmission:
                        {
                            if (item.ASDU.InformationObject[3] != 10)//是否写文件传输确认
                            {
                                Console.WriteLine("不是写文件传输确认");
                                return false;
                            }
                            var packet = new FileNr.FileWriteThransmitAckPacket(item.PacketData, 0);
                            if (packet.Result == FileNr.FileTransmitDescription.Success)
                            {
                                writeFileTransmitAckPacket = item;
                                transmitAckPacket = packet;
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("ID不一致");
                                if (WriteFileTransmitAckEvent != null)
                                {
                                    var e = new FileServerEventArgs<FileNr.FileWriteThransmitAckPacket>("从机应答:" + packet.Result.ToString(), FileNr.OperatSign.WriteFileThransmitAck
                                        , writeFileActivityAckPacket, null);
                                    WriteFileTransmitAckEvent(Thread.CurrentThread, e);
                                } 
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
                        if (WriteFileActivityAckEvent != null)
                        {
                            var e = new FileServerEventArgs<FileNr.FileWriteActivityAckPacket>("从机应答", FileNr.OperatSign.WriteFileActivityAck,
                                writeFileActivityAckPacket, ackPacket);
                            WriteFileActivityAckEvent(Thread.CurrentThread, e);
                        }
                        mRepeatCount = 0;
                        stage = TransmitStage.Transmission;

                        transmitPacket = packetManager.PacketCollect[ackCount];
                        writeFileTransmitPacket = new FileWritePacket(0, 0, CauseOfTransmissionList.ActivationACK,
                            0, transmitPacket);



                        return false;                     
                    }
                case TransmitStage.Transmission:
                    {
                        bool state = false;
                        FileNr.FileTransmitDescription result = FileNr.FileTransmitDescription.UnknowError;
                        //先检测确认包
                        if ((transmitAckPacket.FragmentNum == packetManager.PacketCollect[ackCount].FragmentNum)
                            && (transmitAckPacket.Result == FileNr.FileTransmitDescription.Success))
                        {
                            result = FileNr.FileTransmitDescription.Success;
                            if (++ackCount < packetManager.PacketCollect.Count)
                            {
                                transmitPacket = packetManager.PacketCollect[ackCount];
                                writeFileTransmitPacket = new FileWritePacket(0, 0, CauseOfTransmissionList.ActivationACK,
                                    0, transmitPacket);
                            }
                            else
                            {
                                state = true;
                            }
                            mRepeatCount = 0;
                        }
                        if (WriteFileTransmitAckEvent != null)
                        {
                            var e = new FileServerEventArgs<FileNr.FileWriteThransmitAckPacket>("从机应答:" + result.ToString(), FileNr.OperatSign.WriteFileThransmitAck
                                , writeFileActivityAckPacket, null);
                            WriteFileTransmitAckEvent(Thread.CurrentThread, e);
                        }                        
                        return state;
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
                        if (WriteFileActivityAckEvent != null)
                        {
                            var e = new FileServerEventArgs<FileNr.FileWriteActivityAckPacket>(str, FileNr.OperatSign.WriteFileActivityAck,
                                writeFileActivityAckPacket, ackPacket);
                            WriteFileActivityAckEvent(Thread.CurrentThread, e);
                        }
                        break;
                    }
                case TransmitStage.Transmission:
                    {
                        if (WriteFileTransmitAckEvent != null)
                        {
                            var e = new FileServerEventArgs<FileNr.FileWriteThransmitAckPacket>(str, FileNr.OperatSign.ReadFileDataResponseACK,
                                writeFileActivityAckPacket, null);
                            WriteFileTransmitAckEvent(Thread.CurrentThread, e);
                        }
                        break;
                    }
            }
         
            return state;
        }
    }
}
