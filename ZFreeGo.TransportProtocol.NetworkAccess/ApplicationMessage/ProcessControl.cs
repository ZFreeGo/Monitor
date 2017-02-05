using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ApplicationMessage
{
    public class ProcessControlPure
    {
        /// <summary>
        /// 事件控制
        /// </summary>
        public ManualResetEvent mEvent;

        /// <summary>
        /// 处理控制线程
        /// </summary>
        public Thread processThread;
        public bool IsRun;
        public void StopThread()
        {
            try
            {
                IsRun = false;
                if ((processThread != null) && (processThread.IsAlive))
                {
                    if (mEvent!=null)
                    {
                        mEvent.Set();
                       // mEvent.Close();

                       // mEvent.Dispose();
                    }
                    processThread.Join(100);
                    processThread.Abort();
                }
            }
            catch (ThreadAbortException)
            {

            }
        }
    }

    public class ProcessControl<T1, T2, T3> : ProcessControlPure
    {
        /// <summary>
        /// 动作委托
        /// </summary>
        private Action<T1> actionDelegate;
        /// <summary>
        /// 动作完成委托
        /// </summary>
        private Action<T2> actionCompletedDelegate;
        /// <summary>
        /// 动作未完成委托
        /// </summary>
        private Action<T3> actionNoCompletedDelegate;

     

        T1 data1;
        T2 data2;
        T3 data3;

       
        //超时控制
        int millisecondsTimeOut;

        /// <summary>
        /// 启动带有超时检测的控制过程
        /// </summary>
        private void StartProcess()
        {
            //发送启动
            actionDelegate(data1);

           
            if(mEvent.WaitOne(millisecondsTimeOut, false)) //等待过长
            {
                if (IsRun == false)
                {
                    return;
                }
                actionCompletedDelegate(data2);
            }
            else
            {
                if (IsRun == false)
                {
                    return;
                }
                actionNoCompletedDelegate(data3);
            }
            IsRun = false;
           // mEvent.Close();
           // mEvent.Dispose();
        }

        public void StartProcessThread()
        {
            processThread = new Thread(StartProcess);
            processThread.Priority = ThreadPriority.Normal;
            processThread.Start();
            IsRun = true;
        }
        
        /// <summary>
        /// 带有超时检测的控制过程
        /// </summary>
        /// <param name="data1">数据1</param>
        /// <param name="data2">数据2</param>
        /// <param name="data3">数据3</param>
        /// <param name="actionDelegate">动作委托</param>
        /// <param name="actionCompletedDelegate">动作完成委托</param>
        /// <param name="actionNoCompletedDelegate">动作未完成委托</param>
        /// <param name="millisecondsTimeOut">超时检测时间</param>
        /// <param name="mevent">事件</param>
         public ProcessControl(T1 data1, T2 data2, T3 data3, Action<T1> actionDelegate, Action<T2> actionCompletedDelegate,
            Action<T3> actionNoCompletedDelegate, int millisecondsTimeOut, ManualResetEvent mevent)
        {
            this.data1 = data1;
            this.data2 = data2;
            this.data3 = data3;

            this.actionDelegate = actionDelegate;
            this.actionCompletedDelegate = actionCompletedDelegate;
            this.actionNoCompletedDelegate = actionNoCompletedDelegate;
            this.millisecondsTimeOut = millisecondsTimeOut;
            this.mEvent = mevent;
            IsRun = false;
        }

        
        

    }
}
