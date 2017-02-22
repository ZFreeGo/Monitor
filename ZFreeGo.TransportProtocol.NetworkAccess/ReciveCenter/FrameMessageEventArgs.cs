using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.ReciveCenter
{
    /// <summary>
    /// 接收或发送帧信息事件
    /// </summary>
    public class FrameMessageEventArgs : EventArgs
    {
        /// <summary>
        /// 注释帧信息
        /// </summary>
        public string Comment;

        /// <summary>
        /// 原始帧信息
        /// </summary>
        public string RawMessage;

        /// <summary>
        /// 帧信息事件参数
        /// </summary>       
        /// <param name="rawMessage">原始帧信息</param>
        /// <param name="comment">注释</param>
        public FrameMessageEventArgs( string rawMessage,string comment)
        {
            Comment = comment;
            RawMessage = rawMessage;
        }

    }
}
