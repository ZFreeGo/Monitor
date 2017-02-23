using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocols.BasicElement;

namespace ZFreeGo.TransmissionProtocols.MonitorProcessInformation
{
    /// <summary>
    /// 事件记录事件信息
    /// </summary>
    public class EventLogEventArgs<T1, T2> : EventArgs
    {
        public TypeIdentification StatusID;
        /// <summary>
        /// 状态信息
        /// </summary>
        public List<T1> StatusMessage;


        public TypeIdentification MeteringID;
        /// <summary>
        /// 遥测信息
        /// </summary>
        public List<T2> MeteringMessage;

        /// <summary>
        /// 事件记录信息
        /// </summary>
        /// <param name="status">状态-遥信</param>
        /// <param name="metering">测量-遥测</param>
        public EventLogEventArgs(TypeIdentification statusID, List<T1> status,
            TypeIdentification meteringID, List<T2> metering)
        {
            StatusID = statusID;
            StatusMessage = status;
            MeteringID = meteringID;
            MeteringMessage = metering;
        }

    }
}
