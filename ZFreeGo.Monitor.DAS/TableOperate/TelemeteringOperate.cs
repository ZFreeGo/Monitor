using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZFreeGo.Monitor.AutoStudio.ElementParam;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.Monitor.AutoStudio
{
    public partial class MainWindow
    {
        DataSet dataSetTelemetering;
        ICollection<Telemetering> telemetering;
        string pathxmlTelemetering = @"Config\Telemetering.xml";
        string pathxsdTelemetering = @"Config\Telemetering.xsd";

        private void TelemeteringExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataExport<Telemetering>(dataSetTelemetering, DataTypeEnum.Telemetering,
                  telemetering, pathxmlTelemetering);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "TelemeteringExport_Click");
            }
        }

        private void TelemeteringLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                telemetering = DataLoad<Telemetering>(ref pathxmlTelemetering, ref pathxsdTelemetering,
                ref dataSetTelemetering, DataTypeEnum.Telemetering, gridTelemetering);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "TelemeteringLoad_Click");
            }
        }

        /// <summary>
        /// 更新遥测数据
        /// </summary>
        private void UpdateTelemetering(APDU apdu)
        {
            try
            {
                var list = apdu.GetInformationList();

                var m = (ObservableCollection<Telemetering>)telemetering;

                switch ((TypeIdentification)apdu.ASDU.TypeId)
                {
                    case TypeIdentification.M_ME_NA_1://测量值，归一化值
                        {
                            break;
                        }
                    case TypeIdentification.M_ME_NC_1://测量值，短浮点数
                        {
                            foreach (var ele in list)
                            {
                                for (int k = 0; k < m.Count; k++)
                                {
                                    var t = m[k];
                                    if ((t.InternalID + Telemetering.BasicAddress - 1) == ele.Item1)
                                    {
                                        t.TelemeteringValue = (float)ele.Item2;
                                        t.TelemeteringID = (int)(t.InternalID + Telemetering.BasicAddress - 1);

                                    }

                                }
                            }
                            UpdateCalbrationData(list);
                            break;
                        }

                    case TypeIdentification.M_ME_TD_1://带CP56Time2a时标的测量值，归一化值
                    case TypeIdentification.M_ME_TF_1://带CP56Time2a时标的测量值，短浮点数
                        {
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
                MessageBox.Show(ex.Message, "UpdateTelemetering");
            }

        }

        /// <summary>
        /// 遥测右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgmenuTelemetering_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ContextMenu)
            {

                var m = e.Source as MenuItem;
                switch (m.Name)
                {
                    case "itemTelemetringLoadAs":
                        {
                            string path = "";
                            OpenXmlFile(ref path, "xml");

                            telemetering= DataLoad<Telemetering>(ref path, ref pathxsdTelemetering,
                ref dataSetTelemetering, DataTypeEnum.Telemetering, gridTelemetering);
                            break;
                        }
                    case "itemTelemetringSaveAs":
                        {
                            string path = "";
                            SaveXmlFile(ref path);
                            DataExport<Telemetering>(dataSetTelemetering, DataTypeEnum.Telemetering,
             telemetering, path);

                            break;
                        }
                    case "itemTelemetringAddUp":
                        {
                            if (gridTelemetering.SelectedIndex > -1)
                            {
                                var item = new Telemetering(0, "x", 0, 1, 1, "x", "x", "sd");
                                var obser = (ObservableCollection<Telemetering>)telemetering;


                                obser.Insert(gridTelemetering.SelectedIndex, item);


                            }


                            break;
                        }
                    case "itemTelemetringAddDown":
                        {

                            if (gridTelemetering.SelectedIndex > -1)
                            {
                                var item = new Telemetering(0, "x", 0, 1, 1, "x", "x", "sd");
                                var obser = (ObservableCollection<Telemetering>)telemetering;
                                if (gridTelemetering.SelectedIndex < gridTelemetering.Items.Count - 1)
                                {

                                    obser.Insert(gridTelemetering.SelectedIndex + 1, item);
                                }
                                else
                                {
                                    obser.Add(item);
                                }
                            }
                            break;
                        }
                    case "itemTelemetringDeleteSelect":
                        {
                            if (gridTelemetering.SelectedIndex > -1)
                            {


                                var result = MessageBox.Show("是否删除选中行:" + gridTelemetering.SelectedItem.ToString(),
                                    "确认删除", MessageBoxButton.OKCancel);
                                if (result == MessageBoxResult.OK)
                                {
                                    var obser = (ObservableCollection<Telemetering>)telemetering;
                                    obser.RemoveAt(gridTelemetering.SelectedIndex);
                                }
                            }
                            break;
                        }
                }

            }
        }


    }
}
