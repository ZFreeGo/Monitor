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
            DataGridMenumSelected = new RelayCommand<string>(ExecuteDataGridMenumSelected);
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
                    UserData = obj.ReadEletricPulse(false);
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


        #region 表格操作
        private bool fixCheck = false;
        /// <summary>
        /// 检测使能
        /// </summary>
        public bool FixCheck
        {
            get
            {
                return fixCheck;
            }
            set
            {
                fixCheck = value;
                RaisePropertyChanged("FixCheck");                
                RaisePropertyChanged("ReadOnly");
               
            }
        }       
        
        /// <summary>
        /// 检测使能
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return !fixCheck;
            }

        }


        private int selectedIndex = 0;
        /// <summary>
        /// 选择索引
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }
       
        


        public RelayCommand<string> DataGridMenumSelected { get; private set; }

        private void ExecuteDataGridMenumSelected(string name)
        {
            if (!FixCheck)
            {
                return;
            }
            switch (name)
            {
                case "CallElectricPulse":
                    {
                        try
                        {
                            protocolServer.ElectricPulse.StartServer(CauseOfTransmissionList.Activation, 
                                new QualifyCalculateCommad(QCCRequest.All, QCCFreeze.Read));
                        }
                        catch (Exception ex)
                        {
                            Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
                        }
                        break;
                    }
                case "Reload":
                    {

                        UserData = viewData.ReadEletricPulse(true);
                        break;
                    }
                case "Save":
                    {
                        viewData.InsertEletricPulse();
                        break;
                    }
                case "AddUp":
                    {
                        if (SelectedIndex > -1)
                        {
                            var item = new ElectricPulse(0, "", 0, "", "", "");
                            UserData.Insert(SelectedIndex, item);
                        }
                        break;
                    }
                case "AddDown":
                    {
                        if (SelectedIndex > -1)
                        {
                            var item = new ElectricPulse(0, "", 0, "", "", "");
                            if (SelectedIndex < UserData.Count - 1)
                            {

                                UserData.Insert(SelectedIndex + 1, item);
                            }
                            else
                            {
                                UserData.Add(item);
                            }
                        }
                        break;
                    }
                case "DeleteSelect":
                    {
                        if (SelectedIndex > -1)
                        {
                            //var result = MessageBox.Show("是否删除选中行:" + gridTelesignalisation.SelectedItem.ToString(),
                            //    "确认删除", MessageBoxButton.OKCancel);
                            var result = true;
                            if (result)
                            {
                                UserData.RemoveAt(SelectedIndex);
                            }
                        }
                        break;
                    }
            }
        }
        #endregion
    }
}