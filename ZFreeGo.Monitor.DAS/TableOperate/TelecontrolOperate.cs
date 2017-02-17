using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ZFreeGo.Monitor.AutoStudio.ElementParam;
using ZFreeGo.Monitor.AutoStudio.Log;


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

        }
        /// <summary>
        /// 执行合闸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionClose_Click(object sender, RoutedEventArgs e)
        {
            
        }
        /// <summary>
        /// 准备分闸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadyOpen_Click(object sender, RoutedEventArgs e)
        {
           
        }
        /// <summary>
        /// 执行分闸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionOpen_Click(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// 准备电池激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadyBaterryActivated_Click(object sender, RoutedEventArgs e)
        {
       
        }
        /// <summary>
        /// 执行电池活化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActionBaterryActivated_Click(object sender, RoutedEventArgs e)
        {
           
        }

        /// <summary>
        /// 复归预制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetExecute_Click(object sender, RoutedEventArgs e)
        {
           

        }
        /// <summary>
        /// 复归执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetReady_Click(object sender, RoutedEventArgs e)
        {
            
        }


        /// <summary>
        /// 遥控状态初始化
        /// </summary>
        private void TelecontrolInit()
        {
             controlReayActionState = new ControlRealyActionState();
             ReayActionGroup.DataContext = controlReayActionState;

        }


       
    }
}
