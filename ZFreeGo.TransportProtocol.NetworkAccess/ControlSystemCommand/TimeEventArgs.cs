using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ControlSystemCommand
{
    /// <summary>
    /// 时钟同步服务
    /// </summary>
    public class TimeEventArgs : EventArgs
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
        /// 事件同步命令初始化
        /// </summary>
        public TimeEventArgs (string comment, ControlSystemServerResut result)
        {
            Comment = comment;
            Result = result;
        }
    }

    
}
