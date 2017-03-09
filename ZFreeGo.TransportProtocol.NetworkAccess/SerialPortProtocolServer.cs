using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.ControlProcessInformation;
using ZFreeGo.TransmissionProtocols.ControlSystemCommand;
using ZFreeGo.TransmissionProtocols.FileSever;
using ZFreeGo.TransmissionProtocols.Frame;
using ZFreeGo.TransmissionProtocols.Frame104;
using ZFreeGo.TransmissionProtocols.MonitorProcessInformation;
using ZFreeGo.TransmissionProtocols.ReciveCenter;
using ZFreeGo.TransmissionProtocols.TransmissionControl104;

namespace ZFreeGo.TransmissionProtocols
{




    /// <summary>
    /// 网络访问服务
    /// </summary>
    public class SerialPortProtocolServer
    {

        /// <summary>
        /// 协议接收故障信息
        /// </summary>
        public event EventHandler<ProtocolServerFaultArgs> ReciveFaltEvent;

        /// <summary>
        /// 接收帧信息事件参数
        /// </summary>
        public event EventHandler<FrameMessageEventArgs> ReciveFrameMessageEvent;
        /// <summary>
        /// 发送帧信息事件参数
        /// </summary>
        public event EventHandler<FrameMessageEventArgs> SendFrameMessageEvent;


        /// <summary>
        /// 发送委托
        /// </summary>
        private Func<byte[], bool> sendDelegate;

        /// <summary>
        ///检测提取帧信息
        /// </summary>
        private CheckLinkData checkGetMessage;

        /// <summary>
        /// 信息提取
        /// </summary>
        public  CheckLinkData CheckGetMessage
        {
            get
            {
                return checkGetMessage;
            }
        }
            

        /// <summary>
        /// 传输协议应用管理
        /// </summary>
        private ApplicationFrameManager appMessageManager;

        /// <summary>
        /// 传输控制服务
        /// </summary>
        private TransmissionControlServer transmissionControlServer;

        /// <summary>
        /// 获取传输控制服务
        /// </summary>
        public  TransmissionControlServer ControlServer
        {
            get
            {
                return transmissionControlServer;
            }
        }

        /// <summary>
        /// 召唤服务
        /// </summary>
       private CallServer callServer;

        /// <summary>
        /// 获取召唤服务
        /// </summary>
        public CallServer CallServer
       {
           get
           {
               return callServer;
           }
       }
        /// <summary>
        /// 遥信，SOE，事件记录 
        /// </summary>
        private StatusServer telesignalisationServer;

        /// <summary>
        /// 获取状态更新服务 遥信，SOE，事件记录 
        /// </summary>
        public StatusServer TelesignalisationServer
        {
            get
            {
                return telesignalisationServer;
            }
        }

        /// <summary>
        /// 遥测服务
        /// </summary>
        private MeteringServer meteringServer;

        /// <summary>
        /// 获取遥测服务
        /// </summary>
        public MeteringServer MeteringServer
        {
            get
            {
                return meteringServer;
            }
        }
        /// <summary>
        /// 设定值服务
        /// </summary>
        private SetPointServer setpointServer;
        /// <summary>
        /// 获取保护定值服务
        /// </summary>
        public SetPointServer SetPointServer
        {
            get
            {
                return setpointServer;
            }
        }

        /// <summary>
        /// 遥控服务
        /// </summary>
        private ControlServer telecontrolServer;

        /// <summary>
        /// 获取遥控服务
        /// </summary>
        public ControlServer TelecontrolServer
        {
            get
            {
                return telecontrolServer;
            }
        }

        /// <summary>
        /// 时间服务
        /// </summary>
        private TimeSynchronizationServer timeServer;

        /// <summary>
        /// 获取事件服务
        /// </summary>
        public TimeSynchronizationServer TimeServer
        {
            get
            {
                return timeServer;
            }
        }

        /// <summary>
        /// 电能脉冲召唤服务
        /// </summary>
        private ElectricPulseCallServer electricPulse;

        /// <summary>
        /// 获取电能脉冲召唤服务
        /// </summary>
        public ElectricPulseCallServer ElectricPulse
        {
            get
            {
                return electricPulse;
            }
        }

        /// <summary>
        /// 召唤目录文件服
        /// </summary>
        private CallFileDirectoryServer callFileDirectory;

        /// <summary>
        /// 获取召唤目录文件服
        /// </summary>
        public CallFileDirectoryServer CallFileDirectory
        {
            get
            {
                return callFileDirectory;
            }
        }
        /// <summary>
        /// 文件读服务
        /// </summary>
        private FileReadServer fileRead;

        /// <summary>
        /// 获取文件度服务
        /// </summary>
        public FileReadServer FileRead
        {
            get
            {
                return fileRead; ;
            }
        }
        /// <summary>
        /// 文件写服务
        /// </summary>
        private FileWriteServer fileWrite;

        /// <summary>
        /// 获取文件写服务
        /// </summary>
        public FileWriteServer FileWrite
        {
            get
            {
                return fileWrite;
            }
        }

        /// <summary>
        /// 访问服务初始化
        /// </summary>
        public SerialPortProtocolServer(Func<byte[], bool> inSsendDelegate)
        {
            sendDelegate = inSsendDelegate;
            appMessageManager = new ApplicationFrameManager();
            
            transmissionControlServer = new TransmissionControlServer(NetSendData);
            callServer = new CallServer(NetSendData);
            telesignalisationServer = new StatusServer();
            meteringServer = new MonitorProcessInformation.MeteringServer();
            setpointServer = new ControlProcessInformation.SetPointServer(NetSendData);
            telecontrolServer = new ControlProcessInformation.ControlServer(NetSendData);
            timeServer = new TimeSynchronizationServer(NetSendData);
            electricPulse = new ElectricPulseCallServer(NetSendData);

            callFileDirectory = new CallFileDirectoryServer(NetSendData);
            fileRead = new FileReadServer(NetSendData);
            fileWrite = new FileWriteServer(NetSendData);

            checkGetMessageServerConfig();
        }

      
        /// <summary>
        /// 停止服务
        /// </summary>
        public void StopServer()
        {
            checkGetMessage.Close();

            transmissionControlServer.StopServer();
            callServer.StopServer();
            telesignalisationServer.StopServer();
            meteringServer.StopServer();
         
        }
        /// <summary>
        /// 复位服务
        /// </summary>
        public void ResetServer()
        {
            appMessageManager = new ApplicationFrameManager();

        }

        /// <summary>
        /// 静态服务启动，控制传输功能
        /// </summary>
        private void NormalServerConfig()
        {
            
            
        }


        private void checkGetMessageServerConfig()
        {
            checkGetMessage = new CheckLinkData();          
         
            //U-TCF
           // checkGetMessage.TransmitControlCommandArrived += checkGetMessage_TransmitControlCommandArrived;
            //S-Type
            //checkGetMessage.SupervisoryCommandArrived += checkGetMessage_SupervisoryCommandArrived;
            //I-主站控制
            //checkGetMessage.MasterInitializeArrived += checkGetMessage_MasterInitializeArrived;
            //checkGetMessage.MasterInterrogationArrived += checkGetMessage_MasterInterrogationArrived;
            //checkGetMessage.MasterResetArrived += checkGetMessage_MasterResetArrived;
            //checkGetMessage.MasterTimeArrived += checkGetMessage_MasterTimeArrived;

            //I-遥控/遥信/遥测
            checkGetMessage.TelecontrolCommandArrived += checkGetMessage_TelecontrolCommandArrived;
            checkGetMessage.TelemeteringMessageArrived += checkGetMessage_TelemeteringMessageArrived;
            checkGetMessage.TelesignalisationMessageArrived += checkGetMessage_TelesignalisationMessageArrived;

            //电能脉冲
            checkGetMessage.ElectricEnergyArrived += checkGetMessage_ElectricEnergyArrived;

            //保护定值
            checkGetMessage.ProtectSetMessageArrived += checkGetMessage_ProtectSetMessageArrived;
            //文件传输
            checkGetMessage.FileServerArrived += checkGetMessage_FileServerArrived;
            //I-未知
            checkGetMessage.UnknowMessageArrived += checkGetMessage_UnknowMessageArrived;

           
        }

       

        /// <summary>
        ///U帧 TCF传输控制功能事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkGetMessage_TransmitControlCommandArrived(
            object sender, TransmitEventArgs<APCITypeU, byte[]> e)
        {
            try
            {
               
                MakeReciveFrameMessageEvent(e.mdata1.ToString(), e.mdata1.ToString(true));
                if(e.mdata1.TransmissionCotrolFun == TransmissionControlFunction.TestFrame)
                {
                    var apci = new APCITypeU(TransmissionControlFunction.AcknowledgementTestFrame);
                    NetSendData(apci);
                }
                else
                {
                    transmissionControlServer.Enqueue(e.mdata1);
                }
                
                
            }
            catch (Exception ex)
            {
                SendFaultEvent(ex.Message);
            }
        }


        /// <summary>
        /// S帧，监控数据到达
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkGetMessage_SupervisoryCommandArrived(object sender, TransmitEventArgs<APCITypeS, byte[]> e)
        {
            try
            {
                MakeReciveFrameMessageEvent(e.mdata1.ToString(), e.mdata1.ToString(true));
                appMessageManager.AckReceiveSequenceNumber(e.mdata1.ReceiveSequenceNumber);



            }
            catch (Exception ex)
            {
                SendFaultEvent("checkGetMessage_SupervisoryCommandArrived:" + ex.Message);
            }
        }
        /// <summary>
        /// 发送APCI—S 监控帧
        /// </summary>
        private void SendSupervisoryFrame()
        {
            //try
            //{
            //    if (appMessageManager.RealReceiveSequenceNumber > appMessageManager.MaxNoReciveAckNum)
            //    {
            //        var frame = new APCITypeS(appMessageManager.RealReceiveSequenceNumber);
            //        NetSendData(frame);
            //        appMessageManager.NoReciveAckNum = 0;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    SendFaultEvent("SendSupervisoryFrame:" + ex.Message);

            //}



        }


        
    


        /// <summary>
        /// 主站控制命令，主站复位命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkGetMessage_MasterResetArrived(object sender, MasterCommmadEventArgs e)
        {
            try
            {
                appMessageManager.UpdateReceiveSequenceNumber(e.MasterCMD.APCI.TransmitSequenceNumber,
                    e.MasterCMD.APCI.ReceiveSequenceNumber);

                SendSupervisoryFrame();
            }
            catch (Exception ex)
            {
                SendFaultEvent("checkGetMessage_MasterResetArrived:" + ex.Message);
            }
        }
        /// <summary>
        /// 主站控制命令，召唤命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkGetMessage_MasterInterrogationArrived(object sender, MasterCommmadEventArgs e)
        {
            try
            {
                appMessageManager.UpdateReceiveSequenceNumber(e.MasterCMD.APCI.TransmitSequenceNumber,
                    e.MasterCMD.APCI.ReceiveSequenceNumber);

                MakeReciveFrameMessageEvent(e.MasterCMD.ToString(), e.MasterCMD.ToString(true));
                callServer.Enqueue(e.MasterCMD.ASDU);

                SendSupervisoryFrame();

            }
            catch (Exception ex)
            {
                SendFaultEvent("checkGetMessage_MasterInterrogationArrived:" + ex.Message);
            }
        }
        /// <summary>
        /// 主站控制命令，初始化命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkGetMessage_MasterInitializeArrived(object sender, MasterCommmadEventArgs e)
        {
            try
            {
                appMessageManager.UpdateReceiveSequenceNumber(e.MasterCMD.APCI.TransmitSequenceNumber,
                    e.MasterCMD.APCI.ReceiveSequenceNumber);
                
               // BeginInvokeUpdateHistory(e.MasterCMD.FrameArray, e.MasterCMD.FrameArray.Length, "从站发送:I帧：主站初始化:");

            }
            catch (Exception ex)
            {
                SendFaultEvent("checkGetMessage_MasterInitializeArrived:" + ex.Message);
            }
        }




      

        /// <summary>
        /// 未知ID命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkGetMessage_UnknowMessageArrived(object sender, TransmitEventArgs<TypeIdentification, byte[]> e)
        {
            SendFaultEvent("checkGetMessage_UnknowMessageArrived");
        }
        /// <summary>
        /// 遥信信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkGetMessage_TelesignalisationMessageArrived(object sender, TransmitEventArgs<TypeIdentification, APDU> e)
        {
            try
            {
                appMessageManager.UpdateReceiveSequenceNumber(e.mdata2.APCI.TransmitSequenceNumber,
                    e.mdata2.APCI.ReceiveSequenceNumber);
                telesignalisationServer.Enqueue(e.mdata2.ASDU);
                MakeReciveFrameMessageEvent(e.mdata2.ToString(), e.mdata2.ToString(true));

                SendSupervisoryFrame();
            }
            catch (Exception ex)
            {
                SendFaultEvent("checkGetMessage_TelesignalisationMessageArrived:" + ex.Message);
            }

        }
        /// <summary>
        /// 遥测命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkGetMessage_TelemeteringMessageArrived(object sender, TransmitEventArgs<TypeIdentification, APDU> e)
        {
            try
            {
                appMessageManager.UpdateReceiveSequenceNumber(e.mdata2.APCI.TransmitSequenceNumber,
                   e.mdata2.APCI.ReceiveSequenceNumber);
                meteringServer.Enqueue(e.mdata2.ASDU);
                MakeReciveFrameMessageEvent(e.mdata2.ToString(), e.mdata2.ToString(true));
                SendSupervisoryFrame();
            }
            catch (Exception ex)
            {
                SendFaultEvent("checkGetMessage_TelemeteringMessageArrived:" + ex.Message);
            }
        }
        /// <summary>
        /// 遥控命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkGetMessage_TelecontrolCommandArrived(object sender, TransmitEventArgs<TypeIdentification, APDU> e)
        {
            try
            {      
                
                appMessageManager.UpdateReceiveSequenceNumber(e.mdata2.APCI.TransmitSequenceNumber,
                    e.mdata2.APCI.ReceiveSequenceNumber);
                telecontrolServer.Enqueue(e.mdata2.ASDU);
                MakeReciveFrameMessageEvent(e.mdata2.ToString(), e.mdata2.ToString(true));
                SendSupervisoryFrame();
            }
            catch (Exception ex)
            {
                SendFaultEvent("checkGetMessage_TelecontrolCommandArrived:" + ex.Message);
            }
        }
        /// <summary>
        /// 电能脉冲 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkGetMessage_ElectricEnergyArrived(object sender, TransmitEventArgs<TypeIdentification, APDU> e)
        {
            try
            {
                appMessageManager.UpdateReceiveSequenceNumber(e.mdata2.APCI.TransmitSequenceNumber,
                   e.mdata2.APCI.ReceiveSequenceNumber);
               
                electricPulse.Enqueue(e.mdata2.ASDU);
                MakeReciveFrameMessageEvent(e.mdata2.ToString(), e.mdata2.ToString(true));
                SendSupervisoryFrame();
            }
            catch (Exception ex)
            {
                SendFaultEvent("checkGetMessage_ElectricEnergyArrived:" + ex.Message);
            }
        }


        /// <summary>
        /// 保护定值设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkGetMessage_ProtectSetMessageArrived(object sender, TransmitEventArgs<TypeIdentification, APDU> e)
        {
            try
            {
                appMessageManager.UpdateReceiveSequenceNumber(e.mdata2.APCI.TransmitSequenceNumber,
                   e.mdata2.APCI.ReceiveSequenceNumber);
                setpointServer.Enqueue(e.mdata2.ASDU);
                MakeReciveFrameMessageEvent(e.mdata2.ToString(), e.mdata2.ToString(true));
                SendSupervisoryFrame();
            }
            catch (Exception ex)
            {
                SendFaultEvent("checkGetMessage_ProtectSetMessageArrived:" + ex.Message);
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
                appMessageManager.UpdateReceiveSequenceNumber(e.MasterCMD.APCI.TransmitSequenceNumber,
                    e.MasterCMD.APCI.ReceiveSequenceNumber);
                MakeReciveFrameMessageEvent(e.MasterCMD.ToString(), e.MasterCMD.ToString(true));
                timeServer.Enqueue(e.MasterCMD.ASDU);
                SendSupervisoryFrame();
            }
            catch (Exception ex)
            {
                SendFaultEvent("checkGetMessage_MasterTimeArrived" + ex.Message);
            }
        }
        /// <summary>
        /// 文件服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkGetMessage_FileServerArrived(object sender, TransmitEventArgs<TypeIdentification, APDU> e)
        {
            try
            {
                appMessageManager.UpdateReceiveSequenceNumber(e.mdata2.APCI.TransmitSequenceNumber,
                    e.mdata2.APCI.ReceiveSequenceNumber);
                MakeReciveFrameMessageEvent(e.mdata2.ToString(), e.mdata2.ToString(true));
                switch (e.mdata2.ASDU.InformationObject[3])
                {
                    case 1:
                    case 2: //召唤文件目录服务
                        {
                            callFileDirectory.Enqueue(e.mdata2.ASDU);
                            break;
                        }
                    case 3:
                    case 4:
                    case 5:
                    case 6: //读文件服务
                        {
                            fileRead.Enqueue(e.mdata2.ASDU);
                            break;
                        }
                    case 7:
                    case 8:
                    case 9:
                    case 10: //写文件服务
                        {
                            fileWrite.Enqueue(e.mdata2.ASDU);
                            break;
                        }

                }
                SendSupervisoryFrame();
            }
            catch (Exception ex)
            {
                SendFaultEvent("checkGetMessage_FileServerArrived" + ex.Message);
            }
        }
     
        /// <summary>
        /// 发送故障信息
        /// </summary>
        /// <param name="comment">注释</param>
        private void SendFaultEvent(string comment)
        {
            if (ReciveFaltEvent != null)
            {
                ReciveFaltEvent(Thread.CurrentThread, new ProtocolServerFaultArgs(comment));
            }
        }

        /// <summary>
        /// 产生接收帧信息事件
        /// </summary>
        /// <param name="rawStr">原始帧信息</param>
        /// <param name="comment">注释</param>
        private void MakeReciveFrameMessageEvent(string rawStr, string comment)
        {
            if(ReciveFrameMessageEvent != null)            
            {              
                ReciveFrameMessageEvent(Thread.CurrentThread, new FrameMessageEventArgs(rawStr, comment));
            }
        }
        /// <summary>
        /// 产生发送帧信息事件
        /// </summary>
        /// <param name="rawStr">原始帧信息</param>
        /// <param name="comment">注释</param>
        private void MakeSendFrameMessageEvent(string rawStr, string comment)
        {
            if (SendFrameMessageEvent != null)
            {
                SendFrameMessageEvent(Thread.CurrentThread, new FrameMessageEventArgs(rawStr, comment));
            }
        }

        /// <summary>
        /// 数组转字符串,不带换行结束符
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string NumToString(byte[] data)
        {
            StringBuilder strbuild = new StringBuilder(data.Length * 2 + 10);
            foreach (var m in data)
            {
                strbuild.AppendFormat("{0:X2} ", m);
            }
            return strbuild.ToString();
        }
        /// <summary>
        /// 网络发送数据
        /// </summary>
        /// <param name="apci">APCI S</param>
        /// <returns>true-发送成,false--失败</returns>
        private bool NetSendData(APCITypeS apci)
        {
            var data = apci.GetAPCIDataArray();
            if (sendDelegate(data))
            {
                var rawStr = NumToString(data);
                MakeSendFrameMessageEvent(rawStr, apci.ToString(true));
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 网络发送数据
        /// </summary>
        /// <param name="apci">APCI U</param>
        /// <returns>true-发送成,false--失败</returns>
        private bool NetSendData(APCITypeU apci)
        {
           var data = apci.GetAPCIDataArray();
           if( sendDelegate(data))
           {
               var rawStr = NumToString(data);
               MakeSendFrameMessageEvent(rawStr, apci.ToString(true));
               return true;
           }
           else
           {
               return false;
           }

        }
        /// <summary>
        /// 网络发送数据
        /// </summary>
        /// <param name="asdu">控制方向系统命令 ASDU</param>
        /// <returns>true-发送成,false--失败</returns>
        private bool NetSendData(ControlCommandASDU asdu)
        {
            var apdu = PackASDUToAPDU(asdu);

            var data = apdu.GetAPDUDataArray();
            if (sendDelegate(data))
            {
                var rawStr = NumToString(data);
                MakeSendFrameMessageEvent(rawStr, apdu.ToString(true));
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 网络发送数据
        /// </summary>
        /// <param name="asdu">控制方向系统命令 ASDU</param>
        /// <returns>true-发送成,false--失败</returns>
        private bool NetSendData(ControlProcessASDU asdu)
        {
            var apdu = PackASDUToAPDU(asdu);

            var data = apdu.GetAPDUDataArray();
            if (sendDelegate(data))
            {
                var rawStr = NumToString(data);
                MakeSendFrameMessageEvent(rawStr, apdu.ToString(true));
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// 网络发送数据
        /// </summary>
        /// <param name="asdu">文件传输 ASDU</param>
        /// <returns>true-发送成,false--失败</returns>
        private bool NetSendData(FileASDU asdu)
        {
            var apdu = PackASDUToAPDU(asdu);

            var data = apdu.GetAPDUDataArray();
            if (sendDelegate(data))
            {
                var rawStr = NumToString(data);
                MakeSendFrameMessageEvent(rawStr, apdu.ToString(true));
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// ASDU打包生成APUD
        /// </summary>
        /// <param name="asdu">ASDU</param>
        /// <returns>APDU</returns>
        private APDU PackASDUToAPDU(ApplicationServiceDataUnit asdu)
        {
            var apci = new APCITypeI( (byte)(asdu.Length + 4), appMessageManager.TransmitSequenceNumber,
                appMessageManager.RealReceiveSequenceNumber);
          
            asdu.AppDataPublicAddress = appMessageManager.ASDUADdress;
            var apdu = new APDU(apci, asdu);
            return apdu;
        }

       

        
    }
}
