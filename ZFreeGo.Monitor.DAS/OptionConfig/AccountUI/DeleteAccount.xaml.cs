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
using ZFreeGo.Monitor.AutoStudio.Secure;

namespace ZFreeGo.Monitor.AutoStudio.OptionConfig.AccountUI
{
    /// <summary>
    /// DeleteAccount.xaml 的交互逻辑 ---未采用消息传递机制
    /// </summary>
    public partial class DeleteAccount : Page
    {
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
                var state = accountManager.DeleteAccount(accountManager.AccountList[combUserList.SelectedIndex]);
                if (state)
                {
                    UpdateShow(accountManager);
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
    }
}
