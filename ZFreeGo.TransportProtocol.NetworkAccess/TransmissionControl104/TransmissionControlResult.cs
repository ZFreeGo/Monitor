using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.TransmissionControl104
{
   
        /// <summary>
        /// 传输控制结果
        /// </summary>
        public enum TransmissionControlResult
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
            /// 未知ID
            /// </summary>
            UnkowID = 2,

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
