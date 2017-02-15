﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ControlSystemCommand
{
    /// <summary>
    /// 召唤事件消息
    /// </summary>
    public class CallEventArgs : EventArgs
    {
        /// <summary>
        /// 注释
        /// </summary>
        public string Comment;

        /// <summary>
        /// 召唤服务结果
        /// </summary>
        public CallServerResut Result;

        /// <summary>
        /// 召唤事件初始化
        /// </summary>
        public CallEventArgs(string comment, CallServerResut result)
        {
            Comment = comment;
            Result = result;
        }
    }

    /// <summary>
    /// 召唤结果
    /// </summary>
    public enum CallServerResut
    {
        /// <summary>
        /// 解析错误
        /// </summary>
        Error = 0,

        /// <summary>
        /// 召唤超时,重新召唤
        /// </summary>
        OverTime = 1,

        /// <summary>
        /// 召唤激活确认确认
        /// </summary>
        AcvtivityAck = 2,

        /// <summary>
        /// 召唤激活确认
        /// </summary>
        ActivateTermination = 3,

        /// <summary>
        /// 召唤失败
        /// </summary>
        Fault = 4,


        /// <summary>
        /// 未识别
        /// </summary>
        Unknow = 255,
        
    }
}
