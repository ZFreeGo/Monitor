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
            Messenger.Default.Register<string>(this, "ShowUserView", ShowUserView);
           
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
            this.Hide();
            Messenger.Default.Send<string>("MainWindowsClose", "ExecuteMainWindowsClose");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<Exception>(this, "ExceptionMessage", ExceptionMessage);
            Messenger.Default.Send<string>("Start", "ExecuteLoadDataFirst");
        }

        private void ShowUserView(string obj)
        {
           
            if (obj != null)
            {

                switch (obj)
                {
                    case "Telesignalisation":
                        {
                            
                            break;
                        }
                    case "Telemetering":
                        {
                           
                            break;
                        }
                    case "Telecontrol":
                        {
                           
                            break;
                        }
                    case "SOELog":
                        {
                            
                            break;
                        }
                    case "ProtectSetPoint":
                        {
                            
                            break;
                        }
                    case "SystemParameter":
                        {
                            
                            break;
                        }
                    case "SystemCalibration":
                        {
                            
                            break;
                        }
                    case "Communication":
                        {
                          
                            break;
                        }

                }

            }

        }

    }
}