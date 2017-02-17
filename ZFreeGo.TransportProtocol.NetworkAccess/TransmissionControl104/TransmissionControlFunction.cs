using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.TransmissionControl104
{
    /// <summary>
    /// 传输控制功能
    /// </summary>
    public enum TransmissionControlFunction : byte
    {
        /// <summary>
        /// 启动数据传输 STARTDT 命令
        /// </summary>
        StartDataTransmission = 0x07,
        /// <summary>
        /// 停止数据传输 STOPDT 命令
        /// </summary>
        StopDataTransmission = 0x13,
        /// <summary>
        /// 测试过程 TESTFR 命令
        /// </summary>
        TestFrame =  0x43,
        /// <summary>
        /// 启动数据传输 STARTDT 确认
        /// </summary>
        AcknowledgementStartDataTransmission = 0x0B,
        /// <summary>
        /// 停止数据传输 STOPDT 确认
        /// </summary>
        AcknowledgementStopDataTransmission = 0x23,
        /// <summary>
        /// 测试过程 TESTFR 确认
        /// </summary>
        AcknowledgementTestFrame = 0x83

    }
}
