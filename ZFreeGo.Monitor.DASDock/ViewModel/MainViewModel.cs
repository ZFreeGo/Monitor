using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;
using ZFreeGo.Monitor.DASModel;
using ZFreeGo.Monitor.DASModel.DataItemSet;
using ZFreeGo.Monitor.DASModel.GetViewData;
using ZFreeGo.TransmissionProtocols;


namespace ZFreeGo.Monitor.DASDock.ViewModel
{

    public class MainViewModel : ViewModelBase
    {

        private DASModelServer dasModelServer;
        private StateMessage messageCollect;
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
        /// 检测MessageCollectNull是否为空集
        /// </summary>
        void CheckMessageCollectNull()
        {
            if (messageCollect == null)
            {

                messageCollect = dasModelServer.DataFile.StateMessage;

                messageCollect.PropertyChanged += messageCollect_PropertyChanged;

            }
        }
        void messageCollect_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
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


        #region 状态信息

        public string NetMessage
        {
            get
            {
                CheckMessageCollectNull();
                return messageCollect.NetMessage;
            }
            set
            {
                messageCollect.NetMessage = value;
                RaisePropertyChanged("NetMessasge");
            }
        }      

        /// <summary>
        /// 通讯原始接收信息 16进制显示
        /// </summary>
        public string NetRawData
        {
            get
            {
                CheckMessageCollectNull();
                return messageCollect.NetRawData;
            }
            set
            {
                messageCollect.NetRawData = value;
                RaisePropertyChanged("NetRawData");

            }
        }

        /// <summary>
        /// 规约解析信息
        /// </summary>
        public string ProtoclMessage
        {
            get
            {
                CheckMessageCollectNull();
                return messageCollect.ProtoclMessage;
            }
            set
            {
                messageCollect.ProtoclMessage = value;
                RaisePropertyChanged("ProtoclMessage");

            }
        }
        /// <summary>
        /// 异常跟踪信息
        /// </summary>
        public string ExceptionTrace
        {
            get
            {
                CheckMessageCollectNull();
                return messageCollect.ExceptionTrace;
            }
            set
            {
                messageCollect.ExceptionTrace = value;
                RaisePropertyChanged("ExceptionTrace");

            }
        }
        /// <summary>
        /// 校准网络信息
        /// </summary>
        public string CustomNetMessage
        {
            get
            {
                CheckMessageCollectNull();
                return messageCollect.CustomNetMessage;
            }
            set
            {
                messageCollect.CustomNetMessage = value;
                RaisePropertyChanged("CustomNetMessage");

            }
        }
        #endregion
        //发送显示UserView的消息
        void ExecuteLoadData(string name)
        {
            Messenger.Default.Send<MonitorViewData>(dasModelServer.DataFile.MonitorData, "LoadData");
            Messenger.Default.Send<CommunicationServer>(dasModelServer.Communication, "CommunicationServer");
            Messenger.Default.Send<NetWorkProtocolServer>(dasModelServer.ProtocolServer, "NetWorkProtocolServer");
            Messenger.Default.Send<StateMessage>(dasModelServer.DataFile.StateMessage, "StateMessage");
            Messenger.Default.Send<DASModelServer>(dasModelServer, "DASModelServer");
        }
      
       





        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}