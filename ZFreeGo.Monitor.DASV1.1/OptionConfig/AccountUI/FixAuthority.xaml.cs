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
    /// FixStyle.xaml 的交互逻辑
    /// </summary>
    public partial class FixAuthority : Page
    {
        /// <summary>
        /// 用户权限变更事件
        /// </summary>
        public event EventHandler<UserAuthorityEventArgs> UserAuthorityChanged;

        /// <summary>
        /// 当前用户权限等级
        /// </summary>
        private AuthorityLevel currentAuthorityLevel;
        /// <summary>
        /// 选择的用户等级
        /// </summary>
        AuthorityLevel level = AuthorityLevel.None;
        public FixAuthority()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 更新显示数据
        /// </summary>
        /// <param name="strName">用户名</param>
        /// <param name="level">权限等级</param>
        public void UpdateShow(string strName, AuthorityLevel level)
        {
            currentUserName.Text = strName;
            currentAuthorityLevel = level;

            int levelNum = (int)level;
            if(levelNum < 4)
            {
                radioLevelIV.IsEnabled = false;
            }
            
            if (levelNum < 3)
            {
                radioLevelIII.IsEnabled = false;
            }
           
            if (levelNum < 2)
            {
                radioLevelII.IsEnabled = false;
            }
            
            if (levelNum < 1)
            {
                radioLevelI.IsEnabled = false;
            }
            switch (level)
            {
                case AuthorityLevel.I:
                    {
                        radioLevelI.IsChecked = true;
                        break;
                    }
                case AuthorityLevel.II:
                    {
                        radioLevelII.IsChecked = true;
                        break;
                    }
                case AuthorityLevel.III:
                    {
                        radioLevelIII.IsChecked = true;
                        break;
                    }
                case AuthorityLevel.IV:
                    {
                        radioLevelIV.IsChecked = true;
                        break;
                    }
            }
        }

        /// <summary>
        /// 权限选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioLevel_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                

                switch ((sender as RadioButton).Name)
                {
                    case "radioLevelI":
                        {
                            level = AuthorityLevel.I;
                            break;
                        }
                    case "radioLevelII":
                        {
                            level = AuthorityLevel.II;
                            break;
                        }
                    case "radioLevelIII":
                        {
                            level = AuthorityLevel.III;
                            break;
                        }
                    case "radioLevelIV":
                        {
                            level = AuthorityLevel.IV;
                            break;
                        }

                }
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "修改权限");
            }
         
           
        }

        /// <summary>
        /// 应用修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApplyChanged_Cliked(object sender, RoutedEventArgs e)
        {
             if (UserAuthorityChanged != null)
                {
                    UserAuthorityChanged(this, new UserAuthorityEventArgs(level));
                }
        }
    }
}
