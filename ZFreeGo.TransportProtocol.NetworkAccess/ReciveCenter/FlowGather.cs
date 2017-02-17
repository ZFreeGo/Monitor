using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.ReciveCenter
{
    /// <summary>
    /// 控制流程合集，按照规划顺序添加
    /// </summary>
    public enum FlowGather : int
    {
        /// <summary>
        /// TCP连接
        /// </summary>
        TcpStartLink    = 1,
        /// <summary>
        /// Tcp连接建立完毕
        /// </summary>
        TcpLinkACK   = 2,
        /// <summary>
        /// 启动数据传输StartDataTransmission
        /// </summary>
        StartDataTransmission = 3,
        /// <summary>
        /// 数据传输确认
        /// </summary>
        StartDataTransmissionACK = 4,

        /// <summary>
        /// 主召唤
        /// </summary>
        MasterInterrogation = 5,

        /// <summary>
        /// 主召唤确认
        /// </summary>
        MasterInterrogationACK = 6,

        /// <summary>
        /// 时钟同步
        /// </summary>
        TimeSynchronization = 7,

        /// <summary>
        /// 时钟同步 确认
        /// </summary>
        TimeSynchronizationACK = 8,



        /// <summary>
        /// 空闲
        /// </summary>
        Leisure = 1000,

    }
}
