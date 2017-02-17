using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.BasicElement
{
    /// <summary>
    /// 复位进程限定词
    /// </summary>
    public enum QualifyResetProgressList : byte
    {
        /// <summary>
        /// 未使用
        /// </summary>
        NotUsed = 0,

        /// <summary>
        /// 进程的总复位
        /// </summary>
        AllProgressReset = 1,

        /// <summary>
        /// 复位事件缓冲区等待处理的带时标信息
        /// </summary>
        ResetBufferPendingMessageWithTimestamp = 2,
    }
}
