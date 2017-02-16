using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ControlProcessInformation
{
    /// <summary>
    /// 控制事件消息
    /// </summary>
    public class ControlEventArgs : EventArgs
    {
        /// <summary>
        /// 注释
        /// </summary>
        public string Comment;

        /// <summary>
        /// 召唤服务结果
        /// </summary>
        public ControlProcessServerResult Result;

        /// <summary>
        /// 召唤事件初始化
        /// </summary>
        public ControlEventArgs(string comment, ControlProcessServerResult result)
        {
            Comment = comment;
            Result = result;
        }
    }

  
}
