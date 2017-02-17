using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ZFreeGo.TransportProtocol.NetworkAccess.TransmissionControl104
{
    /// <summary>
    /// 传输控制事件参数
    /// </summary>
    public class TransmissionControlEventArgs : EventArgs
    {
        /// <summary>
        /// 传输控制功能
        /// </summary>
        public TransmissionControlFunction ControlFunction;

        /// <summary>
        /// 注释
        /// </summary>
        public String Comment;

        /// <summary>
        /// 传输控制功能
        /// </summary>
        public TransmissionControlEventArgs(TransmissionControlFunction controlFunction, string comment)
        {
            ControlFunction = controlFunction;
            Comment = comment;
        }
    }
}
