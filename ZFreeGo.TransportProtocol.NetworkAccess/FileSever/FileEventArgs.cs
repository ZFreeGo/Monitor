using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.FileSever
{
    /// <summary>
    /// 控制事件消息
    /// </summary>
    public class FileEventArgs : EventArgs
    {
        /// <summary>
        /// 注释
        /// </summary>
        public string Comment;

        /// <summary>
        /// 召唤服务结果
        /// </summary>
        public FileServerResut Result;

        /// <summary>
        /// 异常
        /// </summary>
        public Exception EX;
        /// <summary>
        /// 文件服务过程信息
        /// </summary>
        public FileEventArgs(string comment, FileServerResut result)
        {
            Comment = comment;
            Result = result;
        }
        /// <summary>
        /// 文件服务过程信息
        /// </summary>
        public FileEventArgs(string comment, FileServerResut result, Exception ex)
        {
            Comment = comment;
            Result = result;
            ex = EX;
        }
    }

  
}
