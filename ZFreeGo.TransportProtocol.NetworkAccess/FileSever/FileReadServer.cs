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

     public class FileReadServer :  ReciveSendServer<ApplicationServiceDataUnit>
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
        private FileASDU readFileAcitivtyPacket;

        /// <summary>
        /// 读文件激活确认数据包
        /// </summary>
        private ApplicationServiceDataUnit readFileActivityAckPacket;

        /// <summary>
        /// 读文件激活附加数据包
        /// </summary>
       // private FileReadActivityPacket callPacket;

        /// <summary>
        /// 读文件激活确认附加数据包
        /// </summary>
        private FileReadActivityAckPacket ackPacket;


         /// <summary>
         /// 读文件传输确认包
         /// </summary>
        private FileASDU readFileTransmitAckPacket;

        /// <summary>
        /// 读文件传输包
        /// </summary>
        private ApplicationServiceDataUnit readFileTransmitPacket;

        /// <summary>
        /// 读文件传输附加数据包
        /// </summary>
        private FileDataThransmitPacket transmitPacket;


        /// <summary>
        /// 包管理器
        /// </summary>
        private FilePacketManager packetManager;

       
        /// <summary>
        /// 发送报数据数据
        /// </summary>
        private Func<FileASDU, bool> mSendDataDelegate;

        /// <summary>
        /// 读文件激活应答
        /// </summary>
        public event EventHandler<FileServerEventArgs<FileReadActivityAckPacket>> ReadFileActivityAckEvent;
        /// <summary>
        /// 文件传输应答
        /// </summary>
       // public event EventHandler<FileServerEventArgs<FileReadThransmitAckPacket>> ReadFileTransmitAckEvent;
       // public event EventHandler<FileServerEventArgs<FileReadThransmitAckPacket>> ReadFileTransmitAckEvent;


         /// <summary>
         /// 读文件结果事件
         /// </summary>
        public event EventHandler<FileReadEndEventArgs> ReadFileEndEvent;

        /// <summary>
        /// 包含服务结果，超时，异常等信息
        /// </summary>
        public event EventHandler<FileEventArgs> ProcessMessageEvent;
         /// <summary>
         /// 传输阶段
         /// </summary>
        private TransmitStage stage;

        /// <summary>
        /// 校准服务
        /// </summary>
        public FileReadServer(Func<FileASDU, bool> inSendDataDelegate)
            : base()
        {
            mOverTime = 5000;
            mRepeatMaxCount = 3;
            mSendDataDelegate = inSendDataDelegate;
        }


        private FileAttribute readFileAttribute;   
        
        
        /// <summary>
        /// 启动服务
        /// </summary>        
        /// <param name="packet">包数据</param>        
        public void StartServer(FileReadActivityPacket  packet)
        {
            try
            {
                if (serverState)
                {
                    throw new Exception("召唤文件目录服务正在运行不能重复启动");
                }

                InitData();
                stage = TransmitStage.Activity;
                //packetManager = new FilePacketManager(attribute);
                readFileAttribute = new FileAttribute(packet.Name, 0, null);
                readFileAcitivtyPacket = new FileASDU(CauseOfTransmissionList.Activation, 0, packet);


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
            catch(Exception ex)
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
                        return mSendDataDelegate(readFileAcitivtyPacket);                        
                    }
                case TransmitStage.Transmission:
                    {
                        if (readFileTransmitAckPacket == null)
                        {
                            return true;
                        }
                         return mSendDataDelegate(readFileTransmitAckPacket);                       
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
                            if (item.InformationObject[3] != 4)//是否为读文件激活确认
                            {
                                throw new Exception("不是读文件激活确认");
                               
                            }
                            var packet = new FileReadActivityAckPacket(item.InformationObject, 4, (byte)(item.InformationObject.Length-4));
                            if (packet.Name == readFileAttribute.Name)
                            {
                                readFileActivityAckPacket = item;
                                ackPacket = packet;

                                return true;
                            }
                            else
                            {
                                throw new Exception("名称不一致");                                
                            }                            
                        }
                    case TransmitStage.Transmission:
                        {
                            if (item.InformationObject[3] != 5)//是否为读文件传输
                            {
                                throw new Exception("不是读文件传输");
                               
                            }
                            var packet = new FileDataThransmitPacket(item.InformationObject, 4, (byte)(item.InformationObject.Length-4));
                            if (packet.FileID == packetManager.Attribute.ID)
                            {
                                readFileTransmitPacket = item;
                                transmitPacket = packet;
                                return true;
                            }
                            else
                            {
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
            try
            {
                switch (stage)
                {
                    case TransmitStage.Activity:
                        {

                            readFileAttribute.ID = ackPacket.FileID;
                            readFileAttribute.Size = ackPacket.Size;
                            packetManager = new FilePacketManager(readFileAttribute);

                            if (ReadFileActivityAckEvent != null)
                            {
                                var e = new FileServerEventArgs<FileReadActivityAckPacket>("从机应答", OperatSign.ReadFileActivityACK,
                                    readFileActivityAckPacket, ackPacket);
                                ReadFileActivityAckEvent(Thread.CurrentThread, e);
                            }
                            mRepeatCount = 0;
                            stage = TransmitStage.Transmission;
                            readFileTransmitAckPacket = null;
                            return true;
                        }
                    case TransmitStage.Transmission:
                        {
                            var result = packetManager.AddPacketData(transmitPacket);
                            if (result == FileTransmitDescription.Success)
                            {
                                var addPacket = new FileReadThransmitAckPacket(packetManager.Attribute.ID,
                                    packetManager.CurrentPacket.FragmentNum, packetManager.CurrentPacket.Fllow);
                                readFileTransmitAckPacket = new FileASDU( CauseOfTransmissionList.ActivationACK,
                                    0, addPacket.GetPacketData()); 
                                if (packetManager.FullFlag)
                                {
                                    if (ReadFileEndEvent == null)
                                    {
                                        ReadFileEndEvent(this, new FileReadEndEventArgs("文件读取完毕", packetManager));
                                    }
                                    MakeProcessMessageEvent("文件读取完毕", FileServerResut.Success);

                                    return false;
                                }
                                
                            }
                            else
                            {
                                MakeProcessMessageEvent(result.ToString(), FileServerResut.Error);
                            }

                            //if (ReadFileTransmitAckEvent != null)
                            //{
                            //    var e = new FileServerEventArgs<FileReadThransmitAckPacket>("从机应答:" + result.ToString(), OperatSign.ReadFileDataResponseACK,
                            //        readFileActivityAckPacket, null);
                            //    ReadFileTransmitAckEvent(Thread.CurrentThread, e);
                            //}
                            mRepeatCount = 0;
                            return true;
                        }
                }




                return true;
            }
            catch (Exception ex)
            {
                MakeProcessMessageEvent("AckOnTime()", FileServerResut.Error, ex);
                return false;
            }

        }
        /// <summary>
        /// 超时调用
        /// </summary>
        /// <returns>true-- 结束本次传输</returns>
        public override bool AckOverTime()
        {
            string str = "";
            bool state = true;
            if(++mRepeatCount < mRepeatMaxCount)
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
