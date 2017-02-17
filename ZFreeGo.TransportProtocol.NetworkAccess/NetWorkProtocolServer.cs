using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.ControlSystemCommand;
using ZFreeGo.TransmissionProtocols.FileSever;
using ZFreeGo.TransmissionProtocols.Frame;
using ZFreeGo.TransmissionProtocols.Frame104;
using ZFreeGo.TransmissionProtocols.ReciveCenter;
using ZFreeGo.TransmissionProtocols.TransmissionControl104;

namespace ZFreeGo.TransmissionProtocols
{




    /// <summary>
    /// 网络访问服务
    /// </summary>
    public class NetWorkProtocolServer
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
        private CheckGetMessage checkGetMessage;

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
        /// 网络访问服务初始化
        /// </summary>
        public  NetWorkProtocolServer(Func<byte[], bool> inSsendDelegate)
        {
            sendDelegate = inSsendDelegate;
            appMessageManager = new ApplicationFrameManager();

            transmissionControlServer = new TransmissionControlServer(NetSendData);
            callServer = new CallServer(NetSendData);

        }

        /// <summary>
        /// 静态服务启动，控制传输功能
        /// </summary>
        private void NormalServerConfig()
        {
            
            
        }


        private void checkGetMessageServerConfig()
        {
            checkGetMessage = new CheckGetMessage();          
         
            //U-TCF
            checkGetMessage.TransmitControlCommandArrived += checkGetMessage_TransmitControlCommandArrived;
            //S-Type
            checkGetMessage.SupervisoryCommandArrived += checkGetMessage_SupervisoryCommandArrived;
            //I-主站控制
            checkGetMessage.MasterInitializeArrived += checkGetMessage_MasterInitializeArrived;
            checkGetMessage.MasterInterrogationArrived += checkGetMessage_MasterInterrogationArrived;
            checkGetMessage.MasterResetArrived += checkGetMessage_MasterResetArrived;
            checkGetMessage.MasterTimeArrived += checkGetMessage_MasterTimeArrived;

            //I-遥控/遥信/遥测
            checkGetMessage.TelecontrolCommandArrived += checkGetMessage_TelecontrolCommandArrived;
            checkGetMessage.TelemeteringMessageArrived += checkGetMessage_TelemeteringMessageArrived;
            checkGetMessage.TelesignalisationMessageArrived += checkGetMessage_TelesignalisationMessageArrived;

            //电能脉冲
            checkGetMessage.ElectricEnergyArrived += checkGetMessage_ElectricEnergyArrived;

            //保护定值
            checkGetMessage.ProtectSetMessageArrived += checkGetMessage_ProtectSetMessageArrived;
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
                transmissionControlServer.Enqueue(e.mdata1);
                MakeReciveFrameMessageEvent(e.mdata1.ToString(), e.mdata1.ToString(true));
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
               
               
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message, "checkGetMessage_MasterResetArrived");
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
               // MessageBox.Show(ex.Message, "checkGetMessage_MasterInitializeArrived");
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
                //BeginInvokeUpdateHistory(e.mdata2.FrameArray, e.mdata2.FrameArray.Length, "从站发送:I帧:遥信命令:");

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "heckGetMessage_TelesignalisationMessageArrived");
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
              //  BeginInvokeUpdateHistory(e.mdata2.FrameArray, e.mdata2.FrameArray.Length, "从站发送:I帧:遥测命令");                                
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "遥测命令");
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
                var id = (TypeIdentification)e.mdata2.ASDU.TypeId;
                var cot = (CauseOfTransmissionList)e.mdata2.ASDU.CauseOfTransmission1;
                
                appMessageManager.UpdateReceiveSequenceNumber(e.mdata2.APCI.TransmitSequenceNumber,
                    e.mdata2.APCI.ReceiveSequenceNumber);
                //BeginInvokeUpdateHistory(e.mdata2.FrameArray, e.mdata2.FrameArray.Length, "从站发送:I帧:遥控命令:");

        
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "checkGetMessage_TelecontrolCommandArrived");
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
               // BeginInvokeUpdateHistory(e.mdata2.FrameArray, e.mdata2.FrameArray.Length, "从站发送:I帧:电能脉冲");
            


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "遥测命令");
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
                //BeginInvokeUpdateHistory(e.mdata2.FrameArray, e.mdata2.FrameArray.Length, "从站发送:I帧:校准参数");
               


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "保护定值设置");
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

                //BeginInvokeUpdateHistory(e.MasterCMD.FrameArray, e.MasterCMD.FrameArray.Length, "从站发送:I帧：时间同步:");
               


            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message, "checkGetMessage_MasterTimeArrived");
            }
        }
        /// <summary>
        /// 文件服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void checkGetMessage_FileServerArrived(object sender, TransmitEventArgs<TypeIdentification, FilePacket> e)
        {
            try
            {
                appMessageManager.UpdateReceiveSequenceNumber(e.mdata2.APCI.TransmitSequenceNumber,
                    e.mdata2.APCI.ReceiveSequenceNumber);
               // BeginInvokeUpdateHistory(e.mdata2.FrameArray, e.mdata2.FrameArray.Length, "从站发送:I帧：文件服务:");

                //if (callDirectoryServer != null)
                //{

                //    callDirectoryServer.Enqueue(e.mdata2);
                //}

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "checkGetMessage_FileServerArrived");
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
                strbuild.AppendFormat("{0:X00) ", m);
            }
            return strbuild.ToString();
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
               return true;
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
                return true;
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
