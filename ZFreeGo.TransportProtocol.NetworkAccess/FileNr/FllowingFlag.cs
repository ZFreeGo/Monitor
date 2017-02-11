using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr
{
    /// <summary>
    /// 后续标志
    /// </summary>
    public enum FllowingFlag
    {
        /// <summary>
        /// 有后续
        /// </summary>
        Exist = 1, //有后续
        
        /// <summary>
        /// 无后续
        /// </summary>
        Nothing = 0,
    }
}
