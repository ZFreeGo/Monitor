using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZFreeGo.Monitor.DASModel.DataItemSet;
using ZFreeGo.Monitor.DASModel.Helper;
using ZFreeGo.TransmissionProtocols;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.TransmissionControl104;

namespace ZFreeGo.Monitor.DASModel
{
    /// <summary>
    /// 提供支撑DASII数据逻辑服务
    /// </summary>
    public class DASModelServer
    {
        /// <summary>
        /// 数据文件服务
        /// </summary>
        public DataFileServer DataFile
        {
            get;
            private set;
        }

        /// <summary>
        /// 通讯服务
        /// </summary>
        public CommunicationServer Communication
        {
            get;
            private set;
        }

        private NetWorkProtocolServer protocolServer;
        /// <summary>
        /// 协议服务
        /// </summary>
        public NetWorkProtocolServer ProtocolServer
        {
            get
            {
                return protocolServer;
            }
            
        }


        /// <summary>
        /// 定制服务校准
        /// </summary>
        private OldCalibration customServer;

        public OldCalibration CustomServer
        {
            get
            {
                return customServer;
            }

        } 
            
            /// <summary>
        ///DASII Model服务
        /// </summary>
        public  DASModelServer()
        {
            DataFile = new DataFileServer();
            Communication = new CommunicationServer();
            Communication.NetClient.NetDataEventArrived += NetClient_NetDataEventArrived;
            Communication.NetClient.LinkingEventMsg += NetClient_LinkingEventMsg;
            Communication.NetClient.ExceptionEventArrived += NetServer_ExceptionEventArrived;
            protocolServer = new NetWorkProtocolServer(NetSendData);

            protocolServer.ReciveFaltEvent += protocolServer_ReciveFaltEvent;
            protocolServer.SendFrameMessageEvent += protocolServer_SendFrameMessageEvent;
            protocolServer.ReciveFrameMessageEvent += protocolServer_ReciveFrameMessageEvent;
            //传输控制服务
            protocolServer.ControlServer.ServerEvent += ControlServer_ServerEvent;
            protocolServer.ControlServer.ServerFaultEvent += ControlServer_ServerFaultEvent; ;
            //SOE,事件记录，故障信息
            protocolServer.TelesignalisationServer.SOEStatusEvent += TelesignalisationServer_SOEStatusEvent;
            protocolServer.TelesignalisationServer.StatusUpdateEvent += TelesignalisationServer_StatusUpdateEvent;
            protocolServer.TelesignalisationServer.EventLogEvent +=TelesignalisationServer_EventLogEvent;
            //遥测，电能脉冲，带有时标的电能脉冲信息
            protocolServer.MeteringServer.TelemeteringEvent += MeteringServer_TelemeteringEvent;
            protocolServer.MeteringServer.ElectricPulseEvent += MeteringServer_ElectricPulseEvent;
            protocolServer.MeteringServer.ElectricPulseWithTimeEvent += MeteringServer_ElectricPulseWithTimeEvent;
            protocolServer.MeteringServer.StatusFaultEvent += MeteringServer_StatusFaultEvent;
            //保护定值服务
            protocolServer.SetPointServer.ServerEvent += SetPointServer_ServerEvent;

            //遥控服务
            protocolServer.TelecontrolServer.ServerEvent += TelecontrolServer_ServerEvent;
            //事件服务
            protocolServer.TimeServer.ServerEvent += TimeServer_ServerEvent;
            //文件服务
            protocolServer.CallFileDirectory.CallFileEndEvent += CallFileDirectory_CallFileEndEvent;
            protocolServer.FileRead.ReadFileEndEvent += FileRead_ReadFileEndEvent;
            protocolServer.FileRead.ProcessMessageEvent += FileRead_ProcessMessageEvent;
            protocolServer.FileWrite.ProcessMessageEvent += FileWrite_ProcessMessageEvent;



            //校准服务
            Communication.NetCustomClient.NetDataEventArrived += NetCustomClient_NetDataEventArrived;
            Communication.NetCustomClient.ExceptionEventArrived += NetCustomClient_ExceptionEventArrived;
            Communication.NetCustomClient.LinkingEventMsg +=NetCustomClient_LinkingEventMsg;
            customServer = new OldCalibration();
            customServer.CalibrationMessageArrived += customServer_CalibrationMessageArrived;
            

        }

      
       

        #region 校准服务
        void NetCustomClient_ExceptionEventArrived(object sender, Net.Element.NetExcptionEventArgs e)
        {
            DataFile.StateMessage.AddCustomNetMessage(e.Comment + " \n" + e.OriginException.Message
                + "\n" + e.OriginException.StackTrace);

        }

        void NetCustomClient_NetDataEventArrived(object sender, Net.Element.NetDataArrayEventArgs e)
        {
            lock (customServer.ReciveQuene)
            {
                foreach (var m in e.DataArray)
                {
                    customServer.ReciveQuene.Enqueue(m);

                }
                Console.WriteLine("systemCailbration字节数据接收");

            }
            DataFile.StateMessage.AddCustomNetMessage(e.DataArray, false);
        }
        void customServer_CalibrationMessageArrived(object sender, OldCalibrationEventArgs e)
        {
            try
            {
                DataFile.StateMessage.AddCustomNetMessage(e.Data, true);
                switch (e.Property)
                {
                    case CalibrationAction.FactorCall: //召唤系数
                        {
                            DataFile.StateMessage.AddCustomNetMessage("召唤系数");
                            DataFile.MonitorData.UpdateCalibrationFact(e.DataFrame);
                            break;
                        }
                    case CalibrationAction.FactorDownload:
                        {
                            DataFile.StateMessage.AddCustomNetMessage("系数下载");
                            break;
                        }
                    case CalibrationAction.FactorFix:
                        {
                            DataFile.StateMessage.AddCustomNetMessage("系数固化");
                            break;
                        }

                }
               
            }
            catch (Exception ex)
            {
                DataFile.StateMessage.AddCustomNetMessage(ex.Message + "\n" + ex.StackTrace);
            }
        }
        private void NetCustomClient_LinkingEventMsg(object sender, Net.Element.NetLinkMessagEvent e)
        {           
            DataFile.StateMessage.AddCustomNetMessage(e.Message);
            switch (e.State)
            {
                case Net.Element.NetState.Stop:
                    {                        

                        break;
                    }
            }
        }

        #endregion





        #region 文件服务

        void CallFileDirectory_CallFileEndEvent(object sender, TransmissionProtocols.FileSever.CallFileEndEventArgs e)
        {
            Communication.NetParameter.LinkMessage += e.Message + "\n";
            DataFile.StateMessage.AddProtoclMessage(e.Message, false);
            
            //更新文件列表
            DataFile.MonitorData.UpdateDirectoryList(e.AttributeList);

        }
        void FileRead_ReadFileEndEvent(object sender, TransmissionProtocols.FileSever.FileReadEndEventArgs e)
        {
            Communication.NetParameter.LinkMessage += e.Message + "\n";
            DataFile.StateMessage.AddProtoclMessage(e.Message, false);
            //更新文件数据
           // e.PacketManager
            //将收到的文件写入测试文档。
            using(FileStream file= 
                new  FileStream("test" + DateTime.Now.ToString("HH-MM-SS") + ".data", FileMode.OpenOrCreate))
            {
                foreach(var m in e.PacketManager.PacketCollect)
                {
                    file.Write(m.FileData, 0, m.FileData.Length);
                }
                file.Flush();
                file.Close();
            }
        }
        void FileRead_ProcessMessageEvent(object sender, TransmissionProtocols.FileSever.FileEventArgs e)
        {
            Communication.NetParameter.LinkMessage += e.Comment + "\n";
            DataFile.StateMessage.AddProtoclMessage(e.Comment, false);
            if (e.EX != null)
            {
                DataFile.StateMessage.AddExcptionMessage(e.Comment, e.EX);
                Communication.NetParameter.LinkMessage += e.EX.Message + "\n";
                Communication.NetParameter.LinkMessage += e.EX.StackTrace + "\n";
            }
            switch(e.Result)
            {
                case TransmissionProtocols.FileSever.FileServerResut.Success:
                    {

                        break;
                    }
                case TransmissionProtocols.FileSever.FileServerResut.SendFault:
                    {
                        break;
                    }
                case TransmissionProtocols.FileSever.FileServerResut.Error:
                    {
                        break;
                    }
                case TransmissionProtocols.FileSever.FileServerResut.OverTime:
                    {
                        break;
                    }
                case TransmissionProtocols.FileSever.FileServerResut.Unknow:
                    {
                        break;
                    }

            }
        }
        void FileWrite_ProcessMessageEvent(object sender, TransmissionProtocols.FileSever.FileEventArgs e)
        {
            FileRead_ProcessMessageEvent(sender, e);
        }
        #endregion

        #region  遥测，电能脉冲，带有时标的电能脉冲信息

        void MeteringServer_TelemeteringEvent(object sender, TransmissionProtocols.MonitorProcessInformation.StatusEventArgs<List<Tuple<uint, float, QualityDescription>>> e)
        {
            //MessageBox.Show(e.Message.Count.ToString(), "遥测");
            DataFile.MonitorData.UpdateTelemeteringEvent(e);
            //检测并实时更新校准数据
            if (customServer.IsRealUpdate)
            {
                DataFile.MonitorData.UpdateCalbrationData(e.Message,ref customServer.UpdateIndex);
               // Console.WriteLine("更新索引{0}", customServer.UpdateIndex);
            }
        }
        

        void MeteringServer_ElectricPulseWithTimeEvent(object sender, TransmissionProtocols.MonitorProcessInformation.StatusEventArgs<List<Tuple<uint, float, QualityDescription, CP56Time2a>>> e)
        {
            DataFile.MonitorData.UpdateEletricMessageEvent(e);
        }

        void MeteringServer_ElectricPulseEvent(object sender, TransmissionProtocols.MonitorProcessInformation.StatusEventArgs<List<Tuple<uint, float, QualityDescription>>> e)
        {
            DataFile.MonitorData.UpdateEletricMessageEvent(e);
        }

        #endregion



        #region  时间
        private void TimeServer_ServerEvent(object sender, TransmissionProtocols.ControlSystemCommand.TimeEventArgs e)
        {
            DataFile.StateMessage.AddProtoclMessage(e.Comment);
           
        }
        #endregion

        #region 遥控 保护定值
        private void TelecontrolServer_ServerEvent(object sender, TransmissionProtocols.ControlProcessInformation.ControlEventArgs e)
        {           
           DataFile.StateMessage.AddProtoclMessage( e.Comment );
        }     


        /// <summary>
        /// 保护定值服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SetPointServer_ServerEvent(object sender, TransmissionProtocols.ControlProcessInformation.ControlEventArgs e)
        {           
            DataFile.StateMessage.AddProtoclMessage(e.Comment);
        }
        #endregion


        /// <summary>
        /// 关闭服务
        /// </summary>
        public void StopServer()
        {
            Communication.NetClient.Stop();
            protocolServer.StopServer();
            customServer.Close();

        }
        /// <summary>
        /// 连接信息事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NetClient_LinkingEventMsg(object sender, Net.Element.NetLinkMessagEvent e)
        {            
            Communication.NetParameter.LinkMessage += e.Message + "\n";
            DataFile.StateMessage.AddNetMessage(e.Message);
           switch(e.State)
           {
               case Net.Element.NetState.Stop:
                   {
                       protocolServer.ResetServer();
                      
                       break;
                   }
           }
        }
        #region SOE,事件记录，故障信息
        /// <summary>
        /// 事件记录信息--故障信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TelesignalisationServer_EventLogEvent(object sender,
            TransmissionProtocols.MonitorProcessInformation.EventLogEventArgs<Tuple<uint, byte, CP56Time2a>, Tuple<uint, float>> e)
        {
            DataFile.StateMessage.AddProtoclMessage(e.MeteringID.ToString() + "  " + e.StatusID.ToString());

        }
        void TelesignalisationServer_StatusUpdateEvent(object sender, TransmissionProtocols.MonitorProcessInformation.StatusEventArgs<List<Tuple<uint, byte>>> e)
        {           
            DataFile.MonitorData.UpdateStatusEvent(e);          
        }

        void TelesignalisationServer_SOEStatusEvent(object sender, TransmissionProtocols.MonitorProcessInformation.StatusEventArgs<List<Tuple<uint, byte, CP56Time2a>>> e)
        {        
                   
            DataFile.MonitorData.UpdateSOEEvent(e);
        }
        #endregion

        #region 传输控制故障信息
        /// <summary>
        /// 传输控制故障信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ControlServer_ServerFaultEvent(object sender, TransmissionControlFaultEventArgs e)
        {

            DataFile.StateMessage.AddExcptionMessage(e.Comment);
        }

        void ControlServer_ServerEvent(object sender, TransmissionControlEventArgs e)
        {
            Communication.NetParameter.LinkMessage += e.Comment + "\n";
            DataFile.StateMessage.AddProtoclMessage(e.Comment);
        }
        #endregion


        #region 内部解析状态
        /// <summary>
        /// 接收故障信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void protocolServer_ReciveFaltEvent(object sender, TransmissionProtocols.ReciveCenter.ProtocolServerFaultArgs e)
        {
            DataFile.StateMessage.AddExcptionMessage(e.Comment);
        }
        /// <summary>
        /// 网络连接故障信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NetServer_ExceptionEventArrived(object sender, Net.Element.NetExcptionEventArgs e)
        {

            Communication.NetParameter.LinkMessage += "\n";
            Communication.NetParameter.LinkMessage += e.OriginException.Message;
            Communication.NetParameter.LinkMessage += "\n";
            Communication.NetParameter.LinkMessage += e.OriginException.StackTrace;
            Communication.NetParameter.LinkMessage += "\n";
            Communication.NetParameter.LinkMessage += e.Comment;
            DataFile.StateMessage.AddExcptionMessage(e.Comment, e.OriginException);
        }

        void MeteringServer_StatusFaultEvent(object sender, TransmissionProtocols.MonitorProcessInformation.ProcessFaultEventArgs e)
        {
            Communication.NetParameter.LinkMessage += "\n";
            Communication.NetParameter.LinkMessage += e.EX.Message;
            Communication.NetParameter.LinkMessage += "\n";
            Communication.NetParameter.LinkMessage += e.EX.StackTrace;
            Communication.NetParameter.LinkMessage += "\n";
            Communication.NetParameter.LinkMessage += e.Comment;
            DataFile.StateMessage.AddExcptionMessage(e.Comment, e.EX);
        }


        #endregion
        private void protocolServer_SendFrameMessageEvent(object sender, TransmissionProtocols.ReciveCenter.FrameMessageEventArgs e)
        {
            Communication.NetParameter.LinkMessage += e.RawMessage + "\n";
            Communication.NetParameter.LinkMessage += e.Comment + "\n";

            DataFile.StateMessage.AddNetRawData(e.RawMessage, true);            
            DataFile.StateMessage.AddProtoclMessage(e.Comment, true);
        }

        void protocolServer_ReciveFrameMessageEvent(object sender, TransmissionProtocols.ReciveCenter.FrameMessageEventArgs e)
        {
            Communication.NetParameter.LinkMessage += e.RawMessage + "\n";           
            Communication.NetParameter.LinkMessage += e.Comment + "\n";
           
            DataFile.StateMessage.AddNetRawData(e.RawMessage, false);
            DataFile.StateMessage.AddProtoclMessage(e.Comment, false);
        }

      

    

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NetClient_NetDataEventArrived(object sender, Net.Element.NetDataArrayEventArgs e)
        {
            lock (protocolServer.CheckGetMessage.ReciveQuene)
            {
                foreach (var m in e.DataArray)
                {
                    protocolServer.CheckGetMessage.ReciveQuene.Enqueue(m);

                }
                Console.WriteLine("字节数据接收");
            }
        }

        /// <summary>
        /// 发送网络数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool NetSendData(byte[] data)
        {
            try
            {
                if (Communication.NetClient.IsRun)
                {
                    Communication.NetClient.SendMessage(data);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                DataFile.StateMessage.ExceptionMessage += ex.Message + "\n";
                DataFile.StateMessage.ExceptionMessage += ex.StackTrace + "\n\n";
                return false;
            }

        }


    }
}
