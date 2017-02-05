using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ZFreeGo.Monitor.AutoStudio.ElementParam;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.Monitor.AutoStudio
{
    public partial class MainWindow
    {
        DataSet dataSetTelesignalisation;
        ICollection<Telesignalisation> telesignalisation;
        string pathxmlTelesignalisation = @"Config\Telesignalisation.xml";
        string pathxsdTelesignalisation = @"Config\Telesignalisation.xsd";


        private void TelesignalisationExport_Click(object sender, RoutedEventArgs e)
        {
            DataExport<Telesignalisation>(dataSetTelesignalisation, DataTypeEnum.Telesignalisation,
              telesignalisation, pathxmlTelesignalisation);
            
        }
        private void TelesignalisationLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                telesignalisation = DataLoad<Telesignalisation>(ref pathxmlTelesignalisation,ref pathxsdTelesignalisation,
                 ref dataSetTelesignalisation, DataTypeEnum.Telesignalisation, gridTelesignalisation);

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "TelesignalisationExport_Click");
            }
        }
        private void selected_Click(object sender, RoutedEventArgs e)
        {

            var m = (ObservableCollection<Telesignalisation>)telesignalisation;

            for (int i = 0; i < gridTelesignalisation.Items.Count; i++ )
            {
                //DataRowView drv = gridTelesignalisation.Items[i] as DataRowView;
                //if (drv != null)
                //{
               
                    DataGridRow row = (DataGridRow)gridTelesignalisation.ItemContainerGenerator.ContainerFromIndex(i);
                    if (row == null)
                    {
                        return;
                    }
                    if (i % 2 == 1)
                    {
                        row.Background = new SolidColorBrush(Colors.Red);

                    }
                //}
                

            }

        
        

           
           

        }
        //private void gridTelesignalisation_LoadingRowDetails(object sender, DataGridRowDetailsEventArgs e)
        //{

        //}


        private SolidColorBrush hightLight = new SolidColorBrush(Colors.SkyBlue);
        private SolidColorBrush normal = new SolidColorBrush(Colors.White);
        /// <summary>
        /// 载入行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridTelesignalisation_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var m = (ObservableCollection<Telesignalisation>)telesignalisation;
            //var t = e.Row.DataContext as Telesignalisation;
            //if (t.IsChanged)
            //{
            //    DataGridRow dataGridRow = e.Row;
            //    dataGridRow.Background = new SolidColorBrush(Colors.SkyBlue);
            //    t.IsChanged = false;
            //}
            var t = (Telesignalisation)e.Row.DataContext;
            if(t.IsChanged)
            {
                e.Row.Background = hightLight;
            }
            else
            {
                e.Row.Background = normal;
            }

            
        }




        /// <summary>
        /// 更新遥信数据
        /// </summary>
        private void UpdateTelesignalisation(APDU apdu)
        {
            try
            {
                var list = apdu.GetInformationList();
                
                var m = (ObservableCollection<Telesignalisation>)telesignalisation;
                
                switch ((TypeIdentification)apdu.ASDU.TypeId)
                {
                    case TypeIdentification.M_SP_NA_1://单点信息
                    case TypeIdentification.M_DP_NA_1://双点信息
                        {
                            foreach (var ele in list)
                            {
                                for (int k = 0; k < m.Count; k++)
                                {
                                    var t = m[k];
                                    if ((t.InternalID + Telesignalisation.BasicAddress - 1) == ele.Item1)
                                    {
                                        t.Date = DateTime.Now.ToShortTimeString();
                                        t.TelesignalisationResult =((byte)ele.Item2).ToString("X2");
                                    }

                                }
                            }
                            break;
                        }                   
                      
                    case TypeIdentification.M_SP_TB_1://带CP56Time2a时标的单点信息
                    case TypeIdentification.M_DP_TB_1://带CP56Time2a时标的双点信息
                        {
                           var log = (ObservableCollection<EventLog>)eventLog;
                           foreach (var ele in list)
                           {
                               var time = ele.Item3 as CP56Time2a;
                               
                               var alog = new EventLog((int)ele.Item1, "sd", ele.Item2.ToString(),
                                   time.ToString(), "A", time.Milliseconds.ToString());
                               log.Add(alog);
                               
                           }
                            break;
                        }
                    default:
                            {
                                throw new ArgumentOutOfRangeException("不能识别既定的遥信ID");
                            }
                }

               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UpdateTelesignalisation");
            }

        }
       
        

    }
}
