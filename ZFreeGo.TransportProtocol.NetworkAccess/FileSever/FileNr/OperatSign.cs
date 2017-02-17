using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.FileSever
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

        /// <summary>
        /// 写文件激活
        /// </summary>
        WriteFileActivity = 7,

        /// <summary>
        /// 写文件
        /// </summary>
        WriteFileActivityAck = 8,

        /// <summary>
        /// 写文件数据
        /// </summary>
        WriteFileThransmit = 9,

        /// <summary>
        /// 写文件传输确认
        /// </summary>
        WriteFileThransmitAck = 10,
    }
}
