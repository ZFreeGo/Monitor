using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransmissionProtocols.Frame;
using ZFreeGo.TransmissionProtocols.Helper;

namespace ZFreeGo.TransmissionProtocols.FileSever
{
    public class CallFileDirectoryServer : ReciveSendServer<ApplicationServiceDataUnit>
    {

        /// <summary>
        /// 召唤文件目录数据包
        /// </summary>
        private FileASDU readFileDicrectoryPacket;

        /// <summary>
        /// 召唤文件目录确认数据包
        /// </summary>
        private ApplicationServiceDataUnit readFileDicrectoryAckPacket;

        /// <summary>
        /// 召唤附加数据包
        /// </summary>
       private FileDirectoryCalledPacket callPacket;

        /// <summary>
        /// 应答附加数据包
        /// </summary>
        private FileDirectoryCalledAckPacket ackPacket;



        /// <summary>
        /// 发送报数据数据
        /// </summary>
        public Func<FileASDU, bool> SendPacket;

        

        /// <summary>
        /// 召唤目录服务事件
        /// </summary>
        public event EventHandler<FileServerEventArgs<FileDirectoryCalledAckPacket>> CallFileDirectoryEvent;

        /// <summary>
        /// 召唤文件目录结束事件
        /// </summary>
        public event EventHandler<CallFileEndEventArgs> CallFileEndEvent;


        /// <summary>
        /// 包含服务结果，超时，异常等信息
        /// </summary>
        public event EventHandler<FileEventArgs> ProcessMessageEvent;


        /// <summary>
        /// 文件属性列表
        /// </summary>
        private List<FileAttribute> fileAttributeList;

       
        /// <summary>
        /// 召唤文件目录服务
        /// </summary>
        public CallFileDirectoryServer() :base()
        {
            mOverTime = 5000;
            mRepeatMaxCount = 3;
            

            fileAttributeList = new List<FileAttribute>();
        }
      

        /// <summary>
        /// 文件目录召唤服务
        /// </summary>
        /// <param name="inSendDataDelegate">发送数据委托</param>
        public CallFileDirectoryServer(Func<FileASDU, bool> inSendDataDelegate)
        {
            SendPacket = inSendDataDelegate;
        }
        
        /// <summary>
        /// 启动服务
        /// </summary>
        
        /// <param name="packet">包数据</param>
        public void StartServer(  FileDirectoryCalledPacket packet)
        {
            try
            {
                if (serverState)
                {
                    throw new Exception("召唤文件目录服务正在运行不能重复启动");
                }

                InitData();
                callPacket = packet;
                readFileDicrectoryPacket = new FileASDU(BasicElement.CauseOfTransmissionList.Activation, 0, packet);

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
            if (readFileDicrectoryPacket != null)
            {
                if(SendPacket(readFileDicrectoryPacket))
                {
                    return true; 
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 检测数据
        /// </summary>
        /// <returns>true--检测通过，false--检测失败</returns>
        public override bool CheckData()
        {
            try
            {
                 var item =  mReciveQuene.Dequeue();
                 if (item.InformationObject[2] != 2)//是否为文件传输包
                 {
                     return false;
                 }
                 if (item.InformationObject[3] != 2)//是否为目录召唤确认
                 {
                     return false;
                 }
                 var packet = new FileDirectoryCalledAckPacket(item.InformationObject, 4, (byte)(item.InformationObject.Length - 4));
                 if( packet.DirectoryID == callPacket.DirectoryID)
                 {
                     readFileDicrectoryAckPacket = item;
                     ackPacket = packet;
                     return true;
                 }
                 return false;

            }
            catch(Exception ex)
            {
                MakeProcessMessageEvent("AckOnTime()", FileServerResut.Error, ex);
                return false;
            }
        }
        /// <summary>
        /// 时间内完成调用
        /// </summary>
        /// <returns>true-结束本次传输, false--结束本次传输</returns>
        public override bool AckOnTime()
        {
            try
            {
                bool state = true;
                if (ackPacket.ResultSign == FileResultSign.Success)
                {
                    fileAttributeList.AddRange(ackPacket.FileAttributeList);
                    if (ackPacket.Fllow == FllowingFlag.Nothing)
                    {
                        if (CallFileEndEvent != null)
                        {
                            CallFileEndEvent(this, new CallFileEndEventArgs("召唤目录传输完成", fileAttributeList));
                        }

                        state = false;
                    }
                    else
                    {
                        
                        readFileDicrectoryPacket = null;
                        state = true;
                    }
                }
                else
                {
                    state = true;
                }
                MakeCallFileDirectoryEvent("从机应答:" + ackPacket.ResultSign.ToString(), OperatSign.ReadDirectoryACK,
                        readFileDicrectoryAckPacket, ackPacket);

                return state;
            }
            catch(Exception ex)
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
        /// 召唤文件目录确认包
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="sign"></param>
        /// <param name="packet"></param>
        /// <param name="additionalPacket"></param>
        private void MakeCallFileDirectoryEvent(string comment, OperatSign sign, ApplicationServiceDataUnit packet, FileDirectoryCalledAckPacket additionalPacket)
        {
            if (CallFileDirectoryEvent != null)
            {
                var e = new FileServerEventArgs<FileDirectoryCalledAckPacket>(comment, sign, packet, additionalPacket);
                CallFileDirectoryEvent(Thread.CurrentThread, e);
            }
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
