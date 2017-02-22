using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;



namespace ZFreeGo.TransmissionProtocols.ReciveCenter
{
    public class ControlFlow 
    {
        /// <summary>
        /// 流程步骤
        /// </summary>
        public FlowGather FlowStep;

        public Action TcpStartLinkDelegate;
        public Action TcpLinkACKDelegate;
        public Action StartDataTransmissionDelegate;
        public Action StartDataTransmissionACKDelegate;
        public Action MasterInterrogation;
        public Action MasterInterrogationACKDelegate;
        public Action TimeSynchronizationDelegate;
        public Action TimeSynchronizationACKDelegate;

        public EventControlFlow eventControlFlow;

        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunState;

        private Thread FlowControlThread;

        /// <summary>
        /// 等待控制流
        /// </summary>
        /// <param name="fgNow">当前处理步骤</param>
        /// <param name="fgNext">下一步骤处理</param>
        private void WaitOne(FlowGather fgNow, FlowGather fgNext)
        {
            var m = new EventProperty<FlowGather>(fgNow);

            eventControlFlow.RemoveEventProcess(fgNext);
            eventControlFlow.AddEventProcess(m);

            while (true)
            {
                if (m.Event.WaitOne(new TimeSpan(0, 0, 0, 0, 20)))
                {
                    eventControlFlow.RemoveEventProcess(fgNow);
                    FlowStep = fgNext;
                    break;
                }
                if (! IsRunState)
                {
                    FlowStep = FlowGather.Leisure;
                    break;
                }
            }
           
        }

        /// <summary>
        /// 主处理流程是否处于激活状态 
        /// </summary>
        public void MainTask(FlowGather step)
        {
            try
            {
                switch (step)
                {
                    //建立Tcp连接
                    case FlowGather.TcpStartLink:
                        {
                            TcpStartLinkDelegate();
                            WaitOne(FlowGather.TcpStartLink, FlowGather.StartDataTransmission);


                            break;
                        }
                    //连接确认
                    case FlowGather.TcpLinkACK:
                        {
                            TcpLinkACKDelegate();
                            break;
                        }
                    //启动数据传输
                    case FlowGather.StartDataTransmission:
                        {
                            StartDataTransmissionDelegate();
                            WaitOne(FlowGather.StartDataTransmission, FlowGather.MasterInterrogation); 

                            break;
                        }
                    //确认数据传输
                    case FlowGather.StartDataTransmissionACK:
                        {
                            StartDataTransmissionACKDelegate();
                            break;
                        }
                    //主召唤
                    case FlowGather.MasterInterrogation:
                        {
                            MasterInterrogation();
                            WaitOne(FlowGather.MasterInterrogation, FlowGather.Leisure);

                            break;
                        }
                    //主召唤确认
                    case FlowGather.MasterInterrogationACK:
                        {
                            MasterInterrogationACKDelegate();
                            break;
                        }
                    //时间同步
                    case FlowGather.TimeSynchronization:
                        {
                          //  TimeSynchronizationDelegate(CauseOfTransmissionList.Activation, );
                  
                            //WaitOne(FlowGather.TimeSynchronization, FlowGather.TimeSynchronization);
                            break;
                        }
                    //事件同步确认
                    case FlowGather.TimeSynchronizationACK:
                        {
                            TimeSynchronizationACKDelegate();
                            break;
                        }
                    case FlowGather.Leisure:
                        {
                          //  Console.WriteLine("空闲");
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("未知");

                            break;
                        }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 当前步骤不能完成时，回滚步骤
        /// </summary>
        /// <param name="fgnNow">当前流处理过程</param>
        public void RollbackFlowStep(FlowGather fgnNow)
        {
            try
            {

                if (IsRunState)
                {
                    IsRunState = false;
                    try
                    {
                        FlowControlThread.Join(100);
                        FlowControlThread.Abort();
                    }
                    catch (ThreadAbortException)
                    {

                    }

                    switch (fgnNow)
                    {
                        //建立Tcp连接
                        case FlowGather.TcpStartLink:
                            {
                                FlowStep = fgnNow;
                                ReStartFlow();
                                break;
                            }
                        //连接确认
                        case FlowGather.TcpLinkACK:
                            {

                                break;
                            }
                        //启动数据传输
                        case FlowGather.StartDataTransmission:
                            {

                                FlowStep = fgnNow;
                                ReStartFlow();
                                break;
                            }
                        //确认数据传输
                        case FlowGather.StartDataTransmissionACK:
                            {

                                break;
                            }
                        //主召唤
                        case FlowGather.MasterInterrogation:
                            {


                                ReStartFlow();
                                break;
                            }
                        //主召唤确认
                        case FlowGather.MasterInterrogationACK:
                            {

                                break;
                            }
                        //时间同步
                        case FlowGather.TimeSynchronization:
                            {

                                ReStartFlow();
                                break;
                            }
                        //事件同步确认
                        case FlowGather.TimeSynchronizationACK:
                            {

                                break;
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
                throw ex;
            }


        }

        /// <summary>
        /// 当所选处理完成事，更新控制流处理过程
        /// </summary>
        /// <param name="fgnNow">当前的处理步骤</param>
        public void UpdateFlowStep(FlowGather fgnNow)
        {
            try
            {
                //释放处理流程,处理步骤为当前步骤
                if ((FlowStep == fgnNow) && IsRunState)
                {
                    var m = eventControlFlow.GetEventProcess(fgnNow);
                    m.Event.Set();
                }
            }
            catch (ArgumentNullException)
            {

            }

        }


        /// <summary>
        /// 控制流初始化
        /// </summary>
        /// <param name="tcpStartLinkDelegate">启动Tcp连接委托</param>
        /// <param name="tcpLinkACKDelegate">启动确认委托</param>
        /// <param name="startDataTransmissionDelegate">启动传输委托</param>
        /// <param name="startDataTransmissionACKDelegate">启动传输确认委托</param>
        /// <param name="masterInterrogation">主召唤</param>
        /// <param name="masterInterrogationACKDelegate">主召唤确认</param>
        /// <param name="timeSynchronizationDelegate">时间同步确认</param>
        /// <param name="timeSynchronizationACKDelegate">时间同步确认委托</param>
        public ControlFlow(Action tcpStartLinkDelegate,
        Action tcpLinkACKDelegate, Action startDataTransmissionDelegate, Action startDataTransmissionACKDelegate,
        Action  masterInterrogation, Action masterInterrogationACKDelegate, Action timeSynchronizationDelegate,
        Action timeSynchronizationACKDelegate)
        {
            TcpStartLinkDelegate = tcpStartLinkDelegate;
             TcpLinkACKDelegate  = tcpLinkACKDelegate;
             StartDataTransmissionDelegate = startDataTransmissionDelegate;
             StartDataTransmissionACKDelegate = startDataTransmissionACKDelegate;
             MasterInterrogation = masterInterrogation;
             MasterInterrogationACKDelegate = masterInterrogationACKDelegate;
             TimeSynchronizationDelegate = timeSynchronizationDelegate;
             TimeSynchronizationACKDelegate = timeSynchronizationACKDelegate;

             eventControlFlow = new EventControlFlow();

             FlowControlThread = new Thread(RealFlowControl);
             FlowControlThread.Priority = ThreadPriority.Normal;
        }

        /// <summary>
        /// 控制流初始化
        /// </summary>
        /// <param name="tcpStartLinkDelegate">启动Tcp连接委托</param>
        /// <param name="startDataTransmissionDelegate">启动传输委托</param>
        /// <param name="masterInterrogation">主召唤</param>
        /// <param name="timeSynchronizationDelegate">时间同步确认</param>
        public ControlFlow(Action tcpStartLinkDelegate, 
            Action startDataTransmissionDelegate, 
            Action masterInterrogation,
            Action timeSynchronizationDelegate)
        {
            TcpStartLinkDelegate = tcpStartLinkDelegate;
            StartDataTransmissionDelegate = startDataTransmissionDelegate;
            MasterInterrogation = masterInterrogation;
            TimeSynchronizationDelegate = timeSynchronizationDelegate;

            FlowStep = FlowGather.TcpStartLink;

           // eventControlFlow = new EventControlFlow();
           // FlowControlThread = new Thread(RealFlowControl);
            //FlowControlThread.Priority = ThreadPriority.Normal;
            
        }

        /// <summary>
        /// 启动控制流处理
        /// </summary>
        public void StartFlow()
        {
            //置开始
            if (IsRunState == false)
            {
                FlowStep = FlowGather.TcpStartLink;
                eventControlFlow = new EventControlFlow();
                FlowControlThread = new Thread(RealFlowControl);
                FlowControlThread.Priority = ThreadPriority.Normal;
                IsRunState = true;
                FlowControlThread.Start();
            }
           


        }
        private void ReStartFlow()
        {
            //置开始
            if (IsRunState == false)
            {
                FlowStep = FlowGather.TcpStartLink;
                eventControlFlow = new EventControlFlow();
                FlowControlThread = new Thread(RealFlowControl);
                FlowControlThread.Priority = ThreadPriority.Normal;
                IsRunState = true;
                FlowControlThread.Start();
                
            }



        }
        /// <summary>
        /// 停止流处理过程
        /// </summary>
        public void StopFlow()
        {
            FlowStep = FlowGather.TcpStartLink;
            IsRunState = false;
            try
            {
                if ((FlowControlThread!= null) && (FlowControlThread.IsAlive))
                {
                    FlowControlThread.Join(100);
                    FlowControlThread.Abort();
                }
            }
            catch (ThreadAbortException )
            {

            }
            
        }

        /// <summary>
        /// 控制流进程
        /// </summary>
        void RealFlowControl()
        {
            
            while (IsRunState)
            {
                MainTask(FlowStep);
                Thread.Sleep(50);

            }
            

        }



    }
}
