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
using ZFreeGo.Monitor.AutoStudio.Log;
using ZFreeGo.Monitor.AutoStudio.Secure;

namespace ZFreeGo.Monitor.AutoStudio.OptionConfig.AccountUI
{
    /// <summary>
    /// DeleteAccount.xaml 的交互逻辑 ---未采用消息传递机制
    /// </summary>
    public partial class DeleteAccount : Page
    {
        /// <summary>
        /// 日志产生事件
        /// </summary>
        public event EventHandler<Log.LogEventArgs> MakeLogEvent;


        private AccountManager accountManager;
        public DeleteAccount()
        {
            InitializeComponent();
        }

        public void UpdateShow(AccountManager inAccountManager)
        {
            accountManager = inAccountManager;
            combUserList.Items.Clear();
            foreach( var  m in accountManager.AccountList)
            {
                combUserList.Items.Add(m.UserName);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateShow(accountManager);
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var account =  accountManager.AccountList[combUserList.SelectedIndex];
                var state = accountManager.DeleteAccount( account);
                if (state)
                {
                    UpdateShow(accountManager);
                    string str = string.Format("删除用户名\"{0}\".", account.UserName);
                    MakeLogMessage(sender, "删除用户", str, LogType.Account);
                    MessageBox.Show("成功删除用户", "删除用户");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "删除账户");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

    
        /// <summary>
        /// 产生日志消息
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="content">操作内容</param>
        /// <param name="result">操作结果</param>
        private void MakeLogMessage(object sender, string content, string result, LogType type)
        {
            try
            {
                if (MakeLogEvent != null)
                {
                    var message = new SingleLogMessage(accountManager.LoginAccount.UserName, content, result, type);
                    MakeLogEvent(sender, new Log.LogEventArgs(message));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "日志消息");
            }
        }
    }
}
