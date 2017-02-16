using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using ZFreeGo.Monitor.AutoStudio.CommCenter;
using ZFreeGo.Monitor.AutoStudio.ElementParam;
using ZFreeGo.Net.UPNP;

namespace ZFreeGo.Monitor.AutoStudio
{
    public partial class MainWindow
    {
        DataSet dataSetSystemParameter;
        ICollection<SystemParameter> systemParameter;
        string pathxmlSystemParameter = @"Config\SystemParameter.xml";
        string pathxsdSystemParameter = @"Config\SystemParameter.xsd";

      

        private void SystemParameterExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataExport<SystemParameter>(dataSetSystemParameter, DataTypeEnum.SystemParameter,
                  systemParameter, pathxmlSystemParameter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SystemParameterExport_Click");
            }
        }

        private void SystemParameterLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                systemParameter = DataLoad<SystemParameter>(ref pathxmlSystemParameter, ref pathxsdSystemParameter,
                ref dataSetSystemParameter, DataTypeEnum.SystemParameter, gridSystemParameter);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SystemParameterLoad_Click");
            }
        }

        
    }
}
