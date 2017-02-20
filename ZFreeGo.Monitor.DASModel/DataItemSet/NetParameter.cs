using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Net;

namespace ZFreeGo.Monitor.DASModel.DataItemSet
{
    /// <summary>
    /// 通讯参数
    /// </summary>
    public class NetParameter : ObservableObject
    {
        IPAddress ip;

        /// <summary>
        /// 获取IP
        /// </summary>
        public IPAddress IP
        {
            get
            {
                return ip;
            }
        }
        /// <summary>
        /// 获取或设置IP地址
        /// </summary>
        public string IpAddress
        {
            get { return ip.ToString(); }
            set
            {
                IPAddress iptry;
                if(IPAddress.TryParse(value,out  iptry))
                {
                    ip = iptry;
                    RaisePropertyChanged("IpAddress");
                    
                }
                
               
            }
        }

        int port;

        /// <summary>
        /// 获取或设置端口号
        /// </summary>
        public int  Port
        {
            get { return port; }
            set
            {
                port = value;
                RaisePropertyChanged("Port");
               
            }
        }

        private string linkMessage;

        /// <summary>
        /// 连接信息
        /// </summary>
        public string LinkMessage
        {
            get
            {
                return linkMessage;
            }
            set
            {
                linkMessage = value;
                RaisePropertyChanged("LinkMessage");
               
            }
        }

        private bool netStartEnable;

        /// <summary>
        /// 设置或获取网络使能状态，true-可以启动网络连接，false--禁止连接
        /// </summary>
        public bool NetStartEnable
        {
            set
            {
                netStartEnable = value;
                RaisePropertyChanged("StartNetEnable");
                RaisePropertyChanged("StopNetEnable");
                RaisePropertyChanged("NetEnable");
            }
            get
            {
                return netStartEnable;
            }
        }
        /// <summary>
        /// 获取启动网络使能
        /// </summary>
        public bool StartNetEnable
        {
            get
            {
                return netStartEnable;
            }
            
        }

        /// <summary>
        /// 获取停止网络使能
        /// </summary>
        public bool StopNetEnable
        {
            get
            {
                return !netStartEnable;
            }

           
        }

        /// <summary>
        /// 通讯参数初始化
        /// </summary>
        public NetParameter()
        {
            IPAddress.TryParse("192.168.60.100", out ip);
            port = 2404;
            linkMessage = "启动。\n";
            netStartEnable = true;

        }


    }
}
