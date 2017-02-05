using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using ZFreeGo.Monitor.AutoStudio.ElementParam;
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

        
    }
}
