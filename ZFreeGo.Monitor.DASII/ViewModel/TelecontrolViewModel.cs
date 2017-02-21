using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using ZFreeGo.Monitor.DASModel.GetViewData;
using ZFreeGo.Monitor.DASModel.DataItemSet;
using ZFreeGo.TransmissionProtocols.ControlProcessInformation;
using ZFreeGo.TransmissionProtocols;
using ZFreeGo.TransmissionProtocols.BasicElement;
using System;
using ZFreeGo.TransmissionProtocols.ControlSystemCommand;
using ZFreeGo.Monitor.DASModel.Helper;
using ZFreeGo.Monitor.AutoStudio.ElementParam;

namespace ZFreeGo.Monitor.DASII.ViewModel
{
   
    public class TelecontrolViewModel : ViewModelBase
    {
        private ControlServer telecontrolServer;
        private TimeSynchronizationServer timeServer;

        /// <summary>
        /// Initializes a new instance of the DataGridPageViewModel class.
        /// </summary>
        public TelecontrolViewModel()
        {
            

            timeSynEnable = true;
            _userData = new ObservableCollection<Telecontrol>();
            LoadDataCommand = new RelayCommand(ExecuteLoadDataCommand);
            Messenger.Default.Register<MonitorViewData>(this, "LoadData", ExecuteLoadData);
            Messenger.Default.Register<NetWorkProtocolServer>(this, "NetWorkProtocolServer", ExecuteNetWorkProtocolServer);
            Close = new RelayCommand<string>(ExecuteClose);
            Open = new RelayCommand<string>(ExecuteOpen);
            TimeSyn = new RelayCommand(ExecuteTimeSyn);
        }

        private void InitTime()
        {
            clockTime = new Clock();
            clockTime.Time.PropertyChanged += Time_PropertyChanged;
        }

        void Time_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("Time");
           
        }

      

        private void ExecuteNetWorkProtocolServer(NetWorkProtocolServer obj)
        {
            if (obj != null)
            {
                telecontrolServer = obj.TelecontrolServer;
                timeServer = obj.TimeServer;
            }
        }





        private void ExecuteLoadData(MonitorViewData obj)
        {
            if (obj != null)
            {
                UserData = obj.GetTelecontrolList();
            }
        }


        private Clock clockTime;

        public ClockElement Time
        {
            get
            {
                if (clockTime == null)
                {
                    InitTime();
                }
                return clockTime.Time;
            }
            set
            {
                clockTime.Time = value;
                RaisePropertyChanged("Time");
            }
        }
        private bool timeSynEnable;

        public bool TimeSynEnable
        {
            get
            {
                return timeSynEnable;
            }
            set
            {
                clockTime.UpdateFlag= value;
                timeSynEnable = value;
                RaisePropertyChanged("TimeSynEnable");
            }
        }

        private ObservableCollection<Telecontrol> _userData;
        /// <summary>
        /// 用户信息数据
        /// </summary>
        public ObservableCollection<Telecontrol> UserData
        {
            get { return _userData; }
            set
            {
                _userData = value;
                RaisePropertyChanged("UserData");
            }
        }

      
         /// <summary>
        /// 加载数据
        /// </summary>
        public RelayCommand TimeSyn { get; private set; }

        //加载用户数据
        void ExecuteTimeSyn()
        {
            try
            {
                timeServer.StartServer(CauseOfTransmissionList.Activation, new CP56Time2a(DateTime.Now));
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
           // var get = new GetViewData();
            //UserData = get.GetTelecontrolList();
        }
        #endregion

        #region 合闸
        /// <summary>
        /// 执行合闸
        /// </summary>
        public RelayCommand<string> Close { get; private set; }


        void ExecuteClose(string str)
        {
            try
            {
               
                switch (str)
                {
                    case "Selected":
                        {
                            telecontrolServer.StartServer(CauseOfTransmissionList.Activation, 0,
                                Telecontrol.BasicAddress + 2 - 1,
                                new DoubleCommand(SelectExecuteOption.Select, QUtype.NODefine, DCOState.On));
                            break;
                        }
                    case "Execute":
                        {
                            telecontrolServer.SendActionCommand(CauseOfTransmissionList.Activation, 0,
                                 Telecontrol.BasicAddress + 2 - 1,
                                 new DoubleCommand(SelectExecuteOption.Execute, QUtype.NODefine, DCOState.On));
                            break;
                        }
                }


            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }
        /// <summary>
        /// 执行分闸
        /// </summary>
        public RelayCommand<string> Open { get; private set; }
        
        void ExecuteOpen(string str)
        {
            try
            {

                switch (str)
                {
                    case "Selected":
                        {
                            telecontrolServer.StartServer(CauseOfTransmissionList.Activation, 0,
                                Telecontrol.BasicAddress + 1 - 1,
                                new DoubleCommand(SelectExecuteOption.Select, QUtype.NODefine, DCOState.Off));
                            break;
                        }
                    case "Execute":
                        {
                            telecontrolServer.SendActionCommand(CauseOfTransmissionList.Activation, 0,
                                 Telecontrol.BasicAddress + 1 - 1,
                                 new DoubleCommand(SelectExecuteOption.Execute, QUtype.NODefine, DCOState.Off));
                            break;
                        }
                }


            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }
        #endregion




       
    }
}