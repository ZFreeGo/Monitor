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
   
    public class FileServerViewModel : ViewModelBase
    {
        private MonitorViewData viewData;
        private NetWorkProtocolServer protocolServer;
        /// <summary>
        /// Initializes a new instance of the DataGridPageViewModel class.
        /// </summary>
        public FileServerViewModel()
        {
            _userData = new ObservableCollection<ElectricPulse>();
            LoadDataCommand = new RelayCommand(ExecuteLoadDataCommand);
            Messenger.Default.Register<MonitorViewData>(this, "LoadData", ExecuteLoadData);
            
            Messenger.Default.Register<NetWorkProtocolServer>(this, "NetWorkProtocolServer", ExecuteNetWorkProtocolServer);
            CallCatalogueCommand = new RelayCommand(ExecuteCallCatalogueCommand);
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
                  //  UserData = obj.ReadEletricPulse(false);
                    viewData = obj;
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }

        /************** 属性 **************/
        private ObservableCollection<ElectricPulse> _userData;
        /// <summary>
        /// 用户信息数据
        /// </summary>
        public ObservableCollection<ElectricPulse> UserData
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

        #region 目录召唤

        private bool timeCheck = false;
        /// <summary>
        /// 时间段使能
        /// </summary>
        public bool TimeCheck
        {
            get
            {
                return timeCheck;
            }
            set
            {
                timeCheck = value;
                RaisePropertyChanged("TimeCheck");              
            }
        }

        private UInt32 directoryID = 0;
        /// <summary>
        /// 目录ID
        /// </summary>
        public UInt32 DirectoryID
        {
            get
            {
                return directoryID;
            }
            set
            {
                directoryID = value;
                RaisePropertyChanged("DirectoryID");
            }
        }


        private string directoryName = "all";
        /// <summary>
        /// 目录ID
        /// </summary>
        public string DirectoryName
        {
            get
            {
                return directoryName;
            }
            set
            {
                directoryName = value;
                RaisePropertyChanged("DirectoryName");
            }
        }

        private DateTime startTime = DateTime.Now;
        /// <summary>
        /// 目录ID
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                RaisePropertyChanged("StartTime");
            }
        }
        private DateTime endTime = DateTime.Now;
        /// <summary>
        /// 目录ID
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
                RaisePropertyChanged("EndTime");
            }
        }

        /// <summary>
        /// 召唤目录命令
        /// </summary>
        public RelayCommand CallCatalogueCommand { get; private set; }

        //加载用户数据
        void ExecuteCallCatalogueCommand()
        {
            //var get = new GetViewData();
            //UserData = get.GetTelemeteringList();
        }
        #endregion

    }
}