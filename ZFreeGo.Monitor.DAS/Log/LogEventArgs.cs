using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.Log
{
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// 单条事件消息
        /// </summary>
        public SingleLogMessage Message;


        /// <summary>
        /// 日志产生消息
        /// </summary>
        /// <param name="message"></param>
        public LogEventArgs(SingleLogMessage message)
        {
            Message = message;
        }
    }
}
