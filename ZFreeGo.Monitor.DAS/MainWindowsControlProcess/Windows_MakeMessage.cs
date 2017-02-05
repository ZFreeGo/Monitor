using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ApplicationMessage;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.Monitor.AutoStudio
{
    public partial class MainWindow
    {
        /// <summary>
        /// TypeS 监控功能, 消息处理完成后处理
        /// </summary>
        /// <param name="n"></param>
        private void MeeageProcessCompleted(int n)
        {
            Action<string> myDelegate = UpdateReciveTxt;
            Dispatcher.BeginInvoke(myDelegate, "应答完成\n");

            //controlFlow.UpdateFlowStep(FlowGather.StartDataTransmission);

        }

        /// <summary>
        ///  TypeS 监控功能,, 消息处理未完成后处理
        /// </summary>
        /// <param name="n"></param>
        private void MeeageProcessNoCompleted(int n)
        {
            Action<string> myDelegate = UpdateReciveTxt;
            Dispatcher.BeginInvoke(myDelegate, "应答失败\n");
            //controlFlow.RollbackFlowStep(FlowGather.StartDataTransmission);
        }


        /// <summary>
        ///TypeS Supervisory处理流程
        /// </summary>
        /// <param name="data">发送数据数组</param>
        /// <param name="len">发送的数据长度</param>
        /// <param name="ms">超时时间，单位ms</param>
        /// <param name="i">索引</param>
        private bool SupervisoryProcess(byte[] data, byte len, int ms, int i)
        {
            if (data.Length < len)
            {
                return false;
            }            
          
            var process = new ProcessControl<byte[], int, int>(data, i, i, StartMessageProcess,
                MeeageProcessCompleted, MeeageProcessNoCompleted, ms, supervisoryFrameEvent);
            process.StartProcessThread();
            processList.Add(process);

            BeginInvokeUpdateHistory(data, len, "主站发送:S帧");


            return true;

        }

        /// <summary>
        /// 发送APCI—S 监控帧
        /// </summary>
        /// <param name="reciveNum">接收序列号</param>
        void SendSupervisoryFrame(UInt16 reciveNum)
        {
            try
            {
                var frame = new APCITypeS(reciveNum);
                var m = frame.GetAPCIDataArray();
                supervisoryFrameEvent = new System.Threading.ManualResetEvent(false);
                BeginInvokeUpdateHistory("主站发送:S帧:接收序列号:" + reciveNum.ToString() + " ");
                SupervisoryProcess(m, (byte)m.Length, appMessageManager.WaitTime, 0);
                //发送时相当于确认接收
                appMessageManager.NoReciveAckNum = 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "SendSupervisoryFrame");
            }


            
        }


        /// <summary>
        /// 传输功能控制TCF, 消息处理完成后处理
        /// </summary>
        /// <param name="n"></param>
        private void MeeageProcessCompleted(TransmissionCotrolFunction tcf)
        {
            Action<string> myDelegate = UpdateReciveTxt;
            Dispatcher.BeginInvoke(myDelegate, "应答完成\n");


            switch (tcf)
            {
                case TransmissionCotrolFunction.StartDataTransmission:
                    {
                        
                        break;
                    }
                default:
                    {
                        break;
                    }

            }

        }

        /// <summary>
        /// 传输控制功能TCF, 消息处理未完成后处理
        /// </summary>
        /// <param name="n"></param>
        private void MeeageProcessNoCompleted(TransmissionCotrolFunction tcf)
        {
            Action<string> myDelegate = UpdateReciveTxt;
            Dispatcher.BeginInvoke(myDelegate, "应答失败\n");
            switch (tcf)
            {
                case TransmissionCotrolFunction.StartDataTransmission:
                    {
                       
                        break;
                    }
                default:
                    {
                        break;
                    }

            }
        }


        /// <summary>
        /// TCF 处理流程
        /// </summary>
        /// <param name="data">消息帧转化后，发送的字节数组</param>
        /// <param name="len">发送的数据长度</param>
        /// <param name="ms">超时时间，单位ms</param>
        /// <param name="EventTCF eventTCF">事件控制帧</param>
        private bool TCFProcess(byte[] data, byte len, int ms, TransmissionCotrolFunction tcf)
        {
            if (data.Length < len)
            {
                return false;
            }

            var mStartEvent = eventTransmissionManager.GetEventProcess(tcf);
            var process = new ProcessControl<byte[], TransmissionCotrolFunction, TransmissionCotrolFunction>(data, tcf, tcf, StartMessageProcess,
                MeeageProcessCompleted, MeeageProcessNoCompleted, ms, mStartEvent.Event);
            process.StartProcessThread();
            processList.Add(process);

            BeginInvokeUpdateHistory(data, len, "主站发送:U帧");


            return true;

        }
       
        /// <summary>
        /// 发送传输控制指令
        /// </summary>
        /// <param name="tcf">传输控制功能</param>
        private void SendTCFCommand(TransmissionCotrolFunction tcf)
        {
            try
            {
                eventTransmissionManager.AddEventProcess(new EventProperty<TransmissionCotrolFunction>(tcf));
                var apciU = new APCITypeU(tcf);

                TCFProcess(apciU.GetAPCIDataArray(), (byte)apciU.GetAPCIDataArray().Length, 5000, tcf);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SendTCFCommand");
            }

        }

        /// <summary>
        /// 消息传输I格式, 消息处理完成后处理
        /// </summary>
        /// <param name="n"></param>
        private void MeeageProcessCompleted(TypeIdentification id)
        {

          //  BeginInvokeUpdateHistory(id.ToString() + "应答完成");

            switch (id)
            {
                //主站召唤
                case TypeIdentification.C_IC_NA_1:
                    {
                        
                        break;
                    }
                case TypeIdentification.C_CS_NA_1://时钟同步 
                    {
                        break;
                    }
                case TypeIdentification.C_RP_NA_1://复位经进程命令 
                    {
                        break;
                    }
                case TypeIdentification.M_EI_NA_1://初始化结束命令
                    {
                        break;
                    }

                default:
                    {
                        break;
                    }

            }

        }

        /// <summary>
        /// 消息传输I格式, 消息处理未完成后处理
        /// </summary>
        /// <param name="id"></param>
        private void MeeageProcessNoCompleted(TypeIdentification id)
        {
            Action<string> myDelegate = UpdateReciveTxt;
            Dispatcher.BeginInvoke(myDelegate, "应答失败\n");
            switch (id)
            {
                //主站召唤
                case TypeIdentification.C_IC_NA_1:
                    {
                       
                        break;
                    }
                case TypeIdentification.C_CS_NA_1://时钟同步 
                    {
                        break;
                    }
                case TypeIdentification.C_RP_NA_1://复位经进程命令 
                    {
                        break;
                    }
                case TypeIdentification.M_EI_NA_1://初始化结束命令
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }

            }
        }


        /// <summary>
        /// 消息传输I格式 处理流程
        /// </summary>
        /// <param name="data">消息帧转化后，发送的字节数组</param>
        /// <param name="len">发送的数据长度</param>
        /// <param name="ms">超时时间，单位ms</param>
        /// <param name="id">类型</param>
        private bool MainTypeIProcess(byte[] data, int len, int ms, TypeIdentification id)
        {
            if (data.Length < len)
            {
                return false;
            }
            
            var mStartEvent = eventTypeIDManager.GetEventProcess(id);
            var process = new ProcessControl<byte[], TypeIdentification, TypeIdentification>(data, id, id, StartMessageProcess,
                MeeageProcessCompleted, MeeageProcessNoCompleted, ms, mStartEvent.Event);
            process.StartProcessThread();
            processList.Add(process);
            BeginInvokeUpdateHistory(data, len, "主站发送:I帧");
            return true;

        }

        /// <summary>
        /// 发送召唤命令
        /// </summary>
        /// <param name="cot">传输原因</param>
        /// <param name="qoi">召唤限定词</param>
        private void SendMasterCommand(CauseOfTransmissionList cot, QualifyOfInterrogationList qoi)
        {
            try
            {
                var id = TypeIdentification.C_IC_NA_1;//召唤命令
                eventTypeIDManager.AddEventProcess(new EventProperty(id));
                var frame = new MasterCommand(appMessageManager.TransmitSequenceNumber, appMessageManager.RealReceiveSequenceNumber,
                    id, cot, appMessageManager.ASDUADdress, qoi);
                var array = frame.GetAPDUDataArray();
                MainTypeIProcess(array, array.Length, appMessageManager.WaitTime, id);
                appMessageManager.UpdateTransmitSequenceNumber();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "SendMasterCommand发送召唤命令");
            }

        }
        
      
        /// <summary>
        /// 发送时钟同步命令
        /// </summary>
        /// <param name="cot">传输原因</param>
        /// <param name="time">时间</param>
        private void SendMasterCommand(CauseOfTransmissionList cot, CP56Time2a time)
        {
            try
            {
                var id = TypeIdentification.C_CS_NA_1;//时钟同步 
                eventTypeIDManager.AddEventProcess(new EventProperty(id));
                var frame = new MasterCommand(appMessageManager.TransmitSequenceNumber, appMessageManager.RealReceiveSequenceNumber,
                    id, cot, appMessageManager.ASDUADdress, time);
                var array = frame.GetAPDUDataArray();
                
                MainTypeIProcess(array, array.Length, appMessageManager.WaitTime, id);
                appMessageManager.UpdateTransmitSequenceNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SendMasterCommand发送时钟同步命令");
            }

        }
        /// <summary>
        /// 发送进程复位命令
        /// </summary>
        /// <param name="cot">传输原因</param>
        /// <param name=" qrp">复位进程命令</param>
        private void SendMasterCommand(CauseOfTransmissionList cot, QualifyResetProgressList qrp)
        {
            try
            {
                var id = TypeIdentification.C_RP_NA_1;//复位经进程命令 
                eventTypeIDManager.AddEventProcess(new EventProperty(id));
                var frame = new MasterCommand(appMessageManager.TransmitSequenceNumber, appMessageManager.RealReceiveSequenceNumber,
                    id, cot, appMessageManager.ASDUADdress, qrp);
                var array = frame.GetAPDUDataArray();
                MainTypeIProcess(array, array.Length, appMessageManager.WaitTime, id);
                appMessageManager.UpdateTransmitSequenceNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SendMasterCommand复位进程命令");
            }

        }
        /// <summary>
        ///  初始化结束命令
        /// </summary>
        /// <param name="cot">传输原因</param>
        /// <param name="coi">初始化结束命令</param>
        /// <param name="objectAddr">公共地址</param>
        private void SendMasterCommand(CauseOfTransmissionList cot, CauseOfInitialization coi, UInt32 objectAddr)
        {
            try
            {
                var id = TypeIdentification.M_EI_NA_1;//初始化结束命令
                eventTypeIDManager.AddEventProcess(new EventProperty(id));
                var frame = new MasterCommand(appMessageManager.TransmitSequenceNumber, appMessageManager.RealReceiveSequenceNumber,
                    id, cot, appMessageManager.ASDUADdress, objectAddr, coi);
                var array = frame.GetAPDUDataArray();
                MainTypeIProcess(array, array.Length, appMessageManager.WaitTime, id);
                appMessageManager.UpdateTransmitSequenceNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SendMasterCommand复位进程命令");
            }

        }
        /// <summary>
        ///  电能召唤命令
        /// </summary>
        /// <param name="cot">传输原因</param>
        /// <param name="qcc">计量召唤限定词QCC</param>
        /// <param name="objectAddr">公共地址</param>
        private void SendMasterCommand(CauseOfTransmissionList cot, QualifyCalculateCommad qcc)
        {
            try
            {
                var id = TypeIdentification.C_CI_NA_1;//电能召唤
                eventTypeIDManager.AddEventProcess(new EventProperty(id));
                var frame = new MasterCommand(appMessageManager.TransmitSequenceNumber, appMessageManager.RealReceiveSequenceNumber,
                    id, cot, appMessageManager.ASDUADdress, 0, qcc.QCC);
                var array = frame.GetAPDUDataArray();
                MainTypeIProcess(array, array.Length, appMessageManager.WaitTime, id);
                appMessageManager.UpdateTransmitSequenceNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SendMasterCommand电能召唤命令");
            }

        }
        /// <summary>
        /// 读当前定值区号
        /// </summary>
        /// <param name="cot"></param>
        private void SendMasterCommand(CauseOfTransmissionList cot)
        {
            try
            {
                var id = TypeIdentification.C_RR_NA_1;//读设定值
                eventTypeIDManager.AddEventProcess(new EventProperty(id));
                var frame = new MasterCommand(appMessageManager.TransmitSequenceNumber, appMessageManager.RealReceiveSequenceNumber,
                    id, cot, appMessageManager.ASDUADdress);
                var array = frame.GetAPDUDataArray();
                MainTypeIProcess(array, array.Length, appMessageManager.WaitTime, id);
                appMessageManager.UpdateTransmitSequenceNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SendMasterCommand电能召唤命令");
            }

        }
        /// <summary>
        ///  主站遥控命令
        /// </summary>
        /// <param name="cot">传输原因</param>
        /// <param name="sco">单命令</param>
        /// <param name="objectAddr">信息对象地址</param>
        private void SendMasterCommand(CauseOfTransmissionList cot, SingleCommand sco, UInt32 objectAddr)
        {
            try
            {
                var id = TypeIdentification.C_SC_NA_1;//遥控命令
                eventTypeIDManager.AddEventProcess(new EventProperty(id));
                var frame = new MasterCommand(appMessageManager.TransmitSequenceNumber, appMessageManager.RealReceiveSequenceNumber,
                    id, cot, appMessageManager.ASDUADdress, objectAddr, sco.SCO);
                var array = frame.GetAPDUDataArray();
                MainTypeIProcess(array, array.Length, appMessageManager.WaitTime, id);
                appMessageManager.UpdateTransmitSequenceNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SendMasterCommand主站遥控命令");
            }

        }
        /// <summary>
        ///  主站遥控命令 双命令
        /// </summary>
        /// <param name="cot">传输原因</param>
        /// <param name="dco">单命令</param>
        /// <param name="objectAddr">信息对象地址</param>
        private void SendMasterCommand(CauseOfTransmissionList cot, DoubleCommand dco, UInt32 objectAddr)
        {
            try
            {
                var id = TypeIdentification.C_DC_NA_1;//遥控命令
                eventTypeIDManager.AddEventProcess(new EventProperty(id));
                var frame = new MasterCommand(appMessageManager.TransmitSequenceNumber, appMessageManager.RealReceiveSequenceNumber,
                    id, cot, appMessageManager.ASDUADdress, objectAddr, dco.DCO);
                var array = frame.GetAPDUDataArray();
                MainTypeIProcess(array, array.Length, appMessageManager.WaitTime, id);
                appMessageManager.UpdateTransmitSequenceNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SendMasterCommand主站遥控命令");
            }

        }

    


        /// <summary>
        /// 主站发送TypeI-测量值参数/参数激活
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apdu"></param>
        private void SendTypeIMessage(TypeIdentification id, APDU apdu)
        {
            try
            {
                eventTypeIDManager.AddEventProcess(new EventProperty(id));
                var array = apdu.GetAPDUDataArray();
                MainTypeIProcess(array, array.Length, appMessageManager.WaitTime, id);
                appMessageManager.UpdateTransmitSequenceNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SendTypeIMessage主站遥控命令");
            }

        }
       

    }
}
