using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.FileSever
{
    /// <summary>
    /// 召唤文件目录结束事件参数
    /// </summary>
    public class CallFileEndEventArgs : EventArgs
    {
        /// <summary>
        /// 描述
        /// </summary>
        public String Message;

        /// <summary>
        /// 读文件属性列表
        /// </summary>
        public List<FileAttribute> AttributeList;

        /// <summary>
        /// 召唤文件完成时间
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="atrributeList">目录文件属性列表</param>
        public CallFileEndEventArgs(string message,  List<FileAttribute> atrributeList)
        {
            Message = message;
            AttributeList = atrributeList;
        }




    }
}
