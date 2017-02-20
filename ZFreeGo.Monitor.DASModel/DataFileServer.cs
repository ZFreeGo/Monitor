﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.Monitor.DASModel.GetViewData;

namespace ZFreeGo.Monitor.DASModel
{
    /// <summary>
    /// 提供磁盘文件，数据库文件读写管理服务
    /// </summary>
    public class DataFileServer
    {
        /// <summary>
        /// 监控数据，三遥等
        /// </summary>
        private MonitorViewData _monitorViewData;

        /// <summary>
        /// 获取监控数据
        /// </summary>
        public MonitorViewData MonitorData
        {
            get
            {
                return _monitorViewData;
            }
        }

       
        /// <summary>
        /// 数据服务初始化
        /// </summary>
        public DataFileServer()
        {
            _monitorViewData = new MonitorViewData();
        }


    }
}