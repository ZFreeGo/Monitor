using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.BasicElement
{
    //    传送原因＝CAUSE OF TRANSMISSION∶＝ CP8{Cause,P/N,T}
    //其中 Cause∶＝UI6[1..6]<0..63>
    //<0>∶＝ 未定义
    //<1..63>∶＝传送原因序号
    //<1..47>∶＝本配套标准的标准定义(兼容范围)如下表
    //<48..63> ∶＝ 专用范围
    //P/N∶＝ BS1[7]<0..1>
    //<0>∶＝ 肯定确认
    //<1>∶＝否定确认
    //T＝test ∶＝ BS1[8]<0..1>
    //<0>∶＝未试验
    //<1>∶＝试验

    /// <summary>
    /// 传送原因掩码
    /// </summary>
    public enum CauseOfTransmissionMask : uint
    {
        /// <summary>
        /// 试验
        /// </summary>
        Test = 0x80,
        /// <summary>
        /// 未进行试验
        /// </summary>
        TestNo = 0x00,

        /// <summary>
        /// 肯定确认PositiveAcknowledgement 
        /// </summary>
        ACK = 0x00,
        /// <summary>
        /// 否定确认 Negative Acknowledgment
        /// </summary>
        NACK = 0x40,

        /// <summary>
        /// 原因掩码。只保留后四位
        /// </summary>
        CasueMask = 0x1F,

    }
}
