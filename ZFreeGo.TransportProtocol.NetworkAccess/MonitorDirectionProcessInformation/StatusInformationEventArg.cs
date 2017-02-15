using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess
{
    /// <summary>
    /// 状态信息参数，包含原始帧与提取后的参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StatusInformationEventArg<T> : EventArgs
    {

        public APDU Apdu;

        public T Message;

        public StatusInformationEventArg(T message, APDU apdu)
        {
            Message = message;
            Apdu = apdu;
        }
    }
}
