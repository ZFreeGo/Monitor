using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.ReciveCenter
{
    public enum MessageFormat : ushort
    {
        Null = 0,

        /// <summary>
        /// I格式,编号的信息传输格式
        /// </summary>
        InformationTransmitFormat = 1,

        /// <summary>
        /// S 格式,编号的监视功能格式
        /// </summary>
        NumberedSupervisoryFunctions = 2,

        /// <summary>
        /// U 格式,不编号的控制功能格式
        /// </summary>
        UnnumberedControlFunction = 3,

    }
}
