using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.CommCenter
{
    /// <summary>
    /// 校准系数行为
    /// </summary>
    public enum CalibrationAction : byte
    {
        /// <summary>
        /// 空
        /// </summary>
        Null = 0,
        /// <summary>
        /// 系数召唤
        /// </summary>
        FactorCall = 1,
        /// <summary>
        /// 系数下载
        /// </summary>
        FactorDownload = 2,
        /// <summary>
        /// 系数固化
        /// </summary>
        FactorFix = 3,
        /// <summary>
        /// 录波数据
        /// </summary>
        RecordWaveform = 4,
    }
}
