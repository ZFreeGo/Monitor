using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Net.Element
{
    public class NetActionEventArgs : EventArgs
    {
        public NetActionEventArgs(NetBasicAction act, string des)
        {
            NetAction = act; 
            DataMsg = des;
        }
        public NetBasicAction NetAction;

        public string DataMsg;
    }
}
