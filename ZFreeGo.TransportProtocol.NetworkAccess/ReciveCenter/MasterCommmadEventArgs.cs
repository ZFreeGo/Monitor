using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZFreeGo.TransportProtocol.NetworkAccess.TransmissionControl104;

namespace ZFreeGo.TransportProtocol.NetworkAccess.ReciveCenter
{
    /// <summary>
    /// 主站系统命令事件
    /// </summary>
    public class MasterCommmadEventArgs : EventArgs
    {
        /// <summary>
        /// 命令
        /// </summary>
        public MasterCommand MasterCMD;


        /// <summary>
        /// 主站系统命令
        /// </summary>
        /// <param name="cmd">系统命令</param>
        public MasterCommmadEventArgs(MasterCommand cmd)
        {
             MasterCMD= cmd;

             
        }
        


    }
}
