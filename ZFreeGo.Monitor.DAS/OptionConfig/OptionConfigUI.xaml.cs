using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
using ZFreeGo.Monitor.AutoStudio.OptionConfig.AccountUI;
using ZFreeGo.Monitor.AutoStudio.Secure;


namespace ZFreeGo.Monitor.AutoStudio.OptionConfig
{
    /// <summary>
    /// OptionConfigUI.xaml 的交互逻辑
    /// </summary>
    public partial class OptionConfigUI : Window
    {
        /// <summary>
        /// 账户管理器
        /// </summary>
        private AccountManager accountManager;

        /// <summary>
        /// 修改账户权限界面
        /// </summary>
        private FixAuthority fixAuthorityPage;

        /// <summary>
        /// 修改用户名称
        /// </summary>
        private FixUserName fixUserNamePage;
        public OptionConfigUI(AccountManager inAccountManager)
        {
            InitializeComponent();

            accountManager = inAccountManager;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (accountManager.LoginAccount.PowerLevel < 3)
            {
                treeManageAccount.IsExpanded = false;
                treeManageAccount.IsEnabled = false;
            }
           
        }
  




        /// <summary>
        /// 修改账户类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fixAuthority_Selected(object sender, RoutedEventArgs e)
        {
             fixAuthorityPage = new FixAuthority();
            fixAuthorityPage.UpdateShow(accountManager.LoginAccount.UserName,
                (Secure.AuthorityLevel)accountManager.LoginAccount.PowerLevel);

            fixAuthorityPage.UserAuthorityChanged += fixAuthorityPage_UserAuthorityChanged;

            framewPlane.NavigationService.Navigate(fixAuthorityPage);
        }

        /// <summary>
        /// 权限变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fixAuthorityPage_UserAuthorityChanged(object sender, UserAuthorityEventArgs e)
        {
            try
            {
                var state = accountManager.UpdateAccountAuthority(accountManager.LoginAccount.UserName,
                    (int)e.Authority);
                if (state)
                {
                    fixAuthorityPage.UpdateShow(accountManager.LoginAccount.UserName,
               (Secure.AuthorityLevel)accountManager.LoginAccount.PowerLevel);

                    accountManager.SaveAccountInformation();
                    MessageBox.Show("修改权限成功。", "修改权限");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "修改用户权限");
            }
        }

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fixUserName_Selected(object sender, RoutedEventArgs e)
        {
            fixUserNamePage = new FixUserName();
            fixUserNamePage.UpdateShow(accountManager.LoginAccount.UserName);
            fixUserNamePage.UserNameChanged += fixUserNamePage_UserNameChanged;
            framewPlane.NavigationService.Navigate(fixUserNamePage);
        }
        /// <summary>
        /// 修改用户名事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void fixUserNamePage_UserNameChanged(object sender, UserNameEventArgs e)
        {
           try
           {
               var state = accountManager.UpdateAccountUserName(accountManager.LoginAccount.UserName, e.UserNameStr);
               if (state)
               {
                   accountManager.SaveAccountInformation();
                   fixUserNamePage.UpdateShow(accountManager.LoginAccount.UserName);
                   MessageBox.Show("修改用户名成功.", "修改用户名");
               }

           }
            catch(Exception ex)
           {
               MessageBox.Show(ex.Message, "修改用户名");
           }
        }


        /// <summary>
        /// 修改当前用户密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fixPassword_Selected(object sender, RoutedEventArgs e)
        {
            var page = new FixPassword();           
            page.UpdateShow(accountManager.LoginAccount.UserName);
            page.UserPasswordChanged += page_UserPasswordChanged;

            framewPlane.NavigationService.Navigate(page);
        }

        /// <summary>
        /// 用户密码变更消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void page_UserPasswordChanged(object sender, UserPasswordEventArgs e)
        {
           try
           {
               //检测输入的旧密码是否与库里一致
               if (!accountManager.CheckConformPassword(accountManager.LoginAccount, 
                   e.UserOldPassword))
               {

                   MessageBox.Show("输入的旧密码不正确", "密码验证");

                   return;
               }
             
               if (!accountManager.CheckPasswordSame(e.UserNewPassword, e.UserNewPasswordRepeat))
               {
                   MessageBox.Show("两次输入的密码不一致", "密码验证");
                   return;
               }
               if (!accountManager.CheckPasswordComplexity(e.UserNewPassword))
               {
                   MessageBox.Show("密码不符合强度要求，需要不小于8位，且要包含数字，字母和特殊符号。", "密码验证");
                   return;
               }

               var state = accountManager.UpdateAccountPassword(accountManager.LoginAccount.UserName,
                   e.UserNewPassword);
               if (state)
               {
                   accountManager.SaveAccountInformation();
                   MessageBox.Show("修改密码成功", "修改密码");
               }
             
           }
            catch(Exception ex)
           {
               MessageBox.Show(ex.Message, "用户密码变更消息");
            }
        }
  


        /// <summary>
        /// 新建用户页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void establishedAccount_Selected(object sender, RoutedEventArgs e)
        {
            var newAccountPage = new EstablishAcconut();
            newAccountPage.EstablishNewAccountEvent += newAccountPage_EstablishNewAccountEvent;
            framewPlane.NavigationService.Navigate(newAccountPage);
        }
        /// <summary>
        /// 新建用户消息相应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void newAccountPage_EstablishNewAccountEvent(object sender, EstablishAccountEventArgs e)
        {
            try
            {
                if(!accountManager.CheckPasswordSame(e.UserNewPassword, e.UserNewPasswordRepeat))
                {
                    throw new ArgumentException("用户两次输入的密码不一致");
                }
                if (!accountManager.CheckPasswordComplexity(e.UserNewPassword))
                {
                    MessageBox.Show("密码不符合强度要求，需要不小于8位，且要包含数字，字母和特殊符号。", "密码验证");
                    return;
                }
               if( accountManager.EstablishNewAccount(e.UserName, e.UserNewPassword, e.Authority))
               {
                   accountManager.SaveAccountInformation();
                   MessageBox.Show("新建用户成功", "新建用户");
               }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "新建用户消息处理");
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteAccount_Selected(object sender, RoutedEventArgs e)
        {
            var page = new DeleteAccount();
            page.UpdateShow(accountManager);
            framewPlane.NavigationService.Navigate(page);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            accountManager.SaveAccountInformation();
        }




       

  
      


        

       
    }
}
