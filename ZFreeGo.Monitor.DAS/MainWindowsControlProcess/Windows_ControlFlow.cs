using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.Monitor.AutoStudio
{
    public partial class MainWindow 
    {
        /// <summary>
        /// 网络跨线程启动
        /// </summary>
        private void UPNPStartServerThread()
        {
            Action myDelegate = UPNPStartServer;
            Dispatcher.BeginInvoke(myDelegate);
        }
        /// <summary>
        /// 发送STARTDT指令
        /// </summary>
        private void SendSTARTDT()
        {
            SendTCFCommand(TransmissionCotrolFunction.StartDataTransmission);
        }
        /// <summary>
        /// 发送召唤指令
        /// </summary>
        private void SendInterrogationCommand()
        {
            SendMasterCommand(CauseOfTransmissionList.Activation, QualifyOfInterrogationList.GeneralInterrogation);
        }
       /// <summary>
       /// 发送监控命令
       /// </summary>
        private void SupervisoryCommand()
        {
            SendSupervisoryFrame(appMessageManager.RealReceiveSequenceNumber);
        }
    }
}
