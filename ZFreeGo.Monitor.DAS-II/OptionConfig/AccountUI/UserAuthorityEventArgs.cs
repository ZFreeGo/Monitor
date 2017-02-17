using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.OptionConfig.AccountUI
{
    public class UserAuthorityEventArgs : EventArgs
    {

        /// <summary>
        /// 用户权限变更事件
        /// </summary>
        /// <param name="authority">用户权限</param>
        public UserAuthorityEventArgs(ZFreeGo.Monitor.AutoStudio.Secure.AuthorityLevel authority)
        {
            Authority = authority;
        }
        public ZFreeGo.Monitor.AutoStudio.Secure.AuthorityLevel Authority;


    }
}
