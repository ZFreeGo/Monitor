using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.BasicElement
{

    /// <summary>
    /// 初始化原因 CauseOfInitialization COI
    /// </summary>
    public enum CauseOfInitialization : byte
    {
        /// <summary>
        /// 当地电源合上
        /// </summary>
        LocalClosePower = 0,

        /// <summary>
        /// 当地手动复位
        /// </summary>
        LocalManualReset = 1,

        /// <summary>
        /// 远方复位
        /// </summary>
        DistantReset = 2


    }
}
