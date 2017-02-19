using GalaSoft.MvvmLight.Messaging;
using System.Windows;
using ZFreeGo.Monitor.DAS.View;
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
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            //注册MVVMLight消息
            Messenger.Default.Register<string>(this, "ShowUserView", ShowUserView);

            viewTelesignalisation = new TelesignalisationView();
        }

        private void ShowUserView(string obj)
        {
            if (obj != null)
            {
                switch (obj)
                {
                    case "Telesignalisation":
                        {
                            frame.NavigationService.Navigate(viewTelesignalisation);
                            break;
                        }
                }
            }
           
        }
    }
}