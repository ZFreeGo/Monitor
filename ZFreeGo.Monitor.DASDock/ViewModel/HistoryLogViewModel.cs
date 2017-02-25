using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using ZFreeGo.Monitor.DASModel.GetViewData;
using ZFreeGo.Monitor.DASModel.DataItemSet;
using System;
using ZFreeGo.TransmissionProtocols;
using ZFreeGo.TransmissionProtocols.BasicElement;

namespace ZFreeGo.Monitor.DASDock.ViewModel
{
   
    public class HistoryLogViewModel : ViewModelBase
    {
        private MonitorViewData viewData;
        private NetWorkProtocolServer protocolServer;
        /// <summary>
        /// Initializes a new instance of the DataGridPageViewModel class.
        /// </summary>
        public HistoryLogViewModel()
        {
            _userData = new ObservableCollection<HistoryLog>();
            LoadDataCommand = new RelayCommand(ExecuteLoadDataCommand);
            Messenger.Default.Register<MonitorViewData>(this, "LoadData", ExecuteLoadData);
          
            Messenger.Default.Register<NetWorkProtocolServer>(this, "NetWorkProtocolServer", ExecuteNetWorkProtocolServer);
         
        }

        private void ExecuteNetWorkProtocolServer(NetWorkProtocolServer obj)
        {
            if (obj != null)
            {
                protocolServer = obj;

            }
        }

        private void ExecuteLoadData(MonitorViewData obj)
        {
            try
            {
                if (obj != null)
                {
                   // UserData = obj.ReadEletricPulse(false);
                    viewData = obj;
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }

        /************** 属性 **************/
        private ObservableCollection<HistoryLog> _userData;
        /// <summary>
        /// 用户信息数据
        /// </summary>
        public ObservableCollection<HistoryLog> UserData
        {
            get { return _userData; }
            set
            {
                _userData = value;
                RaisePropertyChanged("UserData");
            }
        }
        #region 加载数据命令：LoadDataCommand
        /// <summary>
        /// 加载数据
        /// </summary>
        public RelayCommand LoadDataCommand { get; private set; }

        //加载用户数据
        void ExecuteLoadDataCommand()
        {
            //var get = new GetViewData();
            //UserData = get.GetTelemeteringList();
        }

        #endregion


    }
}