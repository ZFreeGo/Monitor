using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.FileSever
{
    /// <summary>
    /// 写文件传输描述
    /// </summary>
    public enum FileTransmitDescription
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 未知错误
        /// </summary>
        UnknowError = 1,
        /// <summary>
        /// 校验和错误
        /// </summary>
        CheckError = 2,
        /// <summary>
        /// 不同的文件长度
        /// </summary>
        DifferentFileLen = 3,

        /// <summary>
        /// 文件ID与激活ID不一致，
        /// </summary>
        DifferentID = 4,
    }
}
