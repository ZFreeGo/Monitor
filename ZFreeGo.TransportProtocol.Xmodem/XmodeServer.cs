using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZFreeGo.TransportProtocol.Xmodem
{
    public class XmodeServer
    {
  

        /// <summary>
        /// 接收队缓冲 一级缓冲
        /// </summary>
        public Queue<byte> ReciveQuene;

        /// <summary>
        /// 服务事件
        /// </summary>
        public event EventHandler<XmodeServerEventArgs> ServerEvent;


        /// <summary>
        /// 缓冲队列 二级缓冲
        /// </summary>
        private Queue<byte> mReciveQueneBuffer;

        /// <summary>
        /// 当前检测步骤
        /// </summary>
        private ServerStep mCurrentStep;

        /// <summary>
        /// 校验模式
        /// </summary>
        private XmodeDefine? mCheckMode;


      

        /// <summary>
        /// 需要发送的报数据
        /// </summary>
        private List<XmodePacket> mPacketList;

        /// <summary>
        /// 传输确认长度
        /// </summary>
        private int mTransmitAckLen;

        /// <summary>
        /// 超时时间ms
        /// </summary>
        private readonly int mOverTime;

        /// <summary>
        /// 重试次数
        /// </summary>
        private int mRepeatCount;

        /// <summary>
        /// 最大重试次数
        /// </summary>
        private readonly int mRepeatMaxCount;

        /// <summary>
        /// 客户端回复
        /// </summary>
        private XmodeDefine? mClientReply;


        /// <summary>
        /// 终止标识
        /// </summary>
        private bool mEotFlag;

        /// <summary>
        ///  Xmode数据包管理器
        /// </summary>
        private XmodePacketManager mXmodePacketManager;


        private object mLockObj = new object(); //队列同步对象

        /// <summary>
        /// 发送数据委托
        /// </summary>
        private Action<byte[]> sendDataDelegate;


        /// <summary>
        /// 接收线程
        /// </summary>
        private Thread mReadThread;

        /// <summary>
        /// Xmode服务线程
        /// </summary>
        private Thread mSeverThread;
        /// <summary>
        /// 请求信号量
        /// </summary>
        private ManualResetEvent mRequestData;

        /// <summary>
        /// 存在数据信号量
        /// </summary>
        private ManualResetEvent mExistData;

        /// <summary>
        /// 获取服务状态
        /// </summary>
        private bool serverState = false; 
        /// <summary>
        /// 获取服务状态
        /// </summary>
        public bool ServerState
        {
            get
            {
                return serverState;
            }
            private set
            {
                serverState = value;
            }
        }


        public XmodeServer()
        {
            mOverTime = 60000;
            mRepeatMaxCount = 10;
            initData();
        }
        private void initData()
        {
            ReciveQuene = new Queue<byte>();
            mReciveQueneBuffer = new Queue<byte>();
            mCurrentStep = ServerStep.WaitStartTransmision;
            mTransmitAckLen = 0;
           
            mRepeatCount = 0;
            
            mClientReply = null;
            mEotFlag = false;

            mCurrentStep = ServerStep.WaitStartTransmision;

            mRequestData = new ManualResetEvent(false);
            mExistData = new ManualResetEvent(false);
            serverState = false; 
        }


        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="xmodePacketManager">数据包管理器</param>
        /// <param name="inSendDataDelegate">发送数据委托</param>
        public void StartServer(XmodePacketManager xmodePacketManager, Action<byte[]> inSendDataDelegate)
        {

            initData();

            mXmodePacketManager = xmodePacketManager;
            mReadThread = new Thread(xmodeReadThread);
            mReadThread.Priority = ThreadPriority.Normal;
            mReadThread.Name = "Xmode线程数据";
            mReadThread.Start();

            mSeverThread = new Thread(checkServerStepThread);
            mSeverThread.Priority = ThreadPriority.Normal;
            mSeverThread.Name = "XmodeServer线程";
            mSeverThread.Start();

            serverState = true; 

            sendDataDelegate = inSendDataDelegate;
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
             if (mSeverThread != null)
             {
                 mSeverThread.Join(500);
                 mSeverThread.Abort();
             }
             serverState = false;
        }

       


        
       /// <summary>
       /// 检测传输模式，校验和/累加和
       /// </summary>
        /// <param name="reciveData">接收数据队列</param>
       /// <returns>返回校验方式，否则为空</returns>
        private XmodeDefine? checkTransmitMode(Queue<byte> reciveData)
        {
            //以‘C’启动，为CRC校验方式

            while (reciveData.Count != 0)
            {
                var order = reciveData.Dequeue();
                if ((order == (byte)XmodeDefine.c) || (order == (byte)XmodeDefine.C))
                {
                    return XmodeDefine.C;
                }
                else if (order == (byte)XmodeDefine.NAK)//希望以累积和形式
                {
                    return XmodeDefine.NAK;
                }              
            }
            return null;
        }
        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="packet">数据包</param>
        private void sendData(XmodePacket packet)
        {
            sendDataDelegate(packet.PacketData);
        }
        private void sendData(byte data)
        {
            sendDataDelegate(new byte[]{data});
        }






        /// <summary>
        /// 等待应答
        /// </summary>
        /// <returns>true应答通过</returns>
        private bool CheckData()
        {
            try
            {
                mClientReply = null;
                XmodeDefine data = 0; 
                while(mReciveQueneBuffer.Count != 0)
                {
                    
                    data = (XmodeDefine)mReciveQueneBuffer.Dequeue();
                    switch (data)
                    {

                        case XmodeDefine.NAK://请求重发.
                        case XmodeDefine.ACK://发送下一包数据
                        case XmodeDefine.CAN://无条件终止               
                            {
                                mClientReply = data;
                                return true;
                            }
                        default:
                            {

                                continue;
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
            switch(mClientReply)
            {
                case XmodeDefine.CAN:
                    {
                        //发送终止消息
                        if (ServerEvent != null)
                        {
                            ServerEvent(this, new XmodeServerEventArgs(XmodeServerState.Cancel));                           
                        }
                        return true;
                    }
                case XmodeDefine.ACK:
                    {
                        if (mEotFlag)
                        {
                            //结束应答
                            //发送结束传输消息
                            if (ServerEvent != null)
                            {
                                ServerEvent(this, new XmodeServerEventArgs(XmodeServerState.Sucess));                                
                            }
                            return true;
                        }
                        //正常应答
                        mRepeatCount = 0;
                        ++mTransmitAckLen;
                       
                        break;
                    }
                case XmodeDefine.NAK://要求重传
                    {
                        mRepeatCount = 0;
                        
                        break;
                    }
            }
            return false;
        }
        /// <summary>
        /// 超时调用
        /// </summary>
        /// <returns>true-- 结束本次传输</returns>
        private bool AckOverTime()
        {
            if (ServerEvent != null)
            {
                ServerEvent(this, new XmodeServerEventArgs(XmodeServerState.OverTime));
            }

            if (++mRepeatCount >= mRepeatMaxCount)
            {
                if (ServerEvent != null)
                {
                    ServerEvent(this, new XmodeServerEventArgs(XmodeServerState.Failue));
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 传输数据
        /// </summary>
        private bool TransmitData()
        {
            if (mTransmitAckLen >= mPacketList.Count)
            {
                sendData((byte)XmodeDefine.EOT);//结束传输                    
                mEotFlag = true;
                return true;
            }
            else
            {
                sendData(mPacketList[mTransmitAckLen]);
                return false;
            }
        }

        /// <summary>
        /// XmodeServer数据传输服务线程
        /// </summary>
        private void  checkServerStepThread()
        {
            try
            {
                try
                {
                    //获取启动字符，校验模式
                    do
                    {
                        if (mReciveQueneBuffer.Count == 0)
                        {
                            mRequestData.Set(); //发送请求数据信号
                            mExistData.WaitOne();     //等待有数据信号
                            mRequestData.Reset(); //中断获取数据
                        }
                        mCheckMode = checkTransmitMode(mReciveQueneBuffer);

                    } while (mCheckMode == null);

                    mPacketList = mXmodePacketManager.GetPacketList((XmodeDefine)mCheckMode);
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
                            if(!CheckData())
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
                    Console.WriteLine("XmodeServerThread" + ex.Message);
                    Thread.Sleep(100);
                }
                catch (ThreadAbortException ex)
                {
                    Console.WriteLine("XmodeServerThread:" + ex.Message);
                    Thread.ResetAbort();

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("XmodeServerThread:" + ex.Message);
            }
            
        }

        /// <summary>
        /// 信息入队
        /// </summary>
        /// <param name="data">入队数据</param>
        public void Enqueue(byte[] data)
        {
             lock (ReciveQuene)
             {
                 foreach(var m in data)
                 {
                     ReciveQuene.Enqueue(m);                      
                 }
             }

        }
        /// <summary>
        /// TcpRead进程,从Tcp连接中读取顺序
        /// </summary>
        private void xmodeReadThread()
        {
            try
            {
                try
                {
                    while (true)
                    {

                        Thread.Sleep(10);

                        //等待请求数据信号量请求数据                            
                        if (mRequestData.WaitOne())
                        {
                            //二级缓存数据为空，再从一级缓存转存数据
                            lock (ReciveQuene)
                            {
                                while (ReciveQuene.Count > 0)  //转存到二级缓冲
                                {
                                    mReciveQueneBuffer.Enqueue(ReciveQuene.Dequeue());
                                }
                                if (mReciveQueneBuffer.Count > 0)
                                {
                                    mExistData.Set(); //发送有数据请求
                                }
                                else
                                {
                                    mExistData.Reset();
                                }

                            }
                        }


                    }

                }
                catch (ObjectDisposedException ex)
                {
                    Console.WriteLine("XmodeReadServer" + ex.Message);
                    Thread.Sleep(100);
                }
                catch (ThreadAbortException ex)
                {
                    Console.WriteLine("XmodeReadServer" + ex.Message);
                    Thread.ResetAbort();

                }

            }
            catch (Exception ex)
            {
                while (true)
                {
                    Console.WriteLine("XmodeReadServer" + ex.Message);
                    Thread.Sleep(100);
                }

            }
        }





    }
}
