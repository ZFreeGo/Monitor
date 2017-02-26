using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using ZFreeGo.Monitor.DASModel.GetViewData;
using ZFreeGo.Monitor.DASModel.DataItemSet;
using System;
using ZFreeGo.TransmissionProtocols;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.FileSever;

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
           
            LoadDataCommand = new RelayCommand(ExecuteLoadDataCommand);
            Messenger.Default.Register<MonitorViewData>(this, "LoadData", ExecuteLoadData);
            
            Messenger.Default.Register<NetWorkProtocolServer>(this, "NetWorkProtocolServer", ExecuteNetWorkProtocolServer);
            CallCatalogueCommand = new RelayCommand(ExecuteCallCatalogueCommand);
            ReadFileCommand = new RelayCommand(ExecuteReadFileCommand);
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

                    DirectoryConent = obj.GetDirectoryList();
                    viewData = obj;
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
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

        #region 读取文件

        private string readFileName = "all";
        /// <summary>
        /// 目录ID
        /// </summary>
        public string ReadFileName
        {
            get
            {
                return readFileName;
            }
            set
            {
                readFileName = value;
                RaisePropertyChanged("ReadFileName");
            }
        }

        /// <summary>
        /// 召唤目录命令
        /// </summary>
        public RelayCommand ReadFileCommand { get; private set; }

        //加载用户数据
        private void ExecuteReadFileCommand()
        {
            try
            {
                var packet = new  FileReadActivityPacket(readFileName);
                protocolServer.FileRead.StartServer(packet);

            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }
        #endregion



        #region 目录召唤
        public ObservableCollection<FileAttributeItem> directoryConent = null;

        public ObservableCollection<FileAttributeItem> DirectoryConent
        {
            set
            {
                directoryConent = value;
                RaisePropertyChanged("DirectoryConent");       
            }
            get
            {
                return directoryConent;
            }
        }


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

        /// <summary>
        /// 执行召唤文件命令
        /// </summary>
        private void ExecuteCallCatalogueCommand()
        {
            try
            {
                FileCallFlag flag;
                if (timeCheck)
                {
                     flag = FileCallFlag.MeetTime;
                }
                else
                {
                    flag = FileCallFlag.All;
                }

                var packet = new FileDirectoryCalledPacket(directoryID, directoryName,
                    flag, new CP56Time2a(startTime), new CP56Time2a(endTime));
                protocolServer.CallFileDirectory.StartServer(packet);
            }
            catch(Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }

        }
        #endregion

    }
}