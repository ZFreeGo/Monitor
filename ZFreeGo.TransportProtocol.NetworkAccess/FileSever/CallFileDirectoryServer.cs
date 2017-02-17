using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZFreeGo.TransmissionProtocols.FileSever
{
    public class CallFileDirectoryServer : FileTransmissionServer
    {

        /// <summary>
        /// 召唤文件目录数据包
        /// </summary>
        private FileReadPacket readFileDicrectoryPacket;

        /// <summary>
        /// 召唤文件目录确认数据包
        /// </summary>
        private FilePacket readFileDicrectoryAckPacket;

        /// <summary>
        /// 召唤附件数据包
        /// </summary>
       private FileDirectoryCalledPacket callPacket;

        /// <summary>
        /// 应答附加数据包
        /// </summary>
        private FileDirectoryCalledAckPacket ackPacket;

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
        /// 召唤目录服务事件
        /// </summary>
        public event EventHandler<FileServerEventArgs<FileDirectoryCalledAckPacket>> CallFileDirectoryEvent;

        public event EventHandler<CallFileEndEventArgs> CallFileEndEvent;
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
            initData();
        }
        private void initData()
        {
            mRepeatCount = 0;           
            mRequestData = new ManualResetEvent(false);
            mExistData = new ManualResetEvent(false);
            serverState = false;
            fileAttributeList = new List<FileAttribute>();
        }
        
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="inSendDataDelegate">发送数据委托</param>
        /// <param name="packet">包数据</param>
        public void StartServer(Action<FilePacket> inSendDataDelegate,  FileDirectoryCalledPacket packet)
        {
            try
            {
                initData();
                callPacket = packet;
                readFileDicrectoryPacket = new FileReadPacket(0, 0, BasicElement.CauseOfTransmissionList.Activation, 0, packet);

                mReadThread = new Thread(FilePackeReciveThread);
                mReadThread.Priority = ThreadPriority.Normal;
                mReadThread.Name = "ServerThread线程数据";
                mReadThread.Start();

                mServerThread = new Thread(ServeThread);
                mServerThread.Priority = ThreadPriority.Normal;
                mServerThread.Name = "ServerThread线程";
                mServerThread.Start();
                serverState = true;
                SendPacket = inSendDataDelegate;
            }
            catch(Exception ex)
            {
                throw ex;
            }
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
                    Console.WriteLine("CallFileDirectoryServeThread" + ex.Message);
                    Thread.Sleep(100);
                }
                catch (ThreadAbortException ex)
                {
                    Console.WriteLine("CallFileDirectoryServeThread:" + ex.Message);
                    Thread.ResetAbort();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CallFileDirectoryServeThread:" + ex.Message);
            }

        }

        /// <summary>
        /// 发送数据
        /// </summary>
        private void TransmitData()
        {
            if (readFileDicrectoryPacket != null)
            {
                SendPacket(readFileDicrectoryPacket);
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
                 var item =  mReciveQuene.Dequeue();
                 if (item.ASDU.InformationObject[2] != 2)//是否为文件传输包
                 {
                     return false;
                 }
                 if (item.ASDU.InformationObject[3] != 2)//是否为目录召唤确认
                 {
                     return false;
                 }   
                 var packet = new FileDirectoryCalledAckPacket(item.PacketData, 0, (byte)item.PacketData.Length);
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
                throw ex;
            }
        }
        /// <summary>
        /// 时间内完成调用
        /// </summary>
        /// <returns>true--结束本次传输</returns>
        private bool AckOnTime()
        {

            bool state = true;
            if (ackPacket.ResultSign == FileResultSign.Success)
            {
                fileAttributeList.AddRange(ackPacket.FileAttributeList);
                if(ackPacket.Fllow == FllowingFlag.Nothing)
                {
                   if (CallFileEndEvent != null)
                   {
                       CallFileEndEvent(this, new CallFileEndEventArgs("召唤目录传输完成",fileAttributeList));                       
                   }
                   state = false;
                }
                else
                {
                        //readFileDicrectoryPacket = new FileReadPacket(0,0, BasicElement.CauseOfTransmissionList.ActivationACK,)
                    readFileDicrectoryPacket = null;   
                    state = true;
                }
               
            }
            else
            {
                state = true;
            }
                
         
            if (CallFileDirectoryEvent!= null)
            {
                var e = new FileServerEventArgs<FileDirectoryCalledAckPacket>("从机应答:" + ackPacket.ResultSign.ToString(), OperatSign.ReadDirectoryACK,
                    readFileDicrectoryAckPacket, ackPacket);
                CallFileDirectoryEvent(Thread.CurrentThread, e);
            }


            return state;

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
            if (CallFileDirectoryEvent != null)
            {
                var e = new FileServerEventArgs<FileDirectoryCalledAckPacket>(str, OperatSign.ReadDirectoryACK,
                        null, null);
                CallFileDirectoryEvent(Thread.CurrentThread, e);
            }
            return state;
        }
    }
}
