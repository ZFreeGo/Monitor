using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.OptionConfig.AccountUI
{
    public class UserNameEventArgs : EventArgs
    {

        /// <summary>
        /// 用户名称变更事件
        /// </summary>
        /// <param name="name">用户名称</param>
        public UserNameEventArgs(string name)
        {
            UserNameStr = name;
        }
        public string UserNameStr;

     
    }
}
