using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.Xmodem
{
    /// <summary>
    /// Xmode 起始字符
    /// </summary>
    public enum XmodeStartHeader
    {

        /// <summary>
        ///StartOfHearder Xmode 数据头128字节 0x01
        /// </summary>
        SOH = 0x01,
        /// <summary>
        ///StartOfText Xmode-1K 数据头1024 字节 0x02
        /// </summary>
        STX = 0x02,
    }


    /// <summary>
    /// Xmode 字符定义
    /// </summary>
    public enum XmodeDefine
    {


        /// <summary>
        ///EndOfTransmission  传输结束 0x04
        /// </summary>
        EOT = 0x04,

        /// <summary>
        /// ACK 认可响应 0x06
        /// </summary>
        ACK = 0x06,

        /// <summary>
        /// 不认可响应
        /// </summary>
        NAK = 0x15,

        /// <summary>
        ///Cancel 撤销传输 0x18
        /// </summary>
        CAN = 0x18,

        /// <summary>
        /// 填充数据包 0x1A
        /// </summary>
        EOF = 0x1A,

        /// <summary>
        /// 'C' 请求指令
        /// </summary>
        C = 'C',
        /// <summary>
        /// 'c' 请求指令
        /// </summary>
        c = 'c'
    }
}
