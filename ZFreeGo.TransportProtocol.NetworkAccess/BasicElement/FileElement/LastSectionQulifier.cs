using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.BasicElement
{
    /// <summary>
    /// 最后的节和段的限定词LSQ
    /// </summary>
    public enum  LastSectionQulifier : byte
    {
        /// <summary>
        /// 未使用
        /// </summary>
        NoUsed = 0,
        /// <summary>
        /// 不带停止激活的文件传输
        /// </summary>
        NoStopActivedFileTransmit = 1,
        /// <summary>
        /// 带停止激活的文件传输
        /// </summary>
        WithStopActivedFileTransmit = 2,
        /// <summary>
        /// 不带停止激活的节传输
        /// </summary>
        NoStopActivedSectionTransmit = 3,
        /// <summary>
        /// 带停止激活的节传输
        /// </summary>
        WithStopActivedSectionTransmit = 4,
    }
}
