using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.Xmodem
{
    public enum ServerStep
    {
        /// <summary>
        /// 等待启动传输
        /// </summary>
        WaitStartTransmision = 1,

        /// <summary>
        /// 传输数据
        /// </summary>
        TransmisionData = 2,
    }
}
