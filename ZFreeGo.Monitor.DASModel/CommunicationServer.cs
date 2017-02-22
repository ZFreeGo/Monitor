using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.Monitor.DASModel.DataItemSet;
using ZFreeGo.Net;


namespace ZFreeGo.Monitor.DASModel
{
    /// <summary>
    ///提供度串口，网络通讯等服务
    /// </summary>
    public class CommunicationServer
    {
        /// <summary>
        /// 网络连接参数
        /// </summary>
        private NetParameter _netParameter;

        /// <summary>
        /// 获取网络参数
        /// </summary>
        public NetParameter NetParameter
        {
            get
            {
                return _netParameter;
            }

        }

        /// <summary>
        /// 网络TCp服务
        /// </summary>
        private NetClient _tcpClient;

        /// <summary>
        /// 获取网络服务
        /// </summary>
        public NetClient NetClient
        {
            get
            {
                return _tcpClient;
            }
        }


        /// <summary>
        /// 通讯服务初始化
        /// </summary>      
        public CommunicationServer()
        {
            _netParameter = new NetParameter();
            _tcpClient = new NetClient();
        }       
    }
}
