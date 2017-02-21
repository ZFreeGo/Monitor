using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.Frame;
using ZFreeGo.TransmissionProtocols.Helper;

namespace ZFreeGo.TransmissionProtocols.ControlProcessInformation
{
    /// <summary>
    /// 控制方向的过程信息，设定值
    /// </summary>
    public class SetPointServer : ReciveSendServer<ApplicationServiceDataUnit>
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
        private ApplicationServiceDataUnit mReciveFrame;

        private Func<ControlProcessASDU, bool> mSendDataDelegate;

        QualifyCommandSet mQos;


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
        /// <param name="sendDataDelegate">发送委托</param>
        private ManualResetEvent mWaitExecueCommand;

        public SetPointServer(Func<ControlProcessASDU, bool> sendDataDelegate)
        {
             mSendDataDelegate = sendDataDelegate;
        }

        /// <summary>
        /// 启动服务,发送预制信息
        /// </summary>
        
        /// <param name="cot">传输原因</param>
        /// <param name="asduPublicAddress">公共地址</param>
        /// <param name="objectAddress">信息对象地址</param>
        /// <param name="dco">双点命令</param> 
        public void StartServer( bool isquense,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, QualifyCommandSet qos,
             List<Tuple<UInt32, ShortFloating>> listFloat)
        {
            try
            {
                if (serverState)
                {
                    throw new ArgumentException("当前服务正在执行，禁止重复启动");
                }
                if(qos.Describle != ActionDescrible.Select)
                {
                    throw new ArgumentException("需要先执行选择任务");
                }
                InitData();
                    
                var id = TypeIdentification.C_SE_NC_1;//设定值命令 短浮点数
                mSendFrame = new ControlProcessASDU(id,isquense, cot, 0, qos, listFloat);
                
                mOverTime = 15000;
                mRepeatCount = 1;
                mQos = qos;
                ActivityAckFlag = false;
                mWaitExecueCommand = new ManualResetEvent(false);


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

      
        public void SendActionCommand(bool isquense,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, QualifyCommandSet qos,
             List<Tuple<UInt32, ShortFloating>> listFloat)
        {
            if (ActivityAckFlag && ServerState)
            {
                //if (mObjectAddress != objectAddress)
                //{
                //    throw new Exception("信息对象地址不一致");
                //}
                //此命令必须为执行命令
                if (qos.Describle == ActionDescrible.Execute)
                {
                    throw new Exception("此命令需要是执行命令");
                }

                var id = TypeIdentification.C_SE_NC_1;//设定值命令 短浮点数
                mSendFrame = new ControlProcessASDU(id, isquense, cot, 0, qos, listFloat);

                bool state = mSendDataDelegate(mSendFrame);
                if (!state)
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
                

                //需要反校参数
                var ds = new QualifyCommandSet(mReciveFrame.InformationObject[mReciveFrame.InformationObject.Length - 1]);
                if (ds.QOS != mQos.QOS)
                {
                    ServerEvent(Thread.CurrentThread, new ControlEventArgs("命令不一致", ControlProcessServerResult.Error));
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
