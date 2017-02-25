using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.DASModel.DataItemSet
{
    public enum LogTypeNum
    {
        /// <summary>
        /// 终端重启记录 行为类
        /// </summary>
        TerminalRestart = 1,
        /// <summary>
        /// 通道连接建立与断开记录 状态类
        /// </summary>
        ChannelConnectBreak = 2,

        /// <summary>
        /// 通信过程异常记录 状态类
        /// </summary>
        CommunicationException = 3,
        /// <summary>
        /// 装置内部各类插件、 元件异常自检记录 状态类
        /// </summary>
        HardwareException = 4,
        /// <summary>
        /// 装置内部软件进程异常记录 状态类
        /// </summary>
        SoftwareException = 5,
        /// <summary>
        /// 主电源通断及电压异常记录 状态类
        /// </summary>
        MainPowerException = 6,
        /// <summary>
        ///备用电源通断、 活化及电压异常记录 状态 
        /// </summary>
        BackpuPowerException = 7,
        /// <summary>
        /// 控制回路断线异常记录 状态类
        /// </summary>
        ControlCircleBreakline = 8,
        /// <summary>
        /// 开关位置异常记录 状态类
        /// </summary>
        SwitchPositionException = 9,
        /// <summary>
        /// 终端参数修改记录 行为类
        /// </summary>
        TerminalParammer = 10,
        /// <summary>
        /// 软件版本升级记录 行为类
        /// </summary>
        VersionUpdateLog = 11
    }
}
