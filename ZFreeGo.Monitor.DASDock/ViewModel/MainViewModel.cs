using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;
using ZFreeGo.Monitor.DASModel;
using ZFreeGo.Monitor.DASModel.GetViewData;
using ZFreeGo.TransmissionProtocols;


namespace ZFreeGo.Monitor.DASDock.ViewModel
{

    public class MainViewModel : ViewModelBase
    {

        private DASModelServer dasModelServer;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ShowUserView = new RelayCommand<string>(ExecuteShowUserView);
           
            dasModelServer = new DASModelServer();

            Messenger.Default.Register<string>(this, "ExecuteLoadDataFirst", ExecuteLoadData);
            Messenger.Default.Register<string>(this, "ExecuteMainWindowsClose", ExecuteMainWindowsClose);
        }

        /// <summary>
        /// 主窗体退出命令
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteMainWindowsClose(string obj)
        {
            dasModelServer.StopServer();
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
   
      

        //发送显示UserView的消息
        void ExecuteLoadData(string name)
        {
            Messenger.Default.Send<MonitorViewData>(dasModelServer.DataFile.MonitorData, "LoadData");
            Messenger.Default.Send<CommunicationServer>(dasModelServer.Communication, "CommunicationServer");
            Messenger.Default.Send<NetWorkProtocolServer>(dasModelServer.ProtocolServer, "NetWorkProtocolServer");
        
        }
      
       





        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}