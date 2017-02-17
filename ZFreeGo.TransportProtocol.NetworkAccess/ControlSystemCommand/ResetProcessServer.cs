using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransportProtocol.NetworkAccess.BasicElement;
using ZFreeGo.TransportProtocol.NetworkAccess.Helper;

namespace ZFreeGo.TransportProtocol.NetworkAccess.ControlSystemCommand
{
    /// <summary>
    /// 复位进程命令
    /// </summary>
    public class ResetProcessServer : ReciveSendServer<ControlCommandASDU>
    {
        /// <summary>
        /// 召唤服务事件
        /// </summary>
        public event EventHandler<ResetProcessArgs> ServerEvent;


        /// <summary>
        /// 帧信息
        /// </summary>
        private ControlCommandASDU mSendFrame;

        /// <summary>
        /// 接收帧
        /// </summary>
        private ControlCommandASDU mReciveFrame;

        private Func<ControlCommandASDU, bool> mSendDataDelegate;

  

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="sendDataDelegate">发送委托</param>
        /// <param name="cot">传输原因</param>
        /// <param name="qoi">传输限定词</param>
        public void StartServer(Func<ControlCommandASDU, bool> sendDataDelegate, CauseOfTransmissionList cot, QualifyResetProgressList qrp)
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
                var id = TypeIdentification.C_RP_NA_1;//复位经进程命令 
                mSendFrame = new ControlCommandASDU(id, cot, 0, qrp);
               
            }
            catch (Exception ex)
            {
                throw ex;
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
                SendEvent("发送失败，终止处理。", ControlSystemServerResut.SendFault);
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
                           
                            SendEvent("召唤激活确认", ControlSystemServerResut.AcvtivityAck);
                            mRepeatCount = 0;
                            return false;
                        }
                    case CauseOfTransmissionList.ActivateTermination:
                        {
                            SendEvent("召唤激活终止", ControlSystemServerResut.ActivateTermination);
                            mRepeatCount = 0;
                            return false;//终止循环
                        }
                    default:
                        {
                            SendEvent("未识别的ID", ControlSystemServerResut.Unknow);
                            return false;
                        }
                }
            }
            catch (Exception ex)
            {
                SendEvent(ex.Message, ControlSystemServerResut.Error);
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
               
                SendEvent(string.Format("应答超时,进行第{0}次重试.", mRepeatCount), ControlSystemServerResut.OverTime);
                return true;
            }
            else
            {
                SendEvent(string.Format("重试失败，结束召唤。", mRepeatCount), ControlSystemServerResut.Fault);
                return false;
            }
        }


        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="comment">注释</param>
        /// <param name="result">结果</param>
        public void SendEvent(string comment, ControlSystemServerResut result)
        {
            if (ServerEvent != null)
            {
                ServerEvent(Thread.CurrentThread, new ResetProcessArgs(comment, result));
            }
        }
    }
}
