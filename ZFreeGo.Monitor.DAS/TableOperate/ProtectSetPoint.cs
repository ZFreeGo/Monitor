using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using ZFreeGo.Monitor.AutoStudio.ElementParam;
using ZFreeGo.Monitor.AutoStudio.Log;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.Frame104;


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
               
                   
               



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DownloadProtectSet_Click");
            }
        }

        
    }
}
