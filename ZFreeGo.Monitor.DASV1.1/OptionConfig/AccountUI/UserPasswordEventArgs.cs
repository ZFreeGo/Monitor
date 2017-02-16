using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.OptionConfig.AccountUI
{
    public class UserPasswordEventArgs : EventArgs
    {


        /// <summary>
        /// 变更密码
        /// </summary>
        /// <param name="oldPass">输入的旧密码</param>
        /// <param name="newPass">输入的新密码</param>
        /// <param name="newPassRepeat">输入的新密码重复</param>
        public UserPasswordEventArgs(SecureString oldPass, SecureString newPass, SecureString newPassRepeat)
        {
            UserOldPassword = oldPass;
            UserNewPassword = newPass;
            UserNewPasswordRepeat = newPassRepeat;
        }
        
        /// <summary>
        /// 输入的旧密码
        /// </summary>
        public SecureString UserOldPassword;
        /// <summary>
        /// 输入的新密码
        /// </summary>
        public SecureString UserNewPassword;
        /// <summary>
        /// 输入的新密码， 重复
        /// </summary>
        public SecureString UserNewPasswordRepeat;
    }
}
