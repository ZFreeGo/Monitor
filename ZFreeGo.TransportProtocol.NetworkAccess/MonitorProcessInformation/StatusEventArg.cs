using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess.MonitorProcessInformation
{
    /// <summary>
    /// 状态信息参数，提取后的参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StatusEventArgs<T> : EventArgs
    {

        public T Message;

        public StatusEventArgs(T message)
        {
            Message = message;
        }
    }
}
