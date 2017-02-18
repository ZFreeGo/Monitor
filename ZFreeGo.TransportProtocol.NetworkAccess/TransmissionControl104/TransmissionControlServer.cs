using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransmissionProtocols.Frame104;
using ZFreeGo.TransmissionProtocols.Helper;

namespace ZFreeGo.TransmissionProtocols.TransmissionControl104
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
        /// 发送任务标志,true-有发送任务需要进行发送检测，false--跳过发送步骤
        /// </summary>
        private bool mSendTaskFlag;

       /// <summary>
       /// 传输控制功能初始化
       /// </summary>
       /// <param name="sendDataDelegate">发送委托</param>
        public TransmissionControlServer(Func<APCITypeU, bool> sendDataDelegate)
        {
            mSendDataDelegate = sendDataDelegate;
           
            StartServer();
            SetWorkMode(false);
        }
        
        /// <summary>
        /// 启动服务
        /// </summary>             
        private void StartServer()
        {
            try
            {
                if (ServerState)
                {
                    throw new Exception("服务正在运行，禁止重复启动");
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
                 
                

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置工作模式
        /// </summary>
        /// <param name="flag">true-发送应答模式，false--接收模式</param>
        public override void SetWorkMode(bool flag)
        {
            base.SetWorkMode(flag);
            if(flag)
            {               
                mSendTaskFlag = true;
            }
            else
            {               
                mSendTaskFlag = false;
            }
        }
     

        /// <summary>
        /// 传输控制
        /// </summary>
        /// <param name="tcf">传输控制功能</param>
        public void SendTransmissonCommand(TransmissionControlFunction tcf)
        {
            if (mSendTaskFlag)
            {
                throw new Exception("正在执行任务，暂时不能执行此功能.");
            }
            mSendFrame = new APCITypeU(tcf);
            mRepeatCount = 0;
            SetWorkMode(true);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <returns>true-成功，false-失败，终止线程</returns>
        public override bool TransmitData()
        {
            if (mSendTaskFlag)
            {
                bool state = mSendDataDelegate(mSendFrame);
                if (!state)
                {
                    SendFaultEvent("发送失败，终止处理。", TransmissionControlResult.SendFault);
                    SetWorkMode(false);
                }
                return state;
            }
            else
            {
                return true; ;
            }
           
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
        } 
        
        /// <summary>
        /// 准时应答后相应的事件
        /// </summary>
        /// <returns>true--正常执行，false--结束执行，终止当前线程</returns>
        public override bool AckOnTime()
        {
            try
            {
                switch (mReciveFrame.TransmissionCotrolFun)
                {
                        case TransmissionControlFunction.StartDataTransmission:
                        {
                            SendEvent(mReciveFrame.TransmissionCotrolFun);
                            break;
                        }
                    case TransmissionControlFunction.AcknowledgementStartDataTransmission:
                        {
                            if (mSendFrame.TransmissionCotrolFun == TransmissionControlFunction.StartDataTransmission)
                            {
                                 SendEvent(mReciveFrame.TransmissionCotrolFun);
                                 SetWorkMode(false);
                                 return false;
                            }
                            else
                            {
                                SendFaultEvent("AcknowledgementStartDataTransmission：发送与接收不对应", TransmissionControlResult.Unknow);
                                return false;
                            }
                                                       
                        }
                    case TransmissionControlFunction.StopDataTransmission:
                        {
                            SendEvent(mReciveFrame.TransmissionCotrolFun);  
                            break;
                        }
                    case TransmissionControlFunction.AcknowledgementStopDataTransmission:
                        {
                            if (mSendFrame.TransmissionCotrolFun == TransmissionControlFunction.StopDataTransmission)
                            {
                                 SendEvent(mReciveFrame.TransmissionCotrolFun);
                                 SetWorkMode(false);
                                 return false;
                            }
                            else
                            {
                                  SendFaultEvent("AcknowledgementStopDataTransmission：发送与接收不对应", TransmissionControlResult.Unknow);
                                  return false;
                            }
                            
                           
                        }
                    case TransmissionControlFunction.TestFrame:
                        {
                            SendEvent(mReciveFrame.TransmissionCotrolFun);
                            mSendDataDelegate(new APCITypeU(TransmissionControlFunction.AcknowledgementTestFrame));
                            break;
                        }
                    case TransmissionControlFunction.AcknowledgementTestFrame:
                        {
                            if(mSendFrame.TransmissionCotrolFun == TransmissionControlFunction.TestFrame)
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
            if (mSendTaskFlag)
            {
                if (++mRepeatCount < mRepeatMaxCount)
                {
                    SendFaultEvent(string.Format("应答超时,进行第{0}次重试.", mRepeatCount), TransmissionControlResult.OverTime);
                    return true;
                }
                else
                {
                    SendFaultEvent(string.Format("重试失败，传输控制功能。", mRepeatCount), TransmissionControlResult.Fault);
                    SetWorkMode(false);
                    return false;

                }
            }
            return true;
           
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="tcf">传输控制功能</param>
        public void SendEvent(TransmissionControlFunction tcf)
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
