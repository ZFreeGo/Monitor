using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using ZFreeGo.Monitor.DASModel.GetViewData;
using ZFreeGo.Monitor.DASModel.DataItemSet;
using ZFreeGo.Monitor.DASModel;
using System;
using ZFreeGo.Monitor.DASModel.Helper;
using System.Windows.Media;
using System.Timers;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols;

namespace ZFreeGo.Monitor.DASDock.ViewModel
{

    public class CalibrationViewModel : ViewModelBase, IDisposable
    {
        /// <summary>
        /// 通讯服务
        /// </summary>
        private CommunicationServer netServer;
        private StateMessage messageCollect;
        private OldCalibration calbrationServer;
        private NetWorkProtocolServer protocolServer;
        private Timer loopTimer;
        private int sendCount = 0;
        /// <summary>
        /// Initializes a new instance of the DataGridPageViewModel class.
        /// </summary>
        public CalibrationViewModel()
        {
            _userData = new ObservableCollection<SystemCalibration>();
            LoadDataCommand = new RelayCommand(ExecuteLoadDataCommand);

            Messenger.Default.Register<DASModelServer>(this, "DASModelServer", ExecuteDASModelServer);

            DataGridMenumSelected = new RelayCommand<string>(ExecuteDataGridMenumSelected);
            StartCalibrationCommand = new RelayCommand(ExecuteStartCalibrationCommand);
            StopCalibrationCommand = new RelayCommand(ExecuteStopCalibrationCommand);
            loopTimer = new Timer();
            loopTimer.AutoReset = true;
            loopTimer.Elapsed += loopTimer_Elapsed;
        }

        void loopTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                loopTimer.Interval = updateTime;
                if (sendCount++ < 10)
                {
                    protocolServer.CallServer.StartServer(CauseOfTransmissionList.Activation, QualifyOfInterrogationList.GeneralInterrogation);

                }
                else
                {
                    sendCount = 0;
                    loopTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                messageCollect.AddCustomNetMessage(ex.Message + "::" + ex.StackTrace);
            }

        }

        private void ExecuteDASModelServer(DASModelServer obj)
        {
            if (obj != null)
            {
                netServer = obj.Communication;
                messageCollect = obj.DataFile.StateMessage;
                calbrationServer = obj.CustomServer;
                UserData = obj.DataFile.MonitorData.GetSystemCalibrationList();
                netServer.NetCustomClient.LinkingEventMsg += NetCustomClient_LinkingEventMsg;
            }
        }

        void NetCustomClient_LinkingEventMsg(object sender, Net.Element.NetLinkMessagEvent e)
        {
            switch (e.State)
            {
                case Net.Element.NetState.EstablishLink:
                    {
                        Tip = "校准模式运行中.......";
                        StartTimer();
                        break;
                    }
                case Net.Element.NetState.Stop:
                    {
                        Tip = "校准模式停止。";
                        sendCount = 0;
                        loopTimer.Stop();
                        break;
                    }
            }
        }

        private void StartTimer()
        {
            sendCount = 0;
            loopTimer.Interval = updateTime;
            loopTimer.Start();
        }


        /************** 属性 **************/
        private ObservableCollection<SystemCalibration> _userData;
        /// <summary>
        /// 用户信息数据
        /// </summary>
        public ObservableCollection<SystemCalibration> UserData
        {
            get { return _userData; }
            set
            {
                _userData = value;
                RaisePropertyChanged("UserData");
            }
        }
        #region 加载数据命令 LoadDataCommand
        /// <summary>
        /// 加载数据
        /// </summary>
        public RelayCommand LoadDataCommand { get; private set; }

        //加载用户数据
        void ExecuteLoadDataCommand()
        {
            //var get = new GetViewData();

            //  UserData = get.GetSystemCalibrationList();
        }
        #endregion


        private bool realUpdate = false;
        public bool RealUpdate
        {
            get
            {
                return realUpdate;
            }
            set
            {
                realUpdate = value;
                calbrationServer.IsRealUpdate = value;
                calbrationServer.UpdateIndex = 0;
                RaisePropertyChanged("RealUpdate");



            }
        }

        private int updateTime = 500;

        /// <summary>
        /// 更新时间
        /// </summary>
        public int UpdateTime
        {
            get
            {

                return updateTime;
            }
            set
            {
                if (value >= 500)
                {
                    updateTime = value;
                }
                else
                {
                    updateTime = 500;
                }


                RaisePropertyChanged("UpdateTime");
            }
        }


        private string tip = "校准模式停止.";

        public string Tip
        {
            get
            {
                return tip;
            }
            set
            {
                tip = value;
                RaisePropertyChanged("Tip");
            }
        }


        #region 启动校准
        /// <summary>
        /// 加载数据
        /// </summary>
        public RelayCommand StartCalibrationCommand { get; private set; }

        //加载用户数据
        void ExecuteStartCalibrationCommand()
        {
            try
            {
                if (netServer.NetClient.IsRun) //必须2404必须先连上
                {
                    netServer.NetCustomClient.StartServer(netServer.NetParameter.IP, 8000);
                }
            }
            catch (Exception ex)
            {
                messageCollect.AddCustomNetMessage(ex.Message + "::" + ex.StackTrace);
            }
        }
        public RelayCommand StopCalibrationCommand { get; private set; }

        //加载用户数据
        void ExecuteStopCalibrationCommand()
        {
            try
            {
                netServer.NetCustomClient.Stop();
            }
            catch (Exception ex)
            {
                messageCollect.AddCustomNetMessage(ex.Message + "::" + ex.StackTrace);
            }
        }
        #endregion

        #region 系数操作
        public RelayCommand<string> FactorOperationCommand;

        private void ExecuteFactorOperationCommand(string arg)
        {
            try
            {
                if (netServer.NetCustomClient.IsRun)
                {
                    switch (arg)
                    {
                        case "Read":
                            {
                                var data = calbrationServer.MakeFactorCallData();
                                netServer.NetCustomClient.SendMessage(data);
                                messageCollect.AddCustomNetMessage("召唤系数");
                                messageCollect.AddCustomNetMessage(data, true);
                                break;
                            }
                        case "Download":
                            {
                                var downfactor = GetFact();
                                var senddata = calbrationServer.MakeFactorDownloadData(downfactor, UserData.Count);
                                netServer.NetCustomClient.SendMessage(senddata);


                                messageCollect.AddCustomNetMessage("下载系数");
                                messageCollect.AddCustomNetMessage(senddata, true);
                                break;
                            }
                        case "Fix":
                            {
                                var data = calbrationServer.MakeFactorFix();
                                netServer.NetCustomClient.SendMessage(data);
                                messageCollect.AddCustomNetMessage("固化系数");
                                messageCollect.AddCustomNetMessage(data, true);
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                messageCollect.AddCustomNetMessage(ex.Message + "::" + ex.StackTrace);
            }
        }
        #endregion

        private UInt32[] GetFact()
        {
            var downfactor = new UInt32[UserData.Count];
            int i = 0;
            foreach (var m in UserData)
            {
                //测试使用
                //  m.DownloadCoefficient = (UInt32)m.CallCoefficient;
                downfactor[i++] = (UInt32)m.DownloadCoefficient;
            }
            return downfactor;
        }



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
                case "Reload":
                    {

                        // UserData = viewData.ReadEletricPulse(true);
                        break;
                    }
                case "Save":
                    {
                        // viewData.InsertEletricPulse();
                        break;
                    }
                case "AddUp":
                    {
                        if (SelectedIndex > -1)
                        {
                            var item = new ElectricPulse(0, "", 0, "", "", "");
                            //UserData.Insert(SelectedIndex, item);
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

                                //UserData.Insert(SelectedIndex + 1, item);
                            }
                            else
                            {
                                //UserData.Add(item);
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


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    // Release managed resources
                }

                loopTimer.Stop();
                loopTimer.Dispose();

                m_disposed = true;
            }
        }

        ~CalibrationViewModel()
        {
            Dispose(false);
        }


        private bool m_disposed;
    }
}