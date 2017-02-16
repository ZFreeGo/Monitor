using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ControlSystemCommand
{
    /// <summary>
    /// 召唤事件消息
    /// </summary>
    public class CallEventArgs : EventArgs
    {
        /// <summary>
        /// 注释
        /// </summary>
        public string Comment;

        /// <summary>
        /// 召唤服务结果
        /// </summary>
        public ControlSystemServerResut Result;

        /// <summary>
        /// 召唤事件初始化
        /// </summary>
        public CallEventArgs(string comment, ControlSystemServerResut result)
        {
            Comment = comment;
            Result = result;
        }
    }

  
}
