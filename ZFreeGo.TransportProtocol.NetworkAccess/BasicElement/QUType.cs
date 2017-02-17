using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.BasicElement
{
    /// <summary>
    /// 命令限定词，代号属性
    /// </summary>
    public enum QUtype : byte
    {
        /// <summary>
        /// 未定义
        /// </summary>
        NODefine = 0,
        /// <summary>
        /// 短脉冲持续时间
        /// </summary>
        ShortPusle = 1,
        /// <summary>
        /// 长脉冲持续时间
        /// </summary>
        LongPulse = 2,
        /// <summary>
        /// 持续时间
        /// </summary>
        ContinuousOutput = 3,
    }
}
