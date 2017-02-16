using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using ZFreeGo.Monitor.AutoStudio.CommCenter;
using ZFreeGo.Monitor.AutoStudio.Log;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ApplicationMessage;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.FileSever;

namespace ZFreeGo.Monitor.AutoStudio
{

    public partial class MainWindow 
    {

        /// <summary>
        /// 传输协议应用管理
        /// </summary>
        public ApplicationFrameManager appMessageManager;
        /// <summary>
        /// 类型ID 同步事件管理
        /// </summary>
        private EventTypeID eventTypeIDManager;
        /// <summary>
        /// 传输控制 同步事件管理
        /// </summary>
        private EventTCF eventTransmissionManager;

        /// <summary>
        /// 编号监视功能帧同步事件
        /// </summary>
        private ManualResetEvent supervisoryFrameEvent;


        /// <summary>
        /// 提取信息帧事件管理
        /// </summary>
        private CheckGetMessage checkGetMessage;

      
        /// <summary>
        /// 管理等待事件
        /// </summary>
        private List<ProcessControlPure> processList;

       

        /// <summary>
        /// 控制流配置
        /// </summary>
        private void ControlProcessConfig()
        {
            eventTypeIDManager = new EventTypeID();
            eventTransmissionManager = new EventTCF();
            checkGetMessage = new CheckGetMessage(eventTypeIDManager, eventTransmissionManager);
            //U-TCF
            checkGetMessage.TransmitControlCommandArrived +=checkGetMessage_TransmitControlCommandArrived;
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
            checkGetMessage.ProtectSetMessageArrived +=checkGetMessage_ProtectSetMessageArrived;
            checkGetMessage.FileServerArrived += checkGetMessage_FileServerArrived;
            //I-未知
            checkGetMessage.UnknowMessageArrived += checkGetMessage_UnknowMessageArrived;

            appMessageManager = new ApplicationFrameManager();          
            processList = new List<ProcessControlPure>();
            
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
                BeginInvokeUpdateHistory(e.mdata2.FrameArray, e.mdata2.FrameArray.Length, "从站发送:I帧：文件服务:");

                if (callDirectoryServer != null)
                {
                    
                    callDirectoryServer.Enqueue(e.mdata2);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "checkGetMessage_FileServerArrived");
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
                //同步释放相应事件
                var m = eventTypeIDManager.GetEventProcess((TypeIdentification)e.MasterCMD.ASDU.TypeId);
                if (m != null)
                {
                    m.Event.Set();
                }
                appMessageManager.UpdateReceiveSequenceNumber(e.MasterCMD.APCI.TransmitSequenceNumber,
                    e.MasterCMD.APCI.ReceiveSequenceNumber);
                BeginInvokeUpdateHistory(e.MasterCMD.FrameArray, e.MasterCMD.FrameArray.Length, "从站发送:I帧：主站复位:");
                MakeLogMessage(sender, "", "主站复位应答" + e.MasterCMD.ToString(), LogType.Time);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "checkGetMessage_MasterResetArrived");
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
                callServer.Enqueue(e.MasterCMD);

                switch ((CauseOfTransmissionList)e.MasterCMD.ASDU.CauseOfTransmission1)
                {
                    case CauseOfTransmissionList.ActivationACK:
                        {                            
                            BeginInvokeUpdateHistory(e.MasterCMD.FrameArray, e.MasterCMD.FrameArray.Length, "从站发送:I帧:召唤激活确认");
                            break;
                        }
                    case CauseOfTransmissionList.ActivateTermination:
                        {

                            BeginInvokeUpdateHistory(e.MasterCMD.FrameArray, e.MasterCMD.FrameArray.Length, "从站发送:I帧:召唤结束");
                            
                            SendSupervisoryFrame((ushort)(appMessageManager.RealReceiveSequenceNumber));
                            break;
                        }
                }


                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "checkGetMessage_MasterInterrogationArrived");
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
                //同步释放相应事件
                var m = eventTypeIDManager.GetEventProcess((TypeIdentification)e.MasterCMD.ASDU.TypeId);
                if (m != null)
                {
                    m.Event.Set();
                }
                appMessageManager.UpdateReceiveSequenceNumber(e.MasterCMD.APCI.TransmitSequenceNumber,
                    e.MasterCMD.APCI.ReceiveSequenceNumber);
                BeginInvokeUpdateHistory(e.MasterCMD.FrameArray, e.MasterCMD.FrameArray.Length, "从站发送:I帧：主站初始化:");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "checkGetMessage_MasterInitializeArrived");          
            }
        }

      
        

        /// <summary>
        /// 启动消息处理过程
        /// </summary>
        /// <param name="data">发送的消息转换为字节后的数组</param>
        private void StartMessageProcess(byte [] data)
        {
            if (isTcpRun)
            {
                upnpServer.SendMessage(data);
            }
            else
            {
                MessageBox.Show("upnpServer未启动，发送数据无效");
            }
           

        }

        /// <summary>
        /// 停止所有processList线程处理
        /// </summary>
        private void StopProcessList()
        {
            if (processList != null)
            {
                for(int i = 0; i < processList.Count; i++)
                {
                    var m = processList[i];
                    
                    if(! m.IsRun) //已经停止运行的,直接移除 
                    {
                        processList.Remove(m);                        
                    }
                    else
                    { //正在运行的先先停止运行,再移除
                        m.IsRun = false;
                        m.StopThread();                      
                    }
                    processList.Remove(m);
                    i = 0;
                }
            }
        }

        /// <summary>
        /// 协议服务复归
        /// </summary>
        private void ResetServer()
        {
            //重新计数当前
            appMessageManager = new ApplicationFrameManager();
            //首先终止当前处理过程
            StopProcessList();
            processList = new List<ProcessControlPure>();
            //更新显示状态
            UpdateDeviceOnlineStatusBar(false);
        }
       
       
    



    }
}
