using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio
{
    public enum LinkNetMode : int
    {
        UPNPServer = 0xA1,
        TcpServer = 0xA5,
        TcpClient = 0xAA,
    }
}
