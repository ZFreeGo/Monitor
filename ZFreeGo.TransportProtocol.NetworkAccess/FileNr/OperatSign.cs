using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr
{
    /// <summary>
    /// 文件操作标识
    /// </summary>
    public enum OperatSign
    {
        /// <summary>
        /// 读文件目录
        /// </summary>
        ReadDirectory = 1,
        /// <summary>
        /// 读文件目录确认
        /// </summary>
        ReadDirectoryACK = 2,
        /// <summary>
        /// 读文件激活
        /// </summary>
        ReadFileActivity = 3,
        /// <summary>
        /// 读文件激活确认
        /// </summary>
        ReadFileActivityACK = 4,

        /// <summary>
        /// 读文件数据响应
        /// </summary>
        ReadFileDataResponse = 5,

        /// <summary>
        /// 读文件数据响应确认
        /// </summary>
        ReadFileDataResponseACK = 6,
    }
}
