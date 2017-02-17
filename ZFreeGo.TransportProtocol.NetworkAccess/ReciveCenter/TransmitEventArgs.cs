using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.ReciveCenter
{
    /// <summary>
    /// 用于帧解析传递事件
    /// </summary>
    public class TransmitEventArgs<T1, T2> :EventArgs
    {
        public T1 mdata1;
        public T2 mdata2;
        /// <summary>
        /// 事件消息
        /// </summary>
        /// <param name="data1">数据</param>
        public TransmitEventArgs(T1 data1, T2 data2)
        {
            mdata1 = data1;
            mdata2 = data2;
        }
        public TransmitEventArgs(T1 data1)
        {
            mdata1 = data1;
            mdata2 = default(T2);
        }

    }
}
