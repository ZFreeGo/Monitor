using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.Frame;
using ZFreeGo.TransmissionProtocols.Helper;

namespace ZFreeGo.TransmissionProtocols.FileSever
{

    public class FileWriteServer : ReciveSendServer<ApplicationServiceDataUnit>
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
        private FileASDU writeFileAcitivtyPacket;

        /// <summary>
        /// 写文件激活确认数据包
        /// </summary>
        private ApplicationServiceDataUnit writeFileActivityAckPacket;

        /// <summary>
        /// 写文件激活附加数据包
        /// </summary>
      //private FileWriteActivityPacket callPacket;

        /// <summary>
        /// 写文件激活确认数据包
        /// </summary>
        private FileWriteActivityAckPacket ackPacket;


         /// <summary>
         /// 写文件传输确认包
         /// </summary>
        private ApplicationServiceDataUnit writeFileTransmitAckPacket;

        /// <summary>
        /// 写文件传输包
        /// </summary>
        private FileASDU writeFileTransmitPacket;

        /// <summary>
        /// 写文件传输附加数据包
        /// </summary>
        private FileDataThransmitPacket transmitPacket;

        /// <summary>
        /// 写文件传输确认附加数据包
        /// </summary>
        private FileWriteThransmitAckPacket transmitAckPacket;

        /// <summary>
        /// 包管理器
        /// </summary>
        private FilePacketManager packetManager;

         /// <summary>
         /// 确认数量
         /// </summary>
         private int ackCount;


       

        /// <summary>
        /// 发送报数据数据
        /// </summary>
        private Func<FileASDU, bool> mSendPacket;


        /// <summary>
        /// 写文件激活应答
        /// </summary>
        public event EventHandler<FileServerEventArgs<FileWriteActivityAckPacket>> WriteFileActivityAckEvent;
        /// <summary>
        /// 文件传输应答
        /// </summary>
        public event EventHandler<FileServerEventArgs<FileWriteThransmitAckPacket>> WriteFileTransmitAckEvent;
         /// <summary>
         /// 传输阶段
         /// </summary>
        private TransmitStage stage;

        /// <summary>
        /// 包含服务结果，超时，异常等信息
        /// </summary>
        public event EventHandler<FileEventArgs> ProcessMessageEvent;
        /// <summary>
        /// 校准服务
        /// </summary>
        public FileWriteServer(Func<FileASDU, bool> inSendPacketDelegate)
            : base()
        {
            mOverTime = 5000;
            mRepeatMaxCount = 3;
           mSendPacket = inSendPacketDelegate;           
        }
  
        
        /// <summary>
        /// 启动服务
        /// </summary>      
        /// <param name="packet">包数据</param>
        /// <param name="attribute">文件属性</param>
        /// <param name="fileData">文件数据</param>
        public void StartServer(FileASDU packet, FileAttribute attribute, byte[] fileData)
        {
            try
            {
                if (serverState)
                {
                    throw new Exception("召唤文件目录服务正在运行不能重复启动");
                }
                ackCount = 0;
                InitData();
                packetManager = new FilePacketManager(attribute, fileData);
                writeFileAcitivtyPacket = packet;


                mReadThread = new Thread(ReciveThread);
                mReadThread.Priority = ThreadPriority.Normal;
                mReadThread.Name = "ServerThread线程数据";
                mReadThread.Start();

                mServerThread = new Thread(ServerThread);
                mServerThread.Priority = ThreadPriority.Normal;
                mServerThread.Name = "ServerThread线程";
                mServerThread.Start();
                serverState = true;
            }
            catch (Exception ex)
            {
                MakeProcessMessageEvent("StartServer", FileServerResut.Error, ex);
            }
        }

       

        /// <summary>
        /// 发送数据
        /// </summary>
        public override bool TransmitData()
        {
            switch(stage)
            {
                case TransmitStage.Activity:
                    {
                        return mSendPacket(writeFileAcitivtyPacket);
                       
                    }
                case TransmitStage.Transmission:
                    {
                        if (writeFileTransmitPacket == null)
                        {
                            return true;
                        }
                        return mSendPacket(writeFileTransmitPacket);
                       
                    }
            }
            return false;
        }
        /// <summary>
        /// 检测数据
        /// </summary>
        /// <returns></returns>
        public override bool CheckData()
        {
            try
            {
                var item = mReciveQuene.Dequeue();
                if (item.InformationObject[2] != 2)//是否为文件传输包
                {
                    throw new Exception("不是文件传输包");
                    
                }
               
                switch (stage)
                {
                    case TransmitStage.Activity:
                        {
                            if (item.InformationObject[3] != 8)//是否为读文件激活确认
                            {
                                throw new Exception("不是写文件激活确认");                               
                            }
                            var packet = new FileWriteActivityAckPacket(item.InformationObject, 4, (byte)(item.InformationObject.Length-4));
                            if (packet.Name != packetManager.Attribute.Name)
                            {
                                 throw new Exception("名称不一致");                                
                            }
                            if (packet.FileID != packetManager.Attribute.ID)
                            {
                                throw new Exception("ID不一致");                               
                            }
                            writeFileActivityAckPacket = item;
                            ackPacket = packet;
                           
                            return true;
                        }
                    case TransmitStage.Transmission:
                        {
                            if (item.InformationObject[3] != 10)//是否写文件传输确认
                            {
                                throw new Exception("不是写文件传输确认");                               
                            }
                            var packet = new FileWriteThransmitAckPacket(item.InformationObject, 4);
                            if (packet.Result == FileTransmitDescription.Success)
                            {
                                writeFileTransmitAckPacket = item;
                                transmitAckPacket = packet;
                                return true;
                            }
                            else
                            {
                               
                                if (WriteFileTransmitAckEvent != null)
                                {
                                    var e = new FileServerEventArgs<FileWriteThransmitAckPacket>("从机应答:" + packet.Result.ToString(), OperatSign.WriteFileThransmitAck
                                        , writeFileActivityAckPacket, null);
                                    WriteFileTransmitAckEvent(Thread.CurrentThread, e);
                                }
                                throw new Exception("ID不一致");                               
                            }                        
                        }
                }

                return false;
            }
            catch(Exception ex)
            {
                MakeProcessMessageEvent("CheckData", FileServerResut.Error, ex);
                return false;
            }
        }
        /// <summary>
        /// 时间内完成调用
        /// </summary>
        /// <returns>false--结束本次传输</returns>
        public override bool AckOnTime()
        {
            switch (stage)
            {
                case TransmitStage.Activity:
                    {
                        if (WriteFileActivityAckEvent != null)
                        {
                            var e = new FileServerEventArgs<FileWriteActivityAckPacket>("从机应答", OperatSign.WriteFileActivityAck,
                                writeFileActivityAckPacket, ackPacket);
                            WriteFileActivityAckEvent(Thread.CurrentThread, e);
                        }
                        mRepeatCount = 0;
                        stage = TransmitStage.Transmission;

                        transmitPacket = packetManager.PacketCollect[ackCount];
                        writeFileTransmitPacket = new FileASDU(CauseOfTransmissionList.Activation,
                            0, transmitPacket);

                       

                        return true;                     
                    }
                case TransmitStage.Transmission:
                    {
                        bool state = true;
                        FileTransmitDescription result = FileTransmitDescription.UnknowError;
                        //先检测确认包
                        if ((transmitAckPacket.FragmentNum == packetManager.PacketCollect[ackCount].FragmentNum)
                            && (transmitAckPacket.Result == FileTransmitDescription.Success))
                        {
                            result = FileTransmitDescription.Success;
                            if (++ackCount < packetManager.PacketCollect.Count)
                            {
                                transmitPacket = packetManager.PacketCollect[ackCount];
                                writeFileTransmitPacket = new  FileASDU( CauseOfTransmissionList.Activation,
                                    0, transmitPacket);
                            }
                            else
                            {
                                MakeProcessMessageEvent("传输完成", FileServerResut.Success);
                                state = false;
                            }
                            mRepeatCount = 0;
                        }
                        if (WriteFileTransmitAckEvent != null)
                        {
                            var e = new FileServerEventArgs<FileWriteThransmitAckPacket>("从机应答:" + result.ToString(), OperatSign.WriteFileThransmitAck
                                , writeFileActivityAckPacket, null);
                            WriteFileTransmitAckEvent(Thread.CurrentThread, e);
                        }
                        return state;
                    }
            }
            return false;

        }

        


        /// <summary>
        /// 超时调用
        /// </summary>
        /// <returns>false-- 结束本次传输</returns>
        public override bool AckOverTime()
        {
            string str = "";
            bool state = true;
            if(++mRepeatCount > mRepeatMaxCount)
            {
                str = string.Format("从机应答超时,准备第{0}次重试.", mRepeatCount);
                MakeProcessMessageEvent(str, FileServerResut.OverTime);
                state =  true;
            }
            else
            {
                str = string.Format("从机应答超时,重试{0}次,达到最大重试次数.", mRepeatCount);
                MakeProcessMessageEvent(str, FileServerResut.Fault);
                state =  false;
            }
            
         
            return state;
        }


        /// <summary>
        /// 产生过程事件信息
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="result"></param>
        /// <param name="ex"></param>
        private void MakeProcessMessageEvent(string comment, FileServerResut result, Exception ex)
        {
            ProcessMessageEvent(this, new FileEventArgs(comment, result, ex));
        }
        private void MakeProcessMessageEvent(string comment, FileServerResut result)
        {
            ProcessMessageEvent(this, new FileEventArgs(comment, result, null));
        }
    }
}
