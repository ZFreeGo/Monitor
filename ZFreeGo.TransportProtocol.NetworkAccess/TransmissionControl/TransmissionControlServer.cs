using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ControlSystemCommand;
using ZFreeGo.TransportProtocol.NetworkAccess.Helper;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.TransmissionControl
{
    /// <summary>
    /// 传输控制功能服务
    /// </summary>
    public class TransmissionControlServer : ReciveSendServer<APCITypeU>
    {
        /// <summary>
        /// 传输服务
        /// </summary>
        public event EventHandler<TransmissionControlEventArgs> ServerEvent;

        /// <summary>
        /// 传输故障事件
        /// </summary>
        public event EventHandler<TransmissionControlFaultEventArgs> ServerFaultEvent;

        /// <summary>
        /// 帧信息
        /// </summary>
        private APCITypeU mSendFrame;

        /// <summary>
        /// 接收帧
        /// </summary>
        private APCITypeU mReciveFrame;

        private Func<APCITypeU, bool> mSendDataDelegate;

        /// <summary>
        /// 发送标志
        /// </summary>
        private bool sendFlag;

        
        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="sendDataDelegate">发送委托</param>
       /// <param name="tcf">传输控制功能</param>
        public void StartServer(Func<APCITypeU, bool> sendDataDelegate)
        {
            try
            {
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
                sendFlag =false;       
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 传输控制功能发送
        /// </summary>
        /// <param name="tcf">传输控制功能</param>
        public void SendTCF(TransmissionCotrolFunction tcf)
        {
             mSendFrame = new APCITypeU(tcf);
                sendFlag = true;
             TransmitData();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <returns>true-成功，false-失败，终止线程</returns>
        public override bool TransmitData()
        {
            if(sendFlag)
            {
                bool state = mSendDataDelegate(mSendFrame);
                if (!state)
                {
                    SendFaultEvent("发送失败，终止处理。", TransmissionControlResult.SendFault);
                }
                return state;
            }
            return true;
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
               
                return true;
            }
            else
            {
                return false;
            }
        } /// <summary>
        /// 准时应答后相应的事件
        /// </summary>
        /// <returns>true--正常执行，false--结束执行，终止当前线程</returns>
        public override bool AckOnTime()
        {
            try
            {
                switch (mReciveFrame.TransmissionCotrolFun)
                {
                        case TransmissionCotrolFunction.StartDataTransmission:
                        {
                            SendEvent(mReciveFrame.TransmissionCotrolFun);
                            break;
                        }
                    case TransmissionCotrolFunction.AcknowledgementStartDataTransmission:
                        {
                            if (mSendFrame.TransmissionCotrolFun == TransmissionCotrolFunction.AcknowledgementStartDataTransmission)
                            {
                                 SendEvent(mReciveFrame.TransmissionCotrolFun); 
                            }
                            else
                            {
                                SendFaultEvent("AcknowledgementStartDataTransmission：发送与接收不对应", TransmissionControlResult.Unknow);
                            }
                                                   
                            
                            break;
                        }
                    case TransmissionCotrolFunction.StopDataTransmission:
                        {
                            SendEvent(mReciveFrame.TransmissionCotrolFun);  
                            break;
                        }
                    case TransmissionCotrolFunction.AcknowledgementStopDataTransmission:
                        {
                            if (mSendFrame.TransmissionCotrolFun == TransmissionCotrolFunction.StartDataTransmission)
                            {
                                 SendEvent(mReciveFrame.TransmissionCotrolFun);  
                            }
                            else
                            {
                                  SendFaultEvent("AcknowledgementStopDataTransmission：发送与接收不对应", TransmissionControlResult.Unknow);
                            }
                           
                            break;
                        }
                    case TransmissionCotrolFunction.TestFrame:
                        {
                            SendEvent(mReciveFrame.TransmissionCotrolFun);  
                            break;
                        }
                    case TransmissionCotrolFunction.AcknowledgementTestFrame:
                        {
                            if(mSendFrame.TransmissionCotrolFun == TransmissionCotrolFunction.TestFrame)
                            {
                                SendEvent(mReciveFrame.TransmissionCotrolFun); 
                            }
                            else
                            {
                                 SendFaultEvent("AcknowledgementTestFrame：发送与接收不对应", TransmissionControlResult.Unknow);
                            }

                            
                            break;
                        }
                   
                    default:
                        {
                            SendFaultEvent("未识别的功能", TransmissionControlResult.UnkowID);
                            break;
                        }
                }
                return true;
            }
            catch (Exception ex)
            {
                SendFaultEvent(ex.Message, TransmissionControlResult.Error);
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

                SendFaultEvent(string.Format("应答超时,进行第{0}次重试.", mRepeatCount), TransmissionControlResult.OverTime);
                return true;
            }
            else
            {
                SendFaultEvent(string.Format("重试失败，结束召唤。", mRepeatCount), TransmissionControlResult.Fault);
                return false;
            }
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="tcf">传输控制功能</param>
        public void SendEvent(TransmissionCotrolFunction tcf)
        {
            if (ServerEvent != null)
            {
                ServerEvent(Thread.CurrentThread, new TransmissionControlEventArgs(tcf, tcf.ToString()));
            }
        }
        /// <summary>
        /// 发送故障事件
        /// </summary>
        /// <param name="comment">注释</param>
        /// <param name="tcr">传输控制功能</param>
        public void SendFaultEvent(string comment, TransmissionControlResult tcr)
        {
            if (ServerFaultEvent != null)
            {
                ServerFaultEvent(Thread.CurrentThread, new TransmissionControlFaultEventArgs(comment, tcr));
            }
        }




        
    }
}
