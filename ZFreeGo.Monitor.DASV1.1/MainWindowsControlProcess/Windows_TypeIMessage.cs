using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ApplicationMessage;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
namespace ZFreeGo.Monitor.AutoStudio
{
     public partial class MainWindow 
    {
      
         /// <summary>
         /// 未知ID命令
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        void checkGetMessage_UnknowMessageArrived(object sender, TransmitEventArgs<TypeIdentification, byte[]> e)
        {           
            MessageBox.Show(e.mdata1.ToString(), "未识别的ID帧");
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
                BeginInvokeUpdateHistory(e.mdata2.FrameArray, e.mdata2.FrameArray.Length, "从站发送:I帧:遥信命令:");

                Action<APDU> update = UpdateTelesignalisation;
                Dispatcher.BeginInvoke(update, e.mdata2);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "heckGetMessage_TelesignalisationMessageArrived");
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
                BeginInvokeUpdateHistory(e.mdata2.FrameArray, e.mdata2.FrameArray.Length, "从站发送:I帧:遥测命令");
                //var m = eventTransmissionManager.GetEventProcess(TransmissionCotrolFunction.StartDataTransmission);
                //if (m != null)
                //{
                //    m.Event.Set();
                //}
                Action<APDU> update = UpdateTelemetering;
                Dispatcher.BeginInvoke(update, e.mdata2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "遥测命令");
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
                var m = eventTypeIDManager.GetEventProcess(id);
                if (m != null)
                {
                    m.Event.Set();
                }
                appMessageManager.UpdateReceiveSequenceNumber(e.mdata2.APCI.TransmitSequenceNumber,
                    e.mdata2.APCI.ReceiveSequenceNumber);
                BeginInvokeUpdateHistory(e.mdata2.FrameArray, e.mdata2.FrameArray.Length, "从站发送:I帧:遥控命令:");
                
                switch (cot)
                {
                    case CauseOfTransmissionList.ActivationACK: //激活确认
                        {
                            SendSupervisoryFrame((ushort)(appMessageManager.RealReceiveSequenceNumber));

                            Action<APDU> lamp = ar => { UpdateTelecontrolState(ar); };
                            Dispatcher.BeginInvoke(lamp, e.mdata2);

                            break;
                        }
                    case CauseOfTransmissionList.DeactivateACK://停止激活确认
                        {

                            break;
                        }
                      
                    case CauseOfTransmissionList.ActivateTermination://激活终止
                        {
                            break;
                        }
                    case CauseOfTransmissionList.Deactivate:
                        {
                            break;
                        }
                 
                }



                //Action<APDU> update = UpdateTelesignalisation;
                //Dispatcher.BeginInvoke(update, e.mdata2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "checkGetMessage_TelecontrolCommandArrived");
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
                BeginInvokeUpdateHistory(e.mdata2.FrameArray, e.mdata2.FrameArray.Length, "从站发送:I帧:电能脉冲");
                switch ((TypeIdentification)e.mdata2.ASDU.TypeId)
                {
                    case TypeIdentification.C_CI_NA_1: //电能脉冲召唤
                        {

                            break;
                        }
                    case TypeIdentification.M_IT_NA_1: //累积量
                        {
                              Action<APDU> update = UpdateElectricEnergy;
                              Dispatcher.BeginInvoke(update, e.mdata2);
                           
                            break;
                        }
                }
    
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "遥测命令");
            }
        }

        /// <summary>
        /// 校准参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkGetMessage_CalibrationMessageArrived(object sender, TransmitEventArgs<TypeIdentification, APDU> e)
        {
            try
            {
                appMessageManager.UpdateReceiveSequenceNumber(e.mdata2.APCI.TransmitSequenceNumber,
                   e.mdata2.APCI.ReceiveSequenceNumber);
                BeginInvokeUpdateHistory(e.mdata2.FrameArray, e.mdata2.FrameArray.Length, "从站发送:I帧:校准参数");
                switch ((TypeIdentification)e.mdata2.ASDU.TypeId)
                {
                    case TypeIdentification.P_AC_NA_1: // 参数激活
                        {

                            break;
                        }
                    case TypeIdentification.P_ME_NC_1: //  测量值参数，短浮点数
                        {

                            break;
                        }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "校准命令");
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
                BeginInvokeUpdateHistory(e.mdata2.FrameArray, e.mdata2.FrameArray.Length, "从站发送:I帧:校准参数");
                switch ((TypeIdentification)e.mdata2.ASDU.TypeId)
                {
                    case TypeIdentification.C_SE_NC_1: // 设定值，短浮点数
                        {

                            break;
                        }
                   
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "保护定值设置");
            }
        }
       

    }
}
