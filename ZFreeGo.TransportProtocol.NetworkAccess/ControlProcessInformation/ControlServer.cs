﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ApplicationMessage;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;
using ZFreeGo.TransportProtocol.NetworkAccess.Helper;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ControlProcessInformation
{
    /// <summary>
    /// 控制方向的过程信息，遥控
    /// </summary>
    public class ControlServer : ReciveSendServer<ControlProcessASDU>
    {
        /// <summary>
        /// 控制服务事件
        /// </summary>
        public event EventHandler<ControlEventArgs> ServerEvent;


        /// <summary>
        /// 帧信息
        /// </summary>
        private ControlProcessASDU mSendFrame;

        /// <summary>
        /// 接收帧
        /// </summary>
        private ControlProcessASDU mReciveFrame;

        private Func<ControlProcessASDU, bool> mSendDataDelegate;

        /// <summary>
        /// 发送命令的信息体地址
        /// </summary>
        private UInt32 mObjectAddress;

        /// <summary>
        /// 发送的双点命令
        /// </summary>
        private DoubleCommand mDCO;


        private bool activityAckFlag;
        /// <summary>
        /// 激活应答标志
        /// </summary>
        public bool ActivityAckFlag
        {
            get
            {
                return activityAckFlag;
            }
            private set
            {
                activityAckFlag = value;
            }
        }

        /// <summary>
        /// 等待执行命令事件
        /// </summary>
        private ManualResetEvent mWaitExecueCommand;

        /// <summary>
        /// 启动服务,发送预制信息
        /// </summary>
        /// <param name="sendDataDelegate">发送委托</param>
        /// <param name="cot">传输原因</param>
        /// <param name="asduPublicAddress">公共地址</param>
        /// <param name="objectAddress">信息对象地址</param>
        /// <param name="dco">双点命令</param> 
        public void StartServer(Func<ControlProcessASDU, bool> sendDataDelegate, CauseOfTransmissionList cot,
              UInt16 asduPublicAddress, UInt32 objectAddress, DoubleCommand dco)
        {
            try
            {
                if (serverState)
                {
                    throw new ArgumentException("当前服务正在执行，禁止重复启动");
                }

                if(dco.SE != SelectExecuteOption.Select)
                {
                    throw new ArgumentException("此命令必须是选择命令");
                }

                InitData();
                mReadThread = new Thread(ReciveThread);
                mReadThread.Priority = ThreadPriority.Normal;
                mReadThread.Name = "ReciveThread线程数据";
                mReadThread.Start();

                mServerThread = new Thread(ServerThread);
                mServerThread.Priority = ThreadPriority.Normal;
                mServerThread.Name = "ServerThread线程";
                mServerThread.Start();
                serverState = true;

                mSendDataDelegate = sendDataDelegate;
                var id = TypeIdentification.C_DC_NA_1;//遥控命令
                mSendFrame = new ControlProcessASDU(id, cot, 0, objectAddress, dco);
                //信息对象地址

                mObjectAddress = objectAddress;
                mDCO = dco;
                
                mOverTime = 15000;
                mRepeatCount = 1;
                ActivityAckFlag = false;
                mWaitExecueCommand = new ManualResetEvent(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送执行命令
        /// </summary>
        /// <param name="objectAddress">信息体地址</param>
        /// <param name="dco">双点命令</param>
        public void SendActionCommand(CauseOfTransmissionList cot,
              UInt16 asduPublicAddress, UInt32 objectAddress, DoubleCommand dco)
        {
            if (ActivityAckFlag && ServerState)
            {
                if (mObjectAddress != objectAddress)
                {
                    throw new Exception("信息对象地址不一致");
                }
                //此命令必须为执行命令
                if (dco.SE == SelectExecuteOption.Execute)
                {
                    throw new Exception("此命令需要是执行命令");
                }

                var id = TypeIdentification.C_DC_NA_1;//遥控命令
                mSendFrame = new ControlProcessASDU(id, cot, 0, objectAddress, dco);

                bool state = mSendDataDelegate(mSendFrame);
                if (state)
                {
                    SendEvent("发送失败，终止处理。", ControlProcessServerResult.SendFault);
                    StopServer();
                }
                mWaitExecueCommand.Set();
            }
            else
            {
                throw new Exception("命令尚未激活,发送无效");
            }

        }


        /// <summary>
        /// 发送数据
        /// </summary>
        /// <returns>true-成功，false-失败，终止线程</returns>
        public override bool TransmitData()
        {
            bool state = mSendDataDelegate(mSendFrame);
            if (!state)
            {
                SendEvent("发送失败，终止处理。", ControlProcessServerResult.SendFault);
            }
            return state;
        }

        /// <summary>
        /// 检测接收数据, Todo::没有实现检测,直接提取
        /// </summary>
        /// <returns>true--检测通过，false--检测失败</returns>
        public override bool CheckData()
        {
            if (mReciveQuene.Count > 0)
            {
                mReciveFrame = mReciveQuene.Dequeue();
                UInt32 objectAddr = ElementTool.CombinationByte(mReciveFrame.InformationObject[0], mReciveFrame.InformationObject[1],
                    mReciveFrame.InformationObject[2]);
                if (mObjectAddress != objectAddr)
                {
                    ServerEvent(Thread.CurrentThread, new ControlEventArgs("信息对象地址不一致", ControlProcessServerResult.Error));
                }               
                var dc = new DoubleCommand(mReciveFrame.InformationObject[3]);
                if (dc.DCO != mDCO.DCO)
                {
                    ServerEvent(Thread.CurrentThread, new ControlEventArgs("双点命令收发不一致", ControlProcessServerResult.Error));
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 准时应答后相应的事件
        /// </summary>
        /// <returns>true--正常执行，false--结束执行，终止当前线程</returns>
        public override bool AckOnTime()
        {
            try
            {
                switch ((CauseOfTransmissionList)mReciveFrame.CauseOfTransmission1)
                {
                    case CauseOfTransmissionList.ActivationACK:
                        {
                            mRepeatCount = 0;
                            SendEvent("激活确认", ControlProcessServerResult.AcvtivityAck);
                            if (ActivityAckFlag)
                            {
                                return false;
                            }
                            else
                            {
                                ActivityAckFlag = true;
                            }

                            if(!mWaitExecueCommand.WaitOne(mOverTime))
                            {
                                SendEvent("执行命令超时", ControlProcessServerResult.OverTime);
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    case CauseOfTransmissionList.ActivateTermination:
                        {
                            SendEvent("激活终止", ControlProcessServerResult.ActivateTermination);
                            mRepeatCount = 0;
                            return false;//终止循环
                        }
                    default:
                        {
                            SendEvent("未识别的传输原因", ControlProcessServerResult.Unknow);
                            return false;
                        }
                }
            }
            catch (Exception ex)
            {
                SendEvent(ex.Message, ControlProcessServerResult.Error);
                StopServer();
                return false;
            }
        }

        /// <summary>
        /// 超时后执行事件
        /// </summary>
        /// <returns>true--继续，fals--终止</returns>
        public override bool AckOverTime()
        {
            if (++mRepeatCount < mRepeatMaxCount)
            {
                SendEvent(string.Format("应答超时,进行第{0}次重试.", mRepeatCount), ControlProcessServerResult.OverTime);
                return true;
            }
            else
            {
                SendEvent(string.Format("重试失败，结束服务。", mRepeatCount), ControlProcessServerResult.Fault);
                return false;
            }
        }


        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="comment">注释</param>
        /// <param name="result">结果</param>
        public void SendEvent(string comment, ControlProcessServerResult result)
        {
            if (ServerEvent != null)
            {
                ServerEvent(Thread.CurrentThread, new ControlEventArgs(comment, result));
            }
        }
    }
}
