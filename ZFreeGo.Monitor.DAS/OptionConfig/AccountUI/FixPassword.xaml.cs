using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZFreeGo.Monitor.AutoStudio.OptionConfig.AccountUI
{
    /// <summary>
    /// FixPassword.xaml 的交互逻辑
    /// </summary>
    public partial class FixPassword : Page
    {
        /// <summary>
        /// 用户密码更改消息
        /// </summary>
        public event EventHandler<UserPasswordEventArgs> UserPasswordChanged;


        public FixPassword()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 更新显示
        /// </summary>
        /// <param name="userName">对应的用户名称</param>
        /// <param name="oldPass">存储的旧密码</param>
        public void UpdateShow(string userName)
        {
            currentUserName.Text = userName;
          }

        /// <summary>
        /// 取消修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            passBoxOld.SecurePassword.Clear();
            passBoxNew.SecurePassword.Clear();
            passBoxNewRepeat.SecurePassword.Clear();



        }
        /// <summary>
        /// 验证密码，发送修改密码消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFixPassword_Click(object sender, RoutedEventArgs e)
        {
            if (UserPasswordChanged != null)
            {
                UserPasswordChanged(this,
                    new UserPasswordEventArgs(passBoxOld.SecurePassword,
                        passBoxNew.SecurePassword, passBoxNewRepeat.SecurePassword));
            }

           
            
        }

        


    }
}
