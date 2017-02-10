using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using ZFreeGo.Monitor.AutoStudio.CommCenter;
using ZFreeGo.Monitor.AutoStudio.ElementParam;
using ZFreeGo.Net.UPNP;

namespace ZFreeGo.Monitor.AutoStudio
{
    public partial class MainWindow
    {
        DataSet dataSetSystemCalibration;
        ICollection<SystemCalibration> systemCalibration;
        string pathxmlSystemCalibration = @"Config\SystemCalibration.xml";
        string pathxsdSystemCalibration = @"Config\SystemCalibration.xsd";

        private int updateIndex = 0;

        private bool IsRealUpdate = false;
        /// <summary>
        /// 专用于校准Tcp服务
        /// </summary>
        private UpnpServer upnpCalibration;
        /// <summary>
        /// 校准模式
        /// </summary>
        private bool calibrationTcpRun = false;

        /// <summary>
        /// 专用的校准事件，兼容旧的通讯方式
        /// </summary>
        private OldCalibration oldCalibration;


        /// <summary>
        /// 一次录波数据
        /// </summary>
        public RecordDatOneCollect recordCollect;

        /// <summary>
        /// 发送次数
        /// </summary>
        private int SendCount;
        /// <summary>
        /// 相关数据初始化
        /// </summary>
        void OldCalibrationInit()
        {
            oldCalibration = new OldCalibration();
            oldCalibration.CalibrationMessageArrived += oldCalibration_CalibrationMessageArrived;
            oldCalibration.RecordWaveformMessageArrived += oldCalibration_RecordWaveformMessageArrived;

            recordCollect = new RecordDatOneCollect();
            recordCollect.WaveformArrived += recordCollect_WaveformArrived;
        }

      
        /// <summary>
        /// 录波数据到达
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oldCalibration_RecordWaveformMessageArrived(object sender, RecordWaveformEventArgs e)
        {
           try
           {
               Action<RecordWaveform> action = (rwf)=>
               {
                   //当前若完成重新初始化
                   if (recordCollect.IsFinished)
                   {
                       recordCollect = new RecordDatOneCollect();
                       recordCollect.WaveformArrived += recordCollect_WaveformArrived;
                   }
                   recordCollect.AddRecordWaveform(rwf);
               };
               Dispatcher.BeginInvoke(action, e.Waveform);
               BeginInvokeUpdateHistory(e.DataArray, e.DataArray.Length, "从站发送：录波数据");
           }
            catch (Exception ex)
           {
               MessageBox.Show(ex.Message, "录波数据");
           }
        }




        /// <summary>
        /// TCP 校准初始化
        /// </summary>
        private void upnpCalibrationInit()
        {

            try
            {
                if (upnpCalibration !=null)
                {
                     throw new Exception("upnpCalibrationInit 不为空，不能建立重复连接,必须首先断开");
                }

                    OldCalibrationInit();

                    upnpCalibration = new UpnpServer(IPAddress.Parse(txtIp.Text), 8000, false);


                    upnpCalibration.NetDataArrayArrived += CalibrationNetData_Arrived;

                    upnpCalibration.ExceptionArrived += CalibrationException_TestArrived;
                    upnpCalibration.LinkingMsg += CalibrationLinkMsg_TestArrived;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "TCP初始化");
            }

           
        }
        /// <summary>
        ///校准 Tcp连接断开
        /// </summary>
        void NetCalibrationStop()
        {
            if (upnpCalibration != null)
            {
                upnpCalibration.Stop();
                upnpCalibration = null;
            }
            if (oldCalibration != null)
            {

                oldCalibration.Close();
            }
        }
        /// <summary>
        ///校准TCP连接信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalibrationLinkMsg_TestArrived(object sender, Net.NetDataArrivedEventArgs e)
        {
            Action<string> myDelegate = UpdateLinkTxt;
            Dispatcher.BeginInvoke(myDelegate, e.DataMsg);
            Action act = () =>
            {
                btnStartCalibration.Content = "停止校准模式";
                calibrationTcpRun = true;
            };

            Dispatcher.BeginInvoke(act);

           
        }


     
        /// <summary>
        /// 校准TCP异常信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalibrationException_TestArrived(object sender, Net.NetDataArrivedEventArgs e)
        {
            Action<string> myDelegate = UpdateLinkTxt;
            Dispatcher.BeginInvoke(myDelegate, e.DataMsg);
            Action act = () =>
            {
                btnStartCalibration.Content = "启动校准模式";
                calibrationTcpRun = false;
                NetCalibrationStop();
            };

            Dispatcher.BeginInvoke(act);
        }
        /// <summary>
        /// 校准Tcp网络数据到达
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalibrationNetData_Arrived(object sender, Net.Element.NetDataArrayEventArgs e)
        {
            lock (oldCalibration.ReciveQuene)
            {
                foreach (var m in e.DataArray)
                {
                    oldCalibration.ReciveQuene.Enqueue(m);

                }
                Console.WriteLine("systemCailbration字节数据接收");
            }

            string msg = Encoding.Unicode.GetString(e.DataArray, 0, e.DataArray.Length);
            string str = "";
            str += "HEX:";
            foreach (var m in e.DataArray)
            {
                str += m.ToString("X2") + " ";
            }
            msg += "\n" + str;
            Action<string> myDelegate = UpdateReciveTxt;
            Dispatcher.BeginInvoke(myDelegate, msg);
        }
        /// <summary>
        /// 系统校准数据导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemCalibrationExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataExport<SystemCalibration>(dataSetSystemCalibration, DataTypeEnum.SystemCalibration,
                  systemCalibration, pathxmlSystemCalibration);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SystemCalibrationExport_Click");
            }
        }
        /// <summary>
        /// 系统数据导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemCalibrationLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                systemCalibration = DataLoad<SystemCalibration>(ref pathxmlSystemCalibration, ref pathxsdSystemCalibration,
                ref dataSetSystemCalibration, DataTypeEnum.SystemCalibration, gridSystemCalibration);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SystemCalibrationLoad_Click");
            }
        }

        /// <summary>
        /// 用于校准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oldCalibration_CalibrationMessageArrived(object sender, OldCalibrationEventArgs e)
        {
            try
            {
                Action<OldCalibrationEventArgs> update = UpdateSystemCalibration;
                //Dispatcher.BeginInvoke(update, e);
                switch (e.Property)
                {
                    case CalibrationAction.FactorCall: //召唤系数
                        {
                            Dispatcher.BeginInvoke(update, e);
                            BeginInvokeUpdateHistory(e.Data, e.Data.Length, "从站发送:FactorCall校准:");
                            MakeLogMessage(sender,"", "召唤系数应答", Log.LogType.Calibration);
                            break;
                        }
                    case CalibrationAction.FactorDownload:
                        {
                            BeginInvokeUpdateHistory(e.Data, e.Data.Length, "从站发送:FactorDownload校准:");
                            MakeLogMessage(sender, "", "系数下载应答", Log.LogType.Calibration);
                            break;
                        }
                    case CalibrationAction.FactorFix:
                        {
                            MessageBox.Show("系数固话成功");
                            BeginInvokeUpdateHistory(e.Data, e.Data.Length, "从站发送:FactorFix校准:");
                            MakeLogMessage(sender, "", "召唤系数固化成功应答", Log.LogType.Calibration);
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "oldCalibration_CalibrationMessageArrived");
            }
        }

        /// <summary>
        ///更新召唤系数
        /// </summary>
        /// <param name="frame">系数帧</param>
        void UpdateFactorCall(UInt32[] frame)
        {
            try
            {
                var observale = (ObservableCollection<SystemCalibration>)systemCalibration;
                if (frame.Length != observale.Count)
                {
                    MessageBox.Show("接收长度与列表长度不一致", "UpdateFactorCall");                    
                }
                int len = Math.Min(frame.Length, observale.Count);
                
                for (int i = 0; i < len; i++)
                {
                    for(int j = 0; j < observale.Count; j++ )
                    {
                        if (observale[j].InternalID == (i+1))
                        {
                            observale[j].CallCoefficient = frame[i];
                            //召唤系数与下载系数相同
                            observale[j].DownloadCoefficient = observale[j].CallCoefficient;

                        }
                        
                    }
                } 
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdateFactorCall");
            }
        }



        /// <summary>
        /// 更新校准参数
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateSystemCalibration(OldCalibrationEventArgs obj)
        {
           try
           {
               UpdateFactorCall(obj.DataFrame);
           }
            catch(Exception ex)
           {
               MessageBox.Show(ex.Message, "更新校准参数");
           }
        }


        /// <summary>
        /// 启动校准模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartCalibration_Click(object sender, RoutedEventArgs e)
        {
            if (calibrationTcpRun == false)
            {
                upnpCalibrationInit();
               

            }
            else
            {
                btnStartCalibration.Content = "启动校准模式";
                calibrationTcpRun = false;
                NetCalibrationStop();
            }


        }
        /// <summary>
        /// 系数读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFactorRead_Click(object sender, RoutedEventArgs e)
        {
            if ((upnpCalibration != null) && calibrationTcpRun)
            {

                var data = oldCalibration.MakeFactorCallData();
                upnpCalibration.SendMessage(data);
                BeginInvokeUpdateHistory(data, data.Length, "主站发送:D帧:召唤系数");
                MakeLogMessage(sender, "系数读取", Log.LogType.Calibration);
            }
            else
            {
                MessageBox.Show("没有开启网络连接", "主站发送:D帧:召唤系数");
            }
        }
        /// <summary>
        /// 系数下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFactorDownload_Click(object sender, RoutedEventArgs e)
        {
            if ((upnpCalibration != null) && calibrationTcpRun)
            {
                var observale = (ObservableCollection<SystemCalibration>)systemCalibration;
                if ((observale != null) && (observale.Count > 0))
                {
                    var downfactor = new UInt32[observale.Count];
                    int i = 0;
                    foreach (var m in observale)
                    {
                        //测试使用
                      //  m.DownloadCoefficient = (UInt32)m.CallCoefficient;
                        downfactor[i++] = (UInt32)m.DownloadCoefficient;
                    }
                    var data = oldCalibration.MakeFactorDownloadData(downfactor, observale.Count);
                    upnpCalibration.SendMessage(data);
                    BeginInvokeUpdateHistory(data, data.Length, "主站发送:D帧:系数下载");
                    MakeLogMessage(sender, "系数下载", Log.LogType.Calibration);
                }
            }
            else
            {
                MessageBox.Show("没有开启网络连接", "系数下载");
            }
        }
        /// <summary>
        /// 系数固化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFacotrFix_Click(object sender, RoutedEventArgs e)
        {
            if ((upnpCalibration != null) && calibrationTcpRun)
            {
                var data = oldCalibration.MakeFactorFix();
                upnpCalibration.SendMessage(data);
                BeginInvokeUpdateHistory(data, data.Length, "主站发送:D帧:系数固化");
                MakeLogMessage(sender, "系数固化", Log.LogType.Calibration);
            }
            else
            {
                MessageBox.Show("没有开启网络连接", "系数固化");
            }
        }

        /// <summary>
        /// 实时更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxRealUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxRealUpdate.IsChecked == true)
            {
                IsRealUpdate = true;
                loopCallTimer.Interval = int.Parse(txtUpdateTime.Text);
                loopCallTimer.Start();
                SendCount = 0;
            }
            else if (CheckBoxRealUpdate.IsChecked == false)
            {
                IsRealUpdate = false;
                loopCallTimer.Interval = int.Parse(txtUpdateTime.Text);
                loopCallTimer.Stop();
                SendCount = 0;
            }
        }
        /// <summary>
        /// 更新校准数据---历史
        /// </summary>
        /// <param name="list">list列表</param>
        void UpdateCalbrationData(List<Tuple<uint, object, object>> list)
        {
            if (IsRealUpdate)
            {
                var m = (ObservableCollection<SystemCalibration>)systemCalibration;
                foreach (var ele in list)
                {
                    for (int k = 0; k < m.Count; k++)
                    {
                        var t = m[k];
                        if ((t.InternalID + Telemetering.BasicAddress - 1) == ele.Item1)
                        {                            
                            t.UpdateData(updateIndex, (float)(ele.Item2));
                        }
                    }
                }

                if (updateIndex <10)
                {
                    updateIndex++;
                }
                else
                {
                    updateIndex = 0;
                }
            }



        }
    }
}
