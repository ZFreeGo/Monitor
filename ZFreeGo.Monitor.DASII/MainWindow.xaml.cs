using GalaSoft.MvvmLight.Messaging;
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

        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            //注册MVVMLight消息
            Messenger.Default.Register<string>(this, "ShowUserView", ShowUserView);

            viewTelesignalisation = new TelesignalisationView();
            viewTelecontrol = new TelecontrolView();
            viewTelemetering = new TelemeteringView();
            viewSOE = new SOEView();
            viewProtectSetpoint = new ProtectSetpointView();
            viewParameter = new ParameterView();
            viewCalibration = new CalibrationView();
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
                   
                }
               
            }
           
        }
    }
}