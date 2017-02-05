using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// FixUserName.xaml 的交互逻辑
    /// </summary>
    public partial class FixUserName : Page
    {
      /// <summary>
      /// 用户名更改消息
      /// </summary>
        public event EventHandler<UserNameEventArgs> UserNameChanged;

        public FixUserName()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 更新用户名的显示
        /// </summary>
        public void UpdateShow(string userName)
        {
            currentUserName.Text = userName;
        }

        /// <summary>
        /// 验证用户名称，发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFixUserName_Click(object sender, RoutedEventArgs e)
        {
            if (checkUserName(txtNewUserName.Text))
            {
                if (UserNameChanged != null)
                {
                    UserNameChanged(this, new UserNameEventArgs(txtNewUserName.Text.Trim()));
                }
            }
        }
        /// <summary>
        /// 取消修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            txtNewUserName.Text = "";
        }

        /// <summary>
        /// 检测用户名是否符合规范
        /// </summary>
        /// <returns>检测通过</returns>
        private bool checkUserName(string  str)
        {
            //检测是否为空
            //检测是否满足基本要求
            //检测是与已有的是否重复
            return true;
        }
    }
}
