using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace ZFreeGo.TransportProtocol.NetworkAccess.ReciveCenter
{

   

    /// <summary>
    /// 通过定时器实现流程控制
    /// </summary>
    public class ControlFlowTimer
    {
       // private Timer loopCallTimer;


       /// <summary>
       /// 建立TCP连接委托
       /// </summary>
        public Func<bool> TcpStartLinkDelegate;
        /// <summary>
        /// 获取TCP连接状态委托
        /// </summary>
        public Func<bool> GetTcpLinkStateDelegate;
        /// <summary>
        /// 启动数据传输委托
        /// </summary>
        public Func<bool> StartDataTransmissionDelegate;
        /// <summary>
        /// 获取启动数据传输状态委托
        /// </summary>
        public Func<bool> GetStartDataTransmission;
        /// <summary>
        /// 总召唤委托
        /// </summary>
        public Func<bool> MasterInterrogationDelegate;
        /// <summary>
        /// 获取总召唤委托
        /// </summary>
        public Func<bool> GetMasterInterrogationStateDelegate;
        /// <summary>
        /// 时间同步委托
        /// </summary>
        public Func<bool> TimeSynchronizationDelegate;
        /// <summary>
        /// 获取时间同步状态
        /// </summary>
        public Func<bool> GetTimeSynchronizationStateDelegate;
        /// <summary>
        /// 停止数据传输委托
        /// </summary>
        public Func<bool> StopDataTransmissionDelegate;


        public Func<bool> StopControl;

        private Timer TimeOutTimer;

        public  ControlFlowTimer()
        {
            TimeOutTimer = new Timer(40000);
            TimeOutTimer.AutoReset = false;
            TimeOutTimer.Elapsed += TimeOutTimer_Elapsed;





        }

        void TimeOutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
