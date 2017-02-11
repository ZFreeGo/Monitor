using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr
{
    /// <summary>
    /// 写文件结果描述
    /// </summary>
    public enum WriteFileResultDescription
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
        /// 不支持的文件名
        /// </summary>
        UnsupportName = 2,
        /// <summary>
        /// 文件尺寸超出范围
        /// </summary>
        OverMaxLen = 3,


    }
}
