using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.ReciveCenter
{
    /// <summary>
    /// 故障信息事件参数
    /// </summary>
    public class ProtocolServerFaultArgs : EventArgs
    {
        /// <summary>
        /// 注释
        /// </summary>
        public String Comment;

        /// <summary>
        /// 接收故障
        /// </summary>
        /// <param name="comment"></param>
        public ProtocolServerFaultArgs(string comment)
        {
            Comment = comment;
        }
    }
}
