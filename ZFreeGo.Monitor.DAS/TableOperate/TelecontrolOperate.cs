using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ZFreeGo.Monitor.AutoStudio.ElementParam;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.Monitor.AutoStudio
{
    public partial class MainWindow
    {
        private DataSet dataSetTelecontrol;
        private ICollection<Telecontrol> telecontrol;
        private ObservableCollection<Telecontrol>  observableTelecontrol;
        private string pathxmlTelecontrol = @"Config\Telecontrol.xml";
        private string pathxsdTelecontrol = @"Config\Telecontrol.xsd";
        private int IndexTelecontrol = 1;
        /// <summary>
        /// 遥控状态记录
        /// </summary>
        private ControlRealyActionState controlReayActionState;
       /// <summary>
       /// 导出遥控表格
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void TelecontrolExport_Click(object sender, RoutedEventArgs e)
        {
            DataExport<Telecontrol>(dataSetTelecontrol, DataTypeEnum.Telecontrol,
              telecontrol, pathxmlTelecontrol);

        }
        /// <summary>
        /// 导入遥控表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TelecontrolLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                telecontrol = DataLoad<Telecontrol>(ref pathxmlTelecontrol, ref pathxsdTelecontrol,
                 ref dataSetTelecontrol, DataTypeEnum.Telecontrol, gridTelecontrol);
                observableTelecontrol = (ObservableCollection<Telecontrol>)telecontrol;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "TelecontrolExport_Click");
            }
        }


        /// <summary>
        /// 准备合闸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadyClose_Click(object sender, RoutedEventArgs e)
        {
            SendMasterCommand(CauseOfTransmissionList.Activation, new DoubleCommand(SelectExecuteOption.Select, QUtype.NODefine, DCOState.On),
               Telecontrol.BasicAddress + 1 -1);
            observableTelecontrol.Add(new Telecontrol(IndexTelecontrol++, "合闸预制", "开关", "合闸预制", "合闸预制", DateTime.Now.ToLongTimeString()));
            gridTelecontrol.ScrollIntoView(gridTelecontrol.Items[gridTelecontrol.Items.Count - 1]);
        }
        /// <summary>
        /// 执行合闸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionClose_Click(object sender, RoutedEventArgs e)
        {
            SendMasterCommand(CauseOfTransmissionList.Activation, new DoubleCommand(SelectExecuteOption.Execute, QUtype.NODefine, DCOState.On),
                Telecontrol.BasicAddress + 1 -1);
            observableTelecontrol.Add(new Telecontrol(IndexTelecontrol++, "合闸执行", "开关", "合闸执行", "合闸执行", DateTime.Now.ToLongTimeString()));
            gridTelecontrol.ScrollIntoView(gridTelecontrol.Items[gridTelecontrol.Items.Count - 1]);
        }
        /// <summary>
        /// 准备分闸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadyOpen_Click(object sender, RoutedEventArgs e)
        {
            SendMasterCommand(CauseOfTransmissionList.Activation, new DoubleCommand(SelectExecuteOption.Select, QUtype.NODefine, DCOState.Off),
              Telecontrol.BasicAddress + 1 - 1);
            observableTelecontrol.Add(new Telecontrol(IndexTelecontrol, "分闸预制", "开关", "分闸预制", "分闸预制", DateTime.Now.ToLongTimeString()));
            gridTelecontrol.ScrollIntoView(gridTelecontrol.Items[gridTelecontrol.Items.Count - 1]);
        }
        /// <summary>
        /// 执行分闸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionOpen_Click(object sender, RoutedEventArgs e)
        {
            SendMasterCommand(CauseOfTransmissionList.Activation, new DoubleCommand(SelectExecuteOption.Execute, QUtype.NODefine, DCOState.Off),
             Telecontrol.BasicAddress + 1 - 1);
            observableTelecontrol.Add(new Telecontrol(IndexTelecontrol, "分闸执行", "开关", "分执行", "分闸执行", DateTime.Now.ToLongTimeString()));
            gridTelecontrol.ScrollIntoView(gridTelecontrol.Items[gridTelecontrol.Items.Count - 1]);
        }

        /// <summary>
        /// 准备电池激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadyBaterryActivated_Click(object sender, RoutedEventArgs e)
        {
            SendMasterCommand(CauseOfTransmissionList.Activation, new DoubleCommand(SelectExecuteOption.Select, QUtype.NODefine, DCOState.On),
                    Telecontrol.BasicAddress + 2 - 1);
            observableTelecontrol.Add(new Telecontrol(IndexTelecontrol++, "电池活化", "设备", "电池活化准备", "电池活化执准备", DateTime.Now.ToLongTimeString()));
            gridTelecontrol.ScrollIntoView(gridTelecontrol.Items[gridTelecontrol.Items.Count - 1]);
        }
        /// <summary>
        /// 执行电池活化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionBaterryActivated_Click(object sender, RoutedEventArgs e)
        {
            SendMasterCommand(CauseOfTransmissionList.Activation, new DoubleCommand(SelectExecuteOption.Execute, QUtype.NODefine, DCOState.On),
                          Telecontrol.BasicAddress + 2 -1);
            observableTelecontrol.Add(new Telecontrol(IndexTelecontrol++, "电池活化", "设备", "电池活化执行", "电池活化执行", DateTime.Now.ToLongTimeString()));
            gridTelecontrol.ScrollIntoView(gridTelecontrol.Items[gridTelecontrol.Items.Count - 1]);
        }

        /// <summary>
        /// 复归预制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetExecute_Click(object sender, RoutedEventArgs e)
        {
            SendMasterCommand(CauseOfTransmissionList.Activation, new DoubleCommand(SelectExecuteOption.Select, QUtype.NODefine, DCOState.On),
              Telecontrol.BasicAddress + 3 -1);
            observableTelecontrol.Add(new Telecontrol(IndexTelecontrol++, "复归预制", "设备", "复归预制", "复归预制", DateTime.Now.ToLongTimeString()));
            gridTelecontrol.ScrollIntoView(gridTelecontrol.Items[gridTelecontrol.Items.Count - 1]);

        }
        /// <summary>
        /// 复归执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetReady_Click(object sender, RoutedEventArgs e)
        {
            SendMasterCommand(CauseOfTransmissionList.Activation, new DoubleCommand(SelectExecuteOption.Execute, QUtype.NODefine, DCOState.On),
             Telecontrol.BasicAddress + 3 -1);
            observableTelecontrol.Add(new Telecontrol(IndexTelecontrol++, "复归执行", "设备", "复归执行", "复归执行", DateTime.Now.ToLongTimeString()));
            gridTelecontrol.ScrollIntoView(gridTelecontrol.Items[gridTelecontrol.Items.Count - 1]);
        }


        /// <summary>
        /// 遥控状态初始化
        /// </summary>
        private void TelecontrolInit()
        {
             controlReayActionState = new ControlRealyActionState();
             ReayActionGroup.DataContext = controlReayActionState;

        }


        /// <summary>
        /// 更新遥控状态,更新相应的按钮状态。
        /// Todo:地址更新需要同步，这限制了灵活性
        /// </summary>
        /// <param name="apdu">APDU帧</param>
        private void UpdateTelecontrolState(APDU apdu)
        {
            try
            {
                UInt32 objectAddr = ElementTool.CombinationByte(apdu.ASDU.InformationObject[0], apdu.ASDU.InformationObject[1], apdu.ASDU.InformationObject[2]);
                var dc = new DoubleCommand(apdu.ASDU.InformationObject[3]);
                switch (objectAddr)
                {
                    //开关控制
                    case Telecontrol.BasicAddress:
                        {

                            if ((dc.DCS == DCOState.On) && (dc.SE == SelectExecuteOption.Select))
                            {
                                //合闸 选择
                                controlReayActionState.ActionCloseEnabled = true;
                                controlReayActionState.ReadyCloseEnabled = false;

                            }
                            else if ((dc.DCS == DCOState.On) && (dc.SE == SelectExecuteOption.Execute))
                            {
                                //合闸 执行
                                controlReayActionState.ActionCloseEnabled = false;
                                controlReayActionState.ReadyCloseEnabled = true;

                            }
                            if ((dc.DCS == DCOState.Off) && (dc.SE == SelectExecuteOption.Select))
                            {
                                //分闸 选择
                                controlReayActionState.ActionOpenEnabled = true;
                                controlReayActionState.ReadyOpenEnabled = false;

                            }
                            else if ((dc.DCS == DCOState.Off) && (dc.SE == SelectExecuteOption.Execute))
                            {
                                //分闸 执行
                                controlReayActionState.ActionOpenEnabled = false;
                                controlReayActionState.ReadyOpenEnabled = true;
                            }

                            break;
                        }
                        //电池活化
                    case (Telecontrol.BasicAddress + 1):
                        {
                            break;
                        }
                        //复归
                    case (Telecontrol.BasicAddress + 2):
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            catch (Exception  ex)
            {
                MessageBox.Show(ex.Message, "更新遥控状态");
            }
        }
    }
}
