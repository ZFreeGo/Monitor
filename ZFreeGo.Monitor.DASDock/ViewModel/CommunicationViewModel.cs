using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using ZFreeGo.Monitor.DASModel.GetViewData;
using ZFreeGo.Monitor.DASModel.DataItemSet;
using ZFreeGo.Monitor.DASModel;
using System;
using ZFreeGo.Net;
using ZFreeGo.TransmissionProtocols;
using ZFreeGo.TransmissionProtocols.TransmissionControl104;
using ZFreeGo.TransmissionProtocols.BasicElement;
using System.Threading;
using System.Net;


namespace ZFreeGo.Monitor.DASDock.ViewModel
{
   
    public class CommunicationViewModel : ViewModelBase
    {
        private NetWorkProtocolServer protocolServer;


        /// <summary>
        /// 总启动标志
        /// </summary>
        private bool allFlag = false;
        /// <summary>
        /// Initializes a new instance of the DataGridPageViewModel class.
        /// </summary>
        public CommunicationViewModel()
        {
           
            LoadDataCommand = new RelayCommand(ExecuteLoadDataCommand);
            StartTcpLink = new RelayCommand<string>(ExecuteStartTcpLink);
            StopTcpLink = new RelayCommand<string>(ExecuteStopTcpLink);
            Messenger.Default.Register<CommunicationServer>(this, "CommunicationServer", ExecuteCommunicationServer);
            Messenger.Default.Register<NetWorkProtocolServer>(this, "NetWorkProtocolServer", ExecuteNetWorkProtocolServer);
            
            StartTransmission = new RelayCommand<string>(ExecuteStartTransmission);
            StopTransmission = new RelayCommand<string>(ExecuteStopTransmission);
            CallAll = new RelayCommand<string>(ExecuteCallAll);
            StartAllLink = new RelayCommand<string>(ExecuteStartAllLink);
            StopAllLink = new RelayCommand<string>(ExecuteStopAllLink);
            ClearText = new RelayCommand<string>(ExecuteClearText);
            ToEnd = new RelayCommand<string>(ExecuteToEnd);
        }

        /// <summary>
        /// 网络协议控制服务到来
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteNetWorkProtocolServer(NetWorkProtocolServer obj)
        {
            if (obj != null)
            {
                protocolServer = obj;
                protocolServer.ControlServer.ServerEvent += ControlServer_ServerEvent;
                protocolServer.ControlServer.ServerFaultEvent += ControlServer_ServerFaultEvent;
            }
        }

        void ControlServer_ServerFaultEvent(object sender, TransmissionControlFaultEventArgs e)
        {
            switch(e.Result)
            {
                case TransmissionControlResult.OverTime:
                case TransmissionControlResult.SendFault:
                case TransmissionControlResult.Fault:
                    {
                        if (allFlag)
                        {
                            ExecuteStopTcpLink("");//随后断开TCP连接
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// 服务信息到来
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ServerInformation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// 通讯服务
        /// </summary>
        /// <param name="obj"></param>
        private void ExecuteCommunicationServer(CommunicationServer obj)
        {
            if (obj != null)
            {
                serverData = obj.NetParameter;
                serverData.PropertyChanged += ServerInformation_PropertyChanged;
                netServer = obj;                
                netServer.NetClient.LinkingEventMsg += NetServer_LinkingEventMsg;             
                
            }
        }

        void ControlServer_ServerEvent(object sender, TransmissionControlEventArgs e)
        {
            switch(e.ControlFunction)
            {
                case TransmissionControlFunction.AcknowledgementStartDataTransmission:
                    {
                        if (allFlag)
                        {
                            ExecuteCallAll("");//随后启动总召唤
                        }
                        break;
                    }
                case TransmissionControlFunction.AcknowledgementStopDataTransmission:
                    {
                        if (allFlag)
                        {
                            ExecuteStopTcpLink("");//随后断开TCP连接
                        }
                        break;
                    }
            }
        }

      

        void NetServer_LinkingEventMsg(object sender, Net.Element.NetLinkMessagEvent e)
        {
            
            switch(e.State)
            {
                case Net.Element.NetState.Stop:
                    {
                        break;
                    }
                case Net.Element.NetState.EstablishLink:
                    {
                        if (allFlag)
                        {
                            ExecuteStartTransmission(""); //建立连接后，随后启动数据传输
                        }
                        break;
                    }

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
            
        }
        #endregion


        #region 通讯属性与服务
        private NetParameter serverData;

        //public NetParameter ServerInformation
        //{
        //   get
        //    {
        //        if (serverData == null)
        //        {
        //            serverData = new NetParameter();
        //        }
        //        return serverData;
        //    }
        //    set
        //    {
        //        serverData = value;
        //        RaisePropertyChanged("ServerInformation");
        //    }
        //}
        private void InitserverData()
        {
             if (serverData == null)
             {
                 serverData = new NetParameter();
             }
        }
        /// <summary>
        /// 获取或设置IP地址
        /// </summary>
        public string IpAddress
        {
            get
            {
                InitserverData(); 
                return serverData.IpAddress; 
            }
            set
            {
                IPAddress iptry;
                if (IPAddress.TryParse(value, out  iptry))
                {
                    serverData.IP = iptry;
                    RaisePropertyChanged("IpAddress");

                }


            }
        }

        /// <summary>
        /// 获取或设置端口号
        /// </summary>
        public int Port
        {
            get
            {
                InitserverData();
                return serverData.Port;
            }
            set
            {
                serverData.Port = value;
                RaisePropertyChanged("Port");

            }
        }

       

        /// <summary>
        /// 连接信息
        /// </summary>
        public string LinkMessage
        {
            get
            {
                InitserverData(); 
                return serverData.LinkMessage;
            }
            set
            {
                serverData.LinkMessage = value;
                RaisePropertyChanged("LinkMessage");

            }
        }

        /// <summary>
        /// 通讯服务
        /// </summary>
        private CommunicationServer netServer;
        /// <summary>
        /// 启动TCP连接
        /// </summary>
        public RelayCommand<string> StartTcpLink { get; private set; }

        
        void ExecuteStartTcpLink(string name)
        {
            try
            {
                netServer.NetClient.StartServer(serverData.IP, serverData.Port);
                
            }
            catch(Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }

        /// <summary>
        /// 停止Tcp连接
        /// </summary>
        public RelayCommand<string> StopTcpLink { get; private set; }


        void ExecuteStopTcpLink(string name)
        {
            try
            {
                netServer.NetClient.Stop();

            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }
        #endregion

        #region 传输控制功能
        /// <summary>
        /// 启动传输控制功能
        /// </summary>
        public RelayCommand<string> StartTransmission { get; private set; }


        void ExecuteStartTransmission(string str)
        {
            try
            {
                protocolServer.ControlServer.StartServer(TransmissionControlFunction.StartDataTransmission);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }

        /// <summary>
        /// 停止传输控制功能
        /// </summary>
        public RelayCommand<string> StopTransmission { get; private set; }


        void ExecuteStopTransmission(string str)
        {
            try
            {
                protocolServer.ControlServer.StartServer(TransmissionControlFunction.StopDataTransmission);
                protocolServer.ResetServer();

            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }
        #endregion

        #region 总召唤
        /// <summary>
        /// 总召唤
        /// </summary>
        public RelayCommand<string> CallAll { get; private set; }


        void ExecuteCallAll(string str)
        {
            try
            {
                protocolServer.CallServer.StartServer(CauseOfTransmissionList.Activation, QualifyOfInterrogationList.GeneralInterrogation);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }
        #endregion

        #region ClearText
        public RelayCommand<string> ClearText { get; private set; }

        void ExecuteClearText(string name)
        {
            try
            {
                LinkMessage = "";
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }
        #endregion
        #region ClearText
        public RelayCommand<string> ToEnd { get; private set; }

        void ExecuteToEnd(string name)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }
        #endregion
        /// <summary>
        /// 启动所有连接
        /// </summary>
        public RelayCommand<string> StartAllLink { get; private set; }


        void ExecuteStartAllLink(string name)
        {
            try
            {
                allFlag = true;
                ExecuteStartTcpLink("");

            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }
        /// <summary>
        /// 停止所有连接
        /// </summary>
        public RelayCommand<string> StopAllLink { get; private set; }


        void ExecuteStopAllLink(string name)
        {
            try
            {
                allFlag = true;
                if (netServer.NetClient.IsRun)
                {
                    ExecuteStopTransmission("");
                }
                else
                {
                    protocolServer.ResetServer();
                }
               

                
                

            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }
    }
}