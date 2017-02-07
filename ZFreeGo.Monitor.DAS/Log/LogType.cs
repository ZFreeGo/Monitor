using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.Log
{
    /// <summary>
    /// 日志消息类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 空
        /// </summary>
        Null = 0,
        /// <summary>
        /// 登陆信息
        /// </summary>
        Login = 1,
        /// <summary>
        /// 遥控
        /// </summary>
        Telecontrol = 2,

        /// <summary>
        /// 网络
        /// </summary>
        Net = 0x10,

        
    }
}
