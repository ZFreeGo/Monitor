using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;

namespace ZFreeGo.Monitor.DASII.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
       

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ShowUserView = new RelayCommand<string>(ExecuteShowUserView);
            
        }
        /********* 命令 **********/
        /// <summary>
        /// 显示UserView窗口
        /// </summary>
        public RelayCommand<string> ShowUserView { get; private set; }

        //发送显示UserView的消息
        void ExecuteShowUserView(string name)
        {


            Messenger.Default.Send<string>(name, "ShowUserView");
        }

      






        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}