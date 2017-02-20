using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocols.BasicElement;


namespace ZFreeGo.TransmissionProtocols.MonitorProcessInformation
{
    /// <summary>
    /// 状态信息参数，提取后的参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StatusEventArgs<T> : EventArgs
    {

        public T Message;

        public TypeIdentification ID;

        public StatusEventArgs(T message, TypeIdentification id)
        {
            Message = message;
            ID = id;
        }
    }
}
