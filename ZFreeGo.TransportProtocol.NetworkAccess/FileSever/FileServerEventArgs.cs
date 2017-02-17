using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.FileSever
{
    /// <summary>
    /// 文件服务事件
    /// </summary>
    public class FileServerEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Message;
        /// <summary>
        /// 操作标识
        /// </summary>
        public OperatSign Operation;

        /// <summary>
        /// 完整数据包
        /// </summary>
        public FilePacket Packet;

        /// <summary>
        /// 附加数据包
        /// </summary>
        public T AdditionalPacket;



        /// <summary>
        /// 文件服务时间
        /// </summary>
        /// <param name="message">文件消息</param>
        /// <param name="operation">操作标识</param>
        /// <param name="packet">数据包</param>
        /// <param name="additionalPacket">附加数据包</param>
        public FileServerEventArgs(string message, OperatSign operation, FilePacket packet, T additionalPacket)
        {
            Message = message;
            Operation = operation;
            Packet = packet;
            AdditionalPacket = additionalPacket;
        }


    }
}
