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
    /// EstablishAcconut.xaml 的交互逻辑
    /// </summary>
    public partial class EstablishAcconut : Page
    {
        /// <summary>
        /// 新建用户事件
        /// </summary>
        public event EventHandler<EstablishAccountEventArgs> EstablishNewAccountEvent;

        
        public EstablishAcconut()
        {
            InitializeComponent();
        }

        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            combLevel.Items.Add("I");
            combLevel.Items.Add("II");
            combLevel.Items.Add("III");
            combLevel.Items.Add("IV");
            combLevel.SelectedIndex = 0;
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (EstablishNewAccountEvent != null)
                {
                    EstablishNewAccountEvent(this, new EstablishAccountEventArgs(
                        txtNewUserName.Text, passBoxNew.SecurePassword, passBoxNewRepeat.SecurePassword,
                         (Secure.AuthorityLevel)(combLevel.SelectedIndex + 1)));

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "建立新用户");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            txtNewUserName.Text = "";
            passBoxNewRepeat.SecurePassword.Clear();
            passBoxNew.SecurePassword.Clear();
        }

      
    }
}
