using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using ZFreeGo.Monitor.DASDock.ViewModel;

namespace ZFreeGo.Monitor.DASDock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            //注册MVVMLight消息
            //Messenger.Default.Register<string>(this, "ShowUserView", ShowUserView);
            Messenger.Default.Register<Exception>(this, "ExceptionMessage", ExceptionMessage);
            Messenger.Default.Send<string>("Start", "ExecuteLoadDataFirst");
        }
        /// <summary>
        /// 异常信息
        /// </summary>
        /// <param name="obj"></param>
        private void ExceptionMessage(Exception obj)
        {
            MessageBox.Show(obj.Message, obj.Source);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Messenger.Default.Send<string>("MainWindowsClose", "ExecuteMainWindowsClose");
        }
    }
}