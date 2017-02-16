using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ControlSystemCommand
{
    /// <summary>
    /// 复位消息
    /// </summary>
    public class ResetProcessArgs : EventArgs
    {
        /// <summary>
        /// 注释
        /// </summary>
        public string Comment;

        /// <summary>
        /// 服务结果
        /// </summary>
        public ControlSystemServerResut Result;

        /// <summary>
        /// 复位消息事件初始化
        /// </summary>
        public ResetProcessArgs(string comment, ControlSystemServerResut result)
        {
            Comment = comment;
            Result = result;
        }
    }
}
