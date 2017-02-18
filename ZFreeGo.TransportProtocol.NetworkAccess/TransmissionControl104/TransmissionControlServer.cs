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
        /// 工作模式 true--发送应答模式，false--只进行接收模式
        /// </summary>
        protected bool mWorkMode;

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

                mThreadName = "TransmissionControlServer-" + DateTime.Now.ToLongTimeString() + "-";

                mReadThread = new Thread(ReciveThread);
                mReadThread.Priority = ThreadPriority.Normal;
                mReadThread.Name = "Read-" + mThreadName;
                mReadThread.Start();
                mServerThread = new Thread(ServerThread);
                mServerThread.Priority = ThreadPriority.Normal;
                mServerThread.Name = "Server-" + mThreadName;
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
        public virtual void SetWorkMode(bool flag)
        {           
            if(flag)
            {               
                mSendTaskFlag = true;
                mWorkMode = true;
                mExistData.Set();
            }
            else
            {               
                mSendTaskFlag = false;
                mWorkMode = false;
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

        /// <summary>
        /// 发送应答处理
        /// </summary>
        protected virtual void SendACkDeal()
        {
            mExistData.Reset();
            if (!TransmitData())
            {
                StopServer();
                return;
            }
            var statTime = DateTime.Now;
            var responseTime = DateTime.Now;
            do
            {
                Thread.Sleep(100);
                mRequestData.Set();//发送请求数据信号

                if (mExistData.WaitOne(mOverTime))     //等待有数据信号
                {
                    if (!ServerState)
                    {
                        return;
                    }
                    mRequestData.Reset();//中断获取数据

                    //超时检测--处理有接收数据但不符合要求的情况
                    responseTime = DateTime.Now;
                    var diffTime = responseTime - statTime;
                    if (diffTime.TotalMilliseconds > mOverTime)
                    {
                        if (AckOverTime())
                        {

                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    //检测数据不符合应答规范则返回
                    if (!CheckData())
                    {
                        continue;
                    }

                    if (!AckOnTime())
                    {
                        break;
                    }
                }
                else
                {
                    if (!AckOverTime())
                    {
                        break;
                    }
                }
                if (!TransmitData())
                {
                    StopServer();
                    break;
                }
                if (!mWorkMode) //转变工作模式
                {
                    break;
                }
                statTime = DateTime.Now; //设置开始时间
            } while (true);

            SetWorkMode(false);
        }

        /// <summary>
        /// 接收处理
        /// </summary>
        public virtual void RecieveDeal()
        {
            mExistData.Reset();
            mRequestData.Set();
            do
            {
                if (mExistData.WaitOne())     //等待有数据信号
                {
                    if (mWorkMode) //转变工作模式
                    {

                        break;
                    }
                    //检测数据不符合应答规范则返回
                    if (!CheckData())
                    {
                        continue;
                    }
                    if (!AckOnTime())
                    {
                        break;
                    }
                }
                Thread.Sleep(100);
            } while (true);
        }
        /// <summary>
        /// 服务进程
        /// </summary>
        public override void ServerThread()
        {
            try
            {
                try
                {
                    do
                    {
                        if (!ServerState)
                        {
                            return;
                        }
                        if (mWorkMode) //发送应答模式应答模式
                        {
                            SendACkDeal();
                        }
                        else  //接收模式
                        {
                            RecieveDeal();
                        }
                    } while (true);

                }
                catch (ObjectDisposedException ex)
                {

                    StopServer();
                    Console.WriteLine("ReciveSend-" + mThreadName + ex.Message);
                    Thread.Sleep(100);
                }
                catch (ThreadAbortException ex)
                {
                    StopServer();
                    Console.WriteLine("ReciveSend-" + mThreadName + ex.Message);
                    Thread.ResetAbort();
                }
            }
            catch (Exception ex)
            {
                StopServer();
                Console.WriteLine("ReciveSend-" + mThreadName + ex.Message);
            }
            StopServer();
        }
        


        
    }
}
