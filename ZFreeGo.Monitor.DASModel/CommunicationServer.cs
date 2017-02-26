using GalaSoft.MvvmLight.Command;
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
        /// 定制网络连接参数
        /// </summary>
        private NetParameter _netCustomParameter;

        /// <summary>
        /// 获取定制网络参数
        /// </summary>
        public NetParameter NetCustomParameter
        {
            get
            {
                return _netCustomParameter;
            }

        }

        /// <summary>
        /// 定制网络TCp服务
        /// </summary>
        private NetClient _tcpCustomClient;

        /// <summary>
        /// 获取定制网络服务
        /// </summary>
        public NetClient NetCustomClient
        {
            get
            {
                return _tcpCustomClient;
            }
        }
        /// <summary>
        /// 通讯服务初始化
        /// </summary>      
        public CommunicationServer()
        {
            _netParameter = new NetParameter();
            _tcpClient = new NetClient();
            //定制服务的端口默认为8000
            _netCustomParameter = new NetParameter();
            _netCustomParameter.Port = 8000;
            _tcpCustomClient = new NetClient();
        }       
    }
}
