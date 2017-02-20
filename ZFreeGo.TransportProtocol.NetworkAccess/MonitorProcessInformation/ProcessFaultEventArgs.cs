using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.MonitorProcessInformation
{
    /// <summary>
    /// 过程处理故障信息
    /// </summary>
    public class ProcessFaultEventArgs : EventArgs
    {
        /// <summary>
        /// 故障信息
        /// </summary>
        public Exception EX;

        public String Comment;

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="comment"></param>
        public ProcessFaultEventArgs(Exception ex, string comment)
        {
            EX = ex;
            Comment = comment;
        }
    }
}
