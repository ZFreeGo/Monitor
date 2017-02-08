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
        /// 接收线程
        /// </summary>
        private Thread readThread;

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


        private object lockObj = new object(); //队列同步对象

        /// <summary>
        /// 发送数据委托
        /// </summary>
        private Action<byte[]> sendDataDelegate;

        /// <summary>
        /// 当前函数
        /// </summary>
        private FunctionTimeout mCurrentFunction;





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
        }


        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="xmodePacketManager">数据包管理器</param>
        /// <param name="inSendDataDelegate">发送数据委托</param>
        public void StartServer(XmodePacketManager xmodePacketManager, Action<byte[]> inSendDataDelegate)
        {
            if(mCurrentFunction != null)
            {
                mCurrentFunction.StopDelegate();
            }
            initData();

            mXmodePacketManager = xmodePacketManager;
            readThread = new Thread(xmodeReadThread);
            readThread.Priority = ThreadPriority.Normal;
            readThread.Name = "Xmode线程数据";
            readThread.Start();
            sendDataDelegate = inSendDataDelegate;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void CloseServer()
        {
            if (readThread != null)
            {
                readThread.Join(500);
                readThread.Abort();
               
            }
        }

       


        
       /// <summary>
       /// 检测传输模式，校验和/累加和
       /// </summary>
       /// <param name="order">命令字节</param>
       /// <returns>返回校验方式，否则为空</returns>
        private XmodeDefine? checkTransmitMode(byte order)
        {
            //以‘C’启动，为CRC校验方式
            if ((order == (byte)XmodeDefine.c) || (order == (byte)XmodeDefine.C))
            {
                return XmodeDefine.C;
            }
            else if(order == (byte)XmodeDefine.NAK)//希望以累积和形式
            {
                return XmodeDefine.NAK;
            }
            else
            {
                return null;
            }
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
        private void waitReply()
        {
            try
            {
                mClientReply = null;
                while (true)
                {
                    if (mReciveQueneBuffer.Count == 0)
                    {
                        return;
                    }
                    var data = (XmodeDefine)mReciveQueneBuffer.Dequeue();
                    switch (data)
                    {

                        case XmodeDefine.NAK://请求重发.
                        case XmodeDefine.ACK://发送下一包数据
                        case XmodeDefine.CAN://无条件终止               
                            {
                                mClientReply = data;
                                return;
                            }
                        default:
                            {

                                break;
                            }
                    }
                }
            }
            catch(Exception ex)
            {
                
            }
        }
        /// <summary>
        /// 时间内完成调用
        /// </summary>
        private void callBackOnTime()
        {
            switch(mClientReply)
            {
                case XmodeDefine.CAN:
                    {
                        //发送终止消息
                        ServerEvent(this, new XmodeServerEventArgs(XmodeServerState.Cancel));
                        break;
                    }
                case XmodeDefine.ACK:
                    {
                        if (mEotFlag)
                        {
                            //结束应答
                            //发送结束传输消息
                            ServerEvent(this, new XmodeServerEventArgs(XmodeServerState.Sucess));
                            return;
                        }
                        //正常应答
                        mRepeatCount = 0;
                        ++mTransmitAckLen;
                        transmitData();
                        break;
                    }
                case XmodeDefine.NAK://要求重传
                    {
                        mRepeatCount = 0;
                        transmitData();
                        break;
                    }
            }
        }
        /// <summary>
        /// 超时调用
        /// </summary>
        private void callBackOverTime()
        {
            //手动终止，则停止传输
            if (mCurrentFunction.ManualAbort)
            {
                return;
            }
            ServerEvent(this, new XmodeServerEventArgs(XmodeServerState.OverTime));
            ++mRepeatCount;

            transmitData();
        }
        /// <summary>
        /// 传输数据
        /// </summary>
        private void transmitData()
        {
            if (mRepeatCount < mRepeatMaxCount)
            {
                if (mTransmitAckLen >= mPacketList.Count)
                {
                    sendData((byte)XmodeDefine.EOT);//结束传输
                    
                    mEotFlag = true;
                }
                else
                {
                    sendData(mPacketList[mTransmitAckLen]);
                }
                mCurrentFunction = new FunctionTimeout(waitReply,
                    callBackOnTime, callBackOverTime, mOverTime);
                mCurrentFunction.DoAction();
            }
            else
            {
                ServerEvent(this, new XmodeServerEventArgs(XmodeServerState.Failue));
            }
        }

        /// <summary>
        /// 检测服务步骤
        /// </summary>
        private void  checkServerStep()
        {
            if (mReciveQueneBuffer.Count == 0)
            {
                return;
            }
            //若为传输状态则暂停此步骤执行
            if (mCurrentStep == ServerStep.TransmisionData)
            {
                return;
            }
            switch (mCurrentStep)
            {
                case ServerStep.WaitStartTransmision:
                    {
                        mCheckMode = checkTransmitMode(mReciveQueneBuffer.Dequeue());
                        if (mCheckMode != null)
                        {
                            mPacketList = mXmodePacketManager.GetPacketList((XmodeDefine)mCheckMode);
                            mCurrentStep = ServerStep.TransmisionData;
                        }
                        break;
                    }
                case ServerStep.TransmisionData:
                    {
                        transmitData();
                        break;
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
                        //二级缓存数据为空，再从一级缓存转存数据

                        lock (ReciveQuene)
                        {
                            while (ReciveQuene.Count > 0)  //转存到二级缓冲
                            {
                                mReciveQueneBuffer.Enqueue(ReciveQuene.Dequeue());
                            }

                        }

                        if (mReciveQueneBuffer.Count > 0)
                        {
                            //MainCheckStep();
                        }

                        Thread.Sleep(10);
                    }

                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();

                }

            }
            catch (Exception ex)
            {
                while (true)
                {
                    Console.WriteLine("XmodeServer" + ex.Message);
                    Thread.Sleep(100);
                }

            }
        }





    }
}
