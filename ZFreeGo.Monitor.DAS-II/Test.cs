using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ZFreeGo.Monitor.AutoStudio.Log;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ApplicationMessage;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ControlSystemCommand;

namespace ZFreeGo.Monitor.AutoStudio
{
    public partial class MainWindow 
    {
        CallServer callServer;
        TimeSynchronizationServer timeServer;

        public void InitCallServer()
        {
            callServer = new CallServer();
            callServer.CallServerEvent += callServer_CallServerEvent;

            timeServer = new TimeSynchronizationServer();
            timeServer.TimerServerEvent += timeServer_TimerServerEvent;

        }

        private void timeServer_TimerServerEvent(object sender, TimeEventArgs e)
        {
            MessageBox.Show(e.Comment);
        }

        void callServer_CallServerEvent(object sender, CallEventArgs e)
        {
            MessageBox.Show(e.Comment);
        }

        private bool SendMessage(MasterCommand command)
        {
            if (isTcpRun)
            {
                command.APCI.TransmitSequenceNumber = appMessageManager.TransmitSequenceNumber;
                command.APCI.ReceiveSequenceNumber =  appMessageManager.RealReceiveSequenceNumber;
                command.ASDU.AppDataPublicAddress = appMessageManager.ASDUADdress;
                var data = command.GetAPDUDataArray();
               
                upnpServer.SendMessage(data);
                appMessageManager.UpdateTransmitSequenceNumber();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 主站初始化，时间同步命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkGetMessage_MasterTimeArrived(object sender, MasterCommmadEventArgs e)
        {
            try
            {
                //同步释放相应事件
                var m = eventTypeIDManager.GetEventProcess((TypeIdentification)e.MasterCMD.ASDU.TypeId);
                if (m != null)
                {
                    m.Event.Set();
                }
                appMessageManager.UpdateReceiveSequenceNumber(e.MasterCMD.APCI.TransmitSequenceNumber,
                    e.MasterCMD.APCI.ReceiveSequenceNumber);

                BeginInvokeUpdateHistory(e.MasterCMD.FrameArray, e.MasterCMD.FrameArray.Length, "从站发送:I帧：时间同步:");
                MakeLogMessage(sender, "", "时间同步应答" + e.MasterCMD.ToString(), LogType.Time);
                timeServer.Enqueue(e.MasterCMD);
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "checkGetMessage_MasterTimeArrived");
            }
        }
        


        public void StartCallServer()
        {
            callServer.StartServer(SendMessage, CauseOfTransmissionList.Activation, QualifyOfInterrogationList.GeneralInterrogation);
        }

        private void btnManualTestCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartCallServer();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "btnManualTestCall_Click");
            }
        }




        private void btnManualTime_Click(object sender, RoutedEventArgs e)
        {
            timeServer.StartServer(SendMessage, CauseOfTransmissionList.Activation, new CP56Time2a(DateTime.Now));
        }
    }
}
