using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZFreeGo.Monitor.AutoStudio.Database;

namespace ZFreeGo.Monitor.AutoStudio.Secure
{
    /// <summary>
    /// AuthoritySettingUI.xaml 的交互逻辑
    /// </summary>
    public partial class AuthoritySettingUI : Window
    {
        private AuthorityManager mAuthorityManager;



        private MainWindow mWindow;

        private bool restartFlag;

        ObservableCollection<ControlAuthority> mItemSouce;

        public AuthoritySettingUI(AuthorityManager authorityManager, MainWindow main)
        {
            InitializeComponent();
            mAuthorityManager = authorityManager;
            mWindow = main;
            restartFlag = false;
        }


        



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mAuthorityManager.LoadAuthorityData();
            mItemSouce = mAuthorityManager.GetItem();
            foreach(var  m in mItemSouce)
            {
                m.UpdateData = false;
            }
            gridControl.ItemsSource = mItemSouce;
        }

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateAuthority_Click(object sender, RoutedEventArgs e)
        {


            var cn = mAuthorityManager.UpdateAuthorityDatabase(mItemSouce);
            
            MessageBox.Show(string.Format("完成更新权限,一共更新{0}条",cn), "权限设置");
            if (cn!=0)
            {
                restartFlag = true; 
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (restartFlag)
            {
                mWindow.Close();
            }
            
        }

        private void btnDataSameUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

    
    }
}
