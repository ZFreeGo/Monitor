using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ApplicationMessage;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;
using ZFreeGo.TransportProtocol.NetworkAccess.Helper;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ControlSystemCommand
{
    /// <summary>
    /// 召唤服务
    /// </summary>
    public class CallServer : ReciveSendServer<MasterCommand>
    {


        /// <summary>
        /// 召唤帧信息
        /// </summary>
        MasterCommand mSendFrame;

        /// <summary>
        /// 接收帧
        /// </summary>
        MasterCommand mReciveFrame;

        Func<MasterCommand, bool> mSendDataDelegate;


        

        /// <summary>
        /// 取消发送标志
        /// </summary>
        private bool cancelSend;

       /// <summary>
       /// 召唤服务启动服务
       /// </summary>
       /// <param name="sendDataDelegate">发送委托</param>
       /// <param name="cot">传输原因</param>
       /// <param name="qoi">传输限定词</param>
        public void StartServer(Func<MasterCommand, bool> sendDataDelegate, CauseOfTransmissionList cot, QualifyOfInterrogationList qoi)
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
                var id = TypeIdentification.C_IC_NA_1;//召唤命令               
                mSendFrame = new MasterCommand(0, 0,
                    id, cot, 0, qoi);

                cancelSend = false;
            }
            catch(Exception ex)
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
            if (cancelSend)
            {
                return true;
            }
            else
            {
                return mSendDataDelegate(mSendFrame);
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
                if(!mReciveFrame.CheckLen())
                {
                    Console.WriteLine("接收帧长度不一致");
                    return false;
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
                switch ((CauseOfTransmissionList)mReciveFrame.ASDU.CauseOfTransmission1)
                {
                    case CauseOfTransmissionList.ActivationACK:
                        {
                            cancelSend = true;//取消下次发送仅仅等待
                            return true;
                        }
                    case CauseOfTransmissionList.ActivateTermination:
                        {
                            
                            return false;//终止循环
                        }
                    default:
                        {
                            ///发送信息
                            break;
                        }
                }
            }
            catch(Exception ex)
            {
                while(true)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(400);
                }
            }
        }


        public override bool AckOverTime()
        {
            if(++mRepeatCount > mRepeatMaxCount)
            {

            }
        }
       


    }
}
