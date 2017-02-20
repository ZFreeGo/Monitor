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

namespace ZFreeGo.Monitor.DASII.ViewModel
{
   
    public class CommunicationViewModel : ViewModelBase
    {
        private NetWorkProtocolServer protocolServer;

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
            ServerInformation = new NetParameter();
            StartTransmission = new RelayCommand<string>(ExecuteStartTransmission);
            StopTransmission = new RelayCommand<string>(ExecuteStopTransmission);
            CallAll = new RelayCommand<string>(ExecuteCallAll);
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
            }
        }
        /// <summary>
        /// 服务信息到来
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ServerInformation_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged("ServerInformation");
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
                netServer.NetClient.ExceptionEventArrived += NetServer_ExceptionEventArrived;
                netServer.NetClient.LinkingEventMsg += NetServer_LinkingEventMsg;
                netServer.NetClient.NetDataEventArrived += NetServer_NetDataEventArrived;

            }
        }

        void NetServer_NetDataEventArrived(object sender, Net.Element.NetDataArrayEventArgs e)
        {
           
        }

        void NetServer_LinkingEventMsg(object sender, Net.Element.NetLinkMessagEvent e)
        {
            ServerInformation.LinkMessage += "\n";
            ServerInformation.LinkMessage += e.Message;
            
        }

        void NetServer_ExceptionEventArrived(object sender, Net.Element.NetExcptionEventArgs e)
        {

            ServerInformation.LinkMessage += "\n";
            ServerInformation.LinkMessage += e.OriginException.Message;
            ServerInformation.LinkMessage += "\n";
            ServerInformation.LinkMessage += e.OriginException.StackTrace;
            ServerInformation.LinkMessage += "\n";
            ServerInformation.LinkMessage += e.Comment;
            
           
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

        public NetParameter ServerInformation
        {
           get
            {
                return serverData;
            }
            set
            {
                serverData = value;
                RaisePropertyChanged("ServerInformation");
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
                protocolServer.ControlServer.SendTransmissonCommand(TransmissionControlFunction.StartDataTransmission);
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
                protocolServer.ControlServer.SendTransmissonCommand(TransmissionControlFunction.StopDataTransmission);
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
    }
}