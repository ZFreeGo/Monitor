using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.Xmodem
{
    public class XmodeServerEventArgs :EventArgs
    {
        public XmodeServerState ServerState;

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="arg">XmodeServer状态</param>
        public XmodeServerEventArgs(XmodeServerState arg)
        {
            ServerState = arg;
        }
    }
}
