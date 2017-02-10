using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using ZFreeGo.Monitor.AutoStudio.ElementParam;
using ZFreeGo.Monitor.AutoStudio.Log;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.Monitor.AutoStudio
{
    public partial class MainWindow
    {
        DataSet dataSetProtectSetPoint;
        ICollection<ProtectSetPoint> protectSetPoint;
        string pathxmlProtectSetPoint = @"Config\ProtectSetPoint.xml";
        string pathxsdProtectSetPoint = @"Config\ProtectSetPoint.xsd";
        private void ProtectSetPointExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataExport<ProtectSetPoint>(dataSetProtectSetPoint, DataTypeEnum.ProtectSetPoint,
                  protectSetPoint, pathxmlProtectSetPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ProtectSetPointExport_Click");
            }
        }

        private void ProtectSetPointLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                protectSetPoint = DataLoad<ProtectSetPoint>(ref pathxmlProtectSetPoint, ref pathxsdProtectSetPoint,
                ref dataSetProtectSetPoint, DataTypeEnum.ProtectSetPoint, gridProtectSetPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ProtectSetPointLoad_Click");
            }
        }

        /// <summary>
        /// 保护定值选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadProtectSetSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var observale = (ObservableCollection<ProtectSetPoint>)protectSetPoint;
                if ((observale != null) && (observale.Count > 0))
                {
                    var qos = new QualifyCommandSet(ActionDescrible.Select);
                    var protectsetAPDU = new APDU(appMessageManager.TransmitSequenceNumber, appMessageManager.RealReceiveSequenceNumber,
                         TypeIdentification.C_SE_NC_1, true, (byte)observale.Count,
                 CauseOfTransmissionList.Activation, appMessageManager.ASDUADdress, ProtectSetPoint.BasicAddress, qos);

                    foreach (var m in observale)
                    {
                        if (m.InternalID <= observale.Count)
                        {
                            var sf = new ShortFloating((float)m.ParameterValue);
                            protectsetAPDU.AddInformationObject(sf.GetDataArray(),
                                (byte)sf.GetDataArray().Length, (byte)(m.InternalID - 1));
                        }
                        else
                        {
                            throw new Exception("序号不在顺序范围之内，无法使用序列化方法，请检查InternalID是否连续");
                        }
                    }
                    //BeginInvokeUpdateHistory(fram.GetAPDUDataArray(), fram.FrameArray.Length, "测试");
                    SendTypeIMessage(TypeIdentification.P_ME_NC_1, protectsetAPDU);
                    MakeLogMessage(sender, "定值选择" + protectsetAPDU.ToString(), LogType.ProtectSetpoint);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DownloadProtectSetSelect_Click");
            }
        }
        /// <summary>
        /// 下载保护定值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadProtectSet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var qos = new QualifyCommandSet(ActionDescrible.Execute);
                var observale = (ObservableCollection<ProtectSetPoint>)protectSetPoint;
                if ((observale != null) && (observale.Count > 0))
                {
                    var protectsetAPDU = new APDU(appMessageManager.TransmitSequenceNumber, appMessageManager.RealReceiveSequenceNumber,
                         TypeIdentification.C_SE_NC_1, true, (byte)observale.Count,
                 CauseOfTransmissionList.Activation, appMessageManager.ASDUADdress, ProtectSetPoint.BasicAddress, qos);



                    foreach (var m in observale)
                    {
                        if (m.InternalID <= observale.Count)
                        {
                            var sf = new ShortFloating((float)m.ParameterValue);
                            protectsetAPDU.AddInformationObject(sf.GetDataArray(), (byte)sf.GetDataArray().Length, (byte)(m.InternalID - 1));
                        }
                        else
                        {
                            throw new Exception("序号不在顺序范围之内，无法使用序列化方法，请检查InternalID是否连续");
                        }
                    }
                    //BeginInvokeUpdateHistory(fram.GetAPDUDataArray(), fram.FrameArray.Length, "测试");
                    SendTypeIMessage(TypeIdentification.P_ME_NC_1, protectsetAPDU);
                    MakeLogMessage(sender, "下载定值" + protectsetAPDU.ToString(), LogType.ProtectSetpoint);
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DownloadProtectSet_Click");
            }
        }

        
    }
}
