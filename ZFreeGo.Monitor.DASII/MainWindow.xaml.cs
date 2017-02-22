using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using System.Windows.Controls;
using ZFreeGo.Monitor.DASII.View;
using ZFreeGo.Monitor.DASII.ViewModel;

namespace ZFreeGo.Monitor.DASII
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        /// 
        TelesignalisationView viewTelesignalisation;
        TelecontrolView viewTelecontrol;
        TelemeteringView viewTelemetering;

        SOEView viewSOE;
        ProtectSetpointView viewProtectSetpoint;
        ParameterView viewParameter;
        CalibrationView viewCalibration;
        CommunicationView viewCommunication;
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            //注册MVVMLight消息
            Messenger.Default.Register<string>(this, "ShowUserView", ShowUserView);
            Messenger.Default.Register<Exception>(this, "ExceptionMessage", ExceptionMessage);
            viewTelesignalisation = new TelesignalisationView();
            viewTelecontrol = new TelecontrolView();
            viewTelemetering = new TelemeteringView();
            viewSOE = new SOEView();
            viewProtectSetpoint = new ProtectSetpointView();
            viewParameter = new ParameterView();
            viewCalibration = new CalibrationView();
            viewCommunication = new CommunicationView();
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

    

        private void ShowUserView(string obj)
        {
            if (obj != null)
            {
                switch(obj)
                {
                    case "Telesignalisation":
                        {
                            frame.NavigationService.Navigate(viewTelesignalisation);
                            break;
                        }
                    case "Telemetering":
                        {
                            frame.NavigationService.Navigate(viewTelemetering);
                            break;
                        }
                    case "Telecontrol":
                        {
                            frame.NavigationService.Navigate(viewTelecontrol);
                            break;
                        }
                    case "SOELog":
                        {
                            frame.NavigationService.Navigate(viewSOE);
                            break;
                        }
                    case "ProtectSetPoint":
                        {
                            frame.NavigationService.Navigate(viewProtectSetpoint);
                            break;
                        }
                    case "SystemParameter":
                        {
                            frame.NavigationService.Navigate(viewParameter);
                            break;
                        }
                    case "SystemCalibration":
                        {
                            frame.NavigationService.Navigate(viewCalibration);
                            break;
                        }
                    case "Communication":
                        {
                            frame.NavigationService.Navigate(viewCommunication);
                            break;
                        }
                   
                }
               
            }
           
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Messenger.Default.Send<string>("MainWindowsClose", "ExecuteMainWindowsClose");
        }

    }
}