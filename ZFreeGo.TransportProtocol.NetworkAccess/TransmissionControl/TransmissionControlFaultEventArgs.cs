using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.TransmissionControl
{
    /// <summary>
    /// 传输控制故障事件参数
    /// </summary>
    public class TransmissionControlFaultEventArgs : EventArgs
    {
        /// <summary>
        /// 传输结果
        /// </summary>
        public TransmissionControlResult Result;

        /// <summary>
        /// 注释
        /// </summary>
        public String Comment;

        /// <summary>
        /// 传输控制功能
        /// </summary>
        public TransmissionControlFaultEventArgs(string comment,TransmissionControlResult result)
        {
            Result = result;
            Comment = comment;
        }
    }
}
