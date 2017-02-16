using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.TransmissionControl
{
    /// <summary>
    /// 传输控制事件参数
    /// </summary>
    public class TransmissionControlEventArgs : EventArgs
    {
        /// <summary>
        /// 传输控制功能
        /// </summary>
        public TransmissionCotrolFunction ControlFunction;

        /// <summary>
        /// 注释
        /// </summary>
        public String Comment;

        /// <summary>
        /// 传输控制功能
        /// </summary>
        public TransmissionControlEventArgs(TransmissionCotrolFunction controlFunction, string comment)
        {
            ControlFunction = controlFunction;
            Comment = comment;
        }
    }
}
