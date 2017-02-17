using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.BasicElement
{
    /// <summary>
    /// 文件准备就绪限定词(FRQ)
    /// </summary>
    public class FileReadyQulifier
    {
        /// <summary>
        /// 选择，请求，停止激活或删除的肯定确认
        /// </summary>
        public const byte ACK = 0;
        /// <summary>
        /// 选择，请求，停止激活或删除的否定确认
        /// </summary>
        public const byte NonACK = 0x80;

        /// <summary>
        /// 文件准备就绪限定词
        /// </summary>
        public byte FRQ;
        public FileReadyQulifier(byte frq)
        {
            FRQ = frq;            
        }
    }
}
