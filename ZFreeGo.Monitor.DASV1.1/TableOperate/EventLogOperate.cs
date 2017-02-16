using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using ZFreeGo.Monitor.AutoStudio.ElementParam;

namespace ZFreeGo.Monitor.AutoStudio
{
    public partial class MainWindow
    {
        DataSet dataSetEventLog;
        ICollection<EventLog> eventLog;
        string pathxmlEventLog = @"Config\EventLog.xml";
        string pathxsdlEventLog = @"Config\EventLog.xsd";

        private void EventlogLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (IsExists(ref pathxmlEventLog, "xml") && IsExists(ref pathxsdlEventLog, "xsd"))
                {
                    dataSetEventLog = getData.ReadXml(pathxmlEventLog, pathxsdlEventLog);
                    eventLog = getData.GetDataCollect(dataSetEventLog, DataTypeEnum.EventLog) as ICollection<EventLog>;
                    if (eventLog != null)
                    {
                        gridEventLog.ItemsSource = eventLog;
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "EventLogLoad_Click");
            }

        }

        private void EventLogExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                getData.UpdateTable(dataSetEventLog, DataTypeEnum.EventLog, eventLog);
                dataSetEventLog.WriteXml(pathxmlEventLog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "EventLlogLoad_Click");
            }
        }

    }
}
