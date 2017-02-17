using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.ControlSystemCommand
{
    /// <summary>
    /// 控制系统命令服务结果
    /// </summary>
    public enum ControlSystemServerResut
    {
        /// <summary>
        /// 解析错误
        /// </summary>
        Error = 0,

        /// <summary>
        /// 超时,重新召唤
        /// </summary>
        OverTime = 1,

        /// <summary>
        /// 激活确认
        /// </summary>
        AcvtivityAck = 2,

        /// <summary>
        /// 激活确认
        /// </summary>
        ActivateTermination = 3,

        /// <summary>
        /// 失败
        /// </summary>
        Fault = 4,

        /// <summary>
        /// 发送失败
        /// </summary>
        SendFault = 5,
        /// <summary>
        /// 未识别
        /// </summary>
        Unknow = 255,

    }
}
