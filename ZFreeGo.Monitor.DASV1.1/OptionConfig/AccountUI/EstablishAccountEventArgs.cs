using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.OptionConfig.AccountUI
{
    public class EstablishAccountEventArgs : EventArgs
    {


        /// <summary>
        /// 新建用户
        /// </summary>
        /// <param name="newUserName">输入的新用户名</param>
         /// <param name="newPass">输入的新密码</param>
        /// <param name="newPassRepeat">输入的新密码重复</param>
        /// <param name=" newAuthoritys">输入的新权限</param>
        public EstablishAccountEventArgs(string newUserName, SecureString newPass, SecureString newPassRepeat,
            ZFreeGo.Monitor.AutoStudio.Secure.AuthorityLevel newAuthority)
        {
            UserName = newUserName;
            Authority = newAuthority;
            UserNewPassword = newPass;
            UserNewPasswordRepeat = newPassRepeat;
        }
        
     
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName;
        /// <summary>
        /// 输入的新密码
        /// </summary>
        public SecureString UserNewPassword;
        /// <summary>
        /// 输入的新密码， 重复
        /// </summary>
        public SecureString UserNewPasswordRepeat;
        /// <summary>
        /// 用户权限
        /// </summary>
        public ZFreeGo.Monitor.AutoStudio.Secure.AuthorityLevel Authority;
    }
}
