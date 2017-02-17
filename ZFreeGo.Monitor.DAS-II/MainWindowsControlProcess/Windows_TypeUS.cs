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
        /// S帧，监控数据到达
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkGetMessage_SupervisoryCommandArrived(object sender, TransmitEventArgs<ushort, byte[]> e)
        {
            try
            {
                //同步释放相应事件
                if (supervisoryFrameEvent != null)
                {
                    supervisoryFrameEvent.Set();
                }
                BeginInvokeUpdateHistory(e.mdata2, e.mdata2.Length, "从站发送:S帧：接收序列号:" + e.mdata1.ToString());

                appMessageManager.AckReceiveSequenceNumber(e.mdata1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "checkGetMessage_SupervisoryCommandArrived");
            }


            // throw new NotImplementedException();
        }



        /// <summary>
        ///U帧 TCF传输控制功能事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkGetMessage_TransmitControlCommandArrived(
            object sender, TransmitEventArgs<TransmissionCotrolFunction, byte[]> e)
        {
            try
            {
                //同步释放相应事件
                //var m = eventTransmissionManager.GetEventProcess(e.mdata1);
                //if (m != null)
                //{
                //    m.Event.Set();
                //}


                switch (e.mdata1)
                {
                    case TransmissionCotrolFunction.StartDataTransmission:
                        {
                            BeginInvokeUpdateHistory(e.mdata2, e.mdata2.Length, "从站发送:U帧:启动数据传输STARTDT命令:");
                            break;
                        }
                    case TransmissionCotrolFunction.AcknowledgementStartDataTransmission:
                        {
                            BeginInvokeUpdateHistory(e.mdata2, e.mdata2.Length, "从站发送:U帧:启动数据传输STARTDT确认:");

                            Action lamp = () => { UpdateDeviceOnlineStatusBar(true); };
                            Dispatcher.BeginInvoke(lamp);

                            var m = eventTransmissionManager.GetEventProcess(TransmissionCotrolFunction.StartDataTransmission);
                            if (m != null)
                            {
                                m.Event.Set();
                            }
                            break;
                        }
                    case TransmissionCotrolFunction.StopDataTransmission:
                        {
                            BeginInvokeUpdateHistory(e.mdata2, e.mdata2.Length, "从站发送:U帧:停止数据传输STARTDT命令:");
                            break;
                        }
                    case TransmissionCotrolFunction.AcknowledgementStopDataTransmission:
                        {
                            BeginInvokeUpdateHistory(e.mdata2, e.mdata2.Length, "从站发送:U帧:停止数据传输STARTDT确认:");
                            break;
                        }
                    case TransmissionCotrolFunction.TestFrame:
                        {
                            BeginInvokeUpdateHistory(e.mdata2, e.mdata2.Length, "从站发送:U帧:测试帧TESTFR命令");
                            SendTCFCommand(TransmissionCotrolFunction.AcknowledgementTestFrame);
                            var m = eventTransmissionManager.GetEventProcess(TransmissionCotrolFunction.AcknowledgementTestFrame);
                            if (m != null)
                            {
                                m.Event.Set();
                            }
                            break;
                        }
                    case TransmissionCotrolFunction.AcknowledgementTestFrame:
                        {
                            BeginInvokeUpdateHistory(e.mdata2, e.mdata2.Length, "从站发送:U帧:测试帧TESTFR确认");

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "checkGetMessage_TransmitControlCommandArrive");

            }
        }



    }
}
