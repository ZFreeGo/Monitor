using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.Xmodem
{
    public enum XmodeServerState
    {
        /// <summary>
        /// 取消传输
        /// </summary>
        Cancel = 1,
        /// <summary>
        /// 超时
        /// </summary>
        OverTime = 2,
        /// <summary>
        /// 成功
        /// </summary>
        Sucess = 3,
        /// <summary>
        /// 传输失败
        /// </summary>
        Failue = 4,
    }
}
