using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr
{
    public enum FileCallFlag 
    {
        /// <summary>
        /// 目录下所有文件
        /// </summary>
        All = 0,

        /// <summary>
        /// 满足搜索时间段
        /// </summary>
        MeetTime = 1,
    }
}
