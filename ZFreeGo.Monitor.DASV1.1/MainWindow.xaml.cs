using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZFreeGo.Monitor.AutoStudio.ElementParam;
using ZFreeGo.Monitor.CommCenter;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ApplicationMessage;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;
using ZFreeGo.Monitor.AutoStudio.Comtrade;
using ZFreeGo.Monitor.AutoStudio.StartupUI;
using ZFreeGo.Monitor.AutoStudio.OptionConfig;
using ZFreeGo.Monitor.AutoStudio.Secure;
using ZFreeGo.Monitor.AutoStudio.Database;
using ZFreeGo.Monitor.AutoStudio.Log;

namespace ZFreeGo.Monitor.AutoStudio
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑。
    /// </summary>
    public partial class MainWindow : Window
    {
        XMLOperate getData = new XMLOperate();
        private SerialControlCenter serialControlCenter; //用于串口通讯

        //循环召唤定时器
        private Timer loopCallTimer;
        /// <summary>
        /// 时间定时器
        /// </summary>
        private Timer timeClock;
        /// <summary>
        /// 时钟时间
        /// </summary>
        private ClockElement clockElement;


        /// <summary>
        /// 通讯状态
        /// </summary>
        private bool DeviceCommState = false;

        /// <summary>
        /// 账户管理器
        /// </summary>
        private AccountManager accountManager;

        /// <summary>
        /// 日志记录
        /// </summary>
        private Log.Logger logger;

        /// <summary>
        /// 权限管理
        /// </summary>
        private AuthorityManager authorityManager;

        /// <summary>
        /// 日志产生事件
        /// </summary>
        public event EventHandler<Log.LogEventArgs> MakeLogEvent;





        public MainWindow(AccountManager inAccountManager, Log.Logger inLogger)
        {
            InitializeComponent();
            TestWave();
            accountManager = inAccountManager;
            logger = inLogger;

            authorityManager = new AuthorityManager(accountManager);
            AuthorityManagerAddControl();
            authorityManager.LoadAuthorityData();

            authorityManager.GetPermission((AuthorityLevel)accountManager.LoginAccount.PowerLevel);


            MakeLogEvent += MainWindow_MakeLogEvent;

            InitCallServer();
        }


        void MainWindow_MakeLogEvent(object sender, LogEventArgs e)
        {
            logger.AddMessage(e.Message);
        }

        /// <summary>
        /// 产生日志消息
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="content">操作内容</param>
        /// <param name="result">操作结果</param>
        private void MakeLogMessage(object sender, string content, LogType type)
        {
            MakeLogMessage(sender, content, "", type);
        }
        /// <summary>
        /// 产生日志消息
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="content">操作内容</param>
        /// <param name="result">操作结果</param>
        private void MakeLogMessage(object sender, string content, string result, LogType type)
        {
            try
            {
                if (MakeLogEvent != null)
                {
                    var message = new SingleLogMessage(accountManager.LoginAccount.UserName, content, result, type);
                    MakeLogEvent(sender, new Log.LogEventArgs(message));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "日志消息");
            }
        }

        /// <summary>
        /// 添加控件到控件管理器
        /// </summary>
        private void AuthorityManagerAddControl()
        {
            try
            {
                //系统参数
                authorityManager.AddControl(this.RefreshSystemParameter, AuthorityLevel.I);
                authorityManager.AddControl(this.SystemParameterLoad, AuthorityLevel.II);
                authorityManager.AddControl(this.SystemParameterExport, AuthorityLevel.II);
                //通讯设置
                authorityManager.AddControl(this.checkIsSub, AuthorityLevel.II);
                authorityManager.AddControl(this.radioServer, AuthorityLevel.II);
                authorityManager.AddControl(this.radioClient, AuthorityLevel.II);
                authorityManager.AddControl(this.txtIp, AuthorityLevel.II);
                authorityManager.AddControl(this.txtPort, AuthorityLevel.II);
                authorityManager.AddControl(this.checkIsReStart, AuthorityLevel.II);

                authorityManager.AddControl(this.btnStartServer, AuthorityLevel.II);
                authorityManager.AddControl(this.btnStopServer, AuthorityLevel.II);
                authorityManager.AddControl(this.btnStartDataTransmission, AuthorityLevel.II);
                authorityManager.AddControl(this.btnStopDataTransmission, AuthorityLevel.II);
                authorityManager.AddControl(this.btnManualCall, AuthorityLevel.II);
                authorityManager.AddControl(this.btnTimeSynA, AuthorityLevel.II);
                authorityManager.AddControl(this.btnRestartServer, AuthorityLevel.II);


                //遥信
                authorityManager.AddControl(this.btnTelesignalisationCall, AuthorityLevel.II);
                authorityManager.AddControl(this.TelesignalisationExport, AuthorityLevel.II);
                authorityManager.AddControl(this.TelesignalisationLoad, AuthorityLevel.II);
                authorityManager.AddControl(this.CheckBoxTelesignalisation, AuthorityLevel.II);


                //遥测
                authorityManager.AddControl(this.btnTelemeteringCall, AuthorityLevel.II);
                authorityManager.AddControl(this.TelemeteringExport, AuthorityLevel.II);
                authorityManager.AddControl(this.TelemeteringLoad, AuthorityLevel.II);
                authorityManager.AddControl(this.CheckBoxTelemetering, AuthorityLevel.II);

                //遥控
                authorityManager.AddControl(this.checkTimeSyn, AuthorityLevel.II);
                authorityManager.AddControl(this.btnTimeSyn, AuthorityLevel.II);
                authorityManager.AddControl(this.txtPassWord, AuthorityLevel.II);
                authorityManager.AddControl(this.btnMakeSecure, AuthorityLevel.II);

                authorityManager.AddControl(this.ReadyClose, AuthorityLevel.II);
                authorityManager.AddControl(this.ActionClose, AuthorityLevel.II);
                authorityManager.AddControl(this.ReadyOpen, AuthorityLevel.II);
                authorityManager.AddControl(this.ActionOpen, AuthorityLevel.II);
                authorityManager.AddControl(this.ReadyBaterryActivated, AuthorityLevel.II);
                authorityManager.AddControl(this.ActionBaterryActivated, AuthorityLevel.II);
                authorityManager.AddControl(this.btnResetReady, AuthorityLevel.II);
                authorityManager.AddControl(this.btnResetExecute, AuthorityLevel.II);
                authorityManager.AddControl(this.TelecontrolExport, AuthorityLevel.II);
                authorityManager.AddControl(this.TelecontrolLoad, AuthorityLevel.II);

                //遥控操作
                authorityManager.AddControl(this.btnCallSetpoint, AuthorityLevel.II);
                authorityManager.AddControl(this.DownloadProtectSetSelect, AuthorityLevel.II);
                authorityManager.AddControl(this.DownloadProtectSet, AuthorityLevel.II);
                authorityManager.AddControl(this.ProtectSetPointExport, AuthorityLevel.II);
                authorityManager.AddControl(this.ProtectSetPointLoad, AuthorityLevel.II);
                authorityManager.AddControl(this.CheckBoxProtectSetPoint, AuthorityLevel.II);

                //系统校准
                authorityManager.AddControl(this.btnStartCalibration, AuthorityLevel.II);
                authorityManager.AddControl(this.btnCalibrationCall, AuthorityLevel.II);
                authorityManager.AddControl(this.CheckBoxRealUpdate, AuthorityLevel.II);
                authorityManager.AddControl(this.txtUpdateTime, AuthorityLevel.II);
                authorityManager.AddControl(this.CheckBoxSystemCalibration, AuthorityLevel.II);

                authorityManager.AddControl(this.btnFactorRead, AuthorityLevel.II);
                authorityManager.AddControl(this.btnFactorDownload, AuthorityLevel.II);
                authorityManager.AddControl(this.btnFacotrFix, AuthorityLevel.II);

                authorityManager.AddControl(this.btnCalibrationExport, AuthorityLevel.II);
                authorityManager.AddControl(this.btnCalibrationLoad, AuthorityLevel.II);


                //事件记录
                authorityManager.AddControl(this.btnClearSOE, AuthorityLevel.II);
                authorityManager.AddControl(this.EventLogLoad, AuthorityLevel.II);
                authorityManager.AddControl(this.EventLogExport, AuthorityLevel.II);
                authorityManager.AddControl(this.CheckBoxEventLog, AuthorityLevel.II);

                //录波采集
                authorityManager.AddControl(this.btnRefremRecord, AuthorityLevel.II);
                authorityManager.AddControl(this.btnRecordLoad, AuthorityLevel.II);
                authorityManager.AddControl(this.btnRecordExport, AuthorityLevel.II);
                authorityManager.AddControl(this.btnConfigComtrade, AuthorityLevel.II);
                //

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AuthorityManagerAddControl");
            }

        }

        /// <summary>
        /// 打开XML配置文件
        /// </summary>
        /// <param name="path">路劲</param>
        /// <param name="style">类型</param>
        /// <returns>是否成功打开</returns>
        private bool OpenXmlFile(ref string path, string style)
        {
            Stream myStream;
            var openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = style + "files   (*." + style + ")|*." + style;
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    using (myStream)
                    {
                        path = openFileDialog1.FileName;
                        return true;
                    }

                }
            }
            return false;

        }
        /// <summary>
        /// 存储XML文件
        /// </summary>
        /// <param name="path">存储路径</param>
        /// <returns>是否成功存储</returns>
        private bool SaveXmlFile(ref string path)
        {
            Stream myStream;
            var saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "xml files   (*.xml)|*.xml";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == true)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    using (myStream)
                    {
                        path = saveFileDialog1.FileName;
                        return true;
                    }

                }
            }
            return false;

        }
        /// <summary>
        /// 检测文件是否存在，若不存在选择一个文件。
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="style"></param>
        /// <returns>文件是否存在</returns>
        private bool IsExists(ref string path, string style)
        {
            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                if (OpenXmlFile(ref path, style))
                {
                    return true;
                }
                else
                {
                    throw new Exception("未选择文件");
                }
            }


        }
        /// <summary>
        /// 将数据导出
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="ds">数据表格</param>
        /// <param name="en">数据类型</param>
        /// <param name="observa">数据列表</param>
        /// <param name="path">路径</param>
        private void DataExport<T>(DataSet ds, DataTypeEnum en, ICollection<T> observa, string path)
        {
            try
            {
                getData.UpdateTable(ds, en, observa);
                ds.WriteXml(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "DataExport");
            }
        }
        /// <summary>
        /// 载入数据列表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="pathxml">xml路径</param>
        /// <param name="pathxsd">xsd路径</param>
        /// <param name="ds">数据表</param>
        /// <param name="en">数据类型</param>
        /// <param name="dg">表格</param>
        /// <returns>返回可观察的列表</returns>
        private ICollection<T> DataLoad<T>(ref string pathxml, ref string pathxsd, ref DataSet ds, DataTypeEnum en, DataGrid dg)
        {
            try
            {


                if (IsExists(ref pathxml, "xml") && IsExists(ref pathxsd, "xsd"))
                {
                    ds = getData.ReadXml(pathxml, pathxsd);
                    var icollect = getData.GetDataCollect(ds, en) as ICollection<T>;
                    if (icollect != null)
                    {
                        dg.ItemsSource = icollect;
                        return icollect;
                    }
                }
                return null;


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, " DataLoad");
                return null;
            }
        }


        /// <summary>
        /// 窗体载入后加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {



            serialControlCenter = new SerialControlCenter();
            UpdatePortShow(serialControlCenter.SerialPort);
            // serialControlCenter.RtuFrameArrived += serialControlCenter_RtuFrameArrived;

            NetInit();
            ControlProcessConfig();
            TelesignalisationLoad_Click(null, null);
            TelemeteringLoad_Click(null, null);
            ProtectSetPointLoad_Click(null, null);
            SystemCalibrationLoad_Click(null, null);
            SystemParameterLoad_Click(null, null);
            TelecontrolLoad_Click(null, null);
            EventlogLoad_Click(null, null);

            loopCallTimer = new Timer(10000);
            loopCallTimer.Elapsed += loopCallTimer_Elapsed;
            loopCallTimer.AutoReset = true;



            timeClock = new Timer(1000);
            timeClock.Elapsed += timeClock_Elapsed;
            timeClock.Start();
            clockElement = new ClockElement(DateTime.Now);
            checkTimeSyn.IsChecked = true;

            TelecontrolInit();

            updateAccountShow();
        }
        /// <summary>
        /// 时钟定时器，按秒运作，检测网络是否正常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeClock_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action act = () =>
            {
                if (checkTimeSyn.IsChecked == true)
                {
                    clockElement = new ClockElement(DateTime.Now);
                    stackTimeShow.DataContext = clockElement;
                }
                //判断状态
                var state = PingAddress(txtIp.Text, 100);
                if (DeviceCommState != state)
                {
                    DeviceCommState = state;
                    UpdateDeviceComStatusBar(DeviceCommState);
                }
            };
            Dispatcher.BeginInvoke(act);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            StopProcessList();
            //关闭网络
            NetUNPStop();
            NetCalibrationStop();

            if (checkGetMessage != null)
            {

                checkGetMessage.Close();
            }
            logger.SaveLog(true);
            MakeLogMessage(this, "退出窗口", LogType.Login);
            accountManager.SaveAccountInformation();
        }

        /// <summary>
        /// 跨线程更新显示
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="dsc">描述</param>
        void BeginInvokeUpdateHistory(byte[] data, int len, string dsc)
        {
            string str = DateTime.Now.ToLongTimeString() + " " + dsc + ": ";
            for (int i = 0; i < len; i++)
            {
                str += data[i].ToString("X2") + " ";
            }
            str += "\n";
            Action<string> myDelegate = UpdateHistoryRecord;
            Dispatcher.BeginInvoke(myDelegate, str);
        }
        /// <summary>
        /// 跨线程更新显示
        /// </summary>
        /// <param name="dsc">描述</param>
        void BeginInvokeUpdateHistory(string dsc)
        {
            string str = DateTime.Now.ToLongTimeString() + " " + dsc;
            str += "\n";
            Action<string> myDelegate = UpdateHistoryRecord;
            Dispatcher.BeginInvoke(myDelegate, str);
        }
        /// <summary>
        /// 更新历史记录
        /// </summary>
        /// <param name="str">显示的字符串</param>
        void UpdateHistoryRecord(string str)
        {
            historyRecord.Text += str;
            historyRecord.ScrollToEnd();
        }
        /// <summary>
        /// 清空记录板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearStateMessage_Click(object sender, RoutedEventArgs e)
        {
            historyRecord.Text = "";

        }




        /// <summary>
        /// 手动召唤
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnManualCall_Click(object sender, RoutedEventArgs e)
        {
            //gridTelesignalisation.Background = gridTelesignalisation.Background;
            SendMasterCommand(CauseOfTransmissionList.Activation, QualifyOfInterrogationList.GeneralInterrogation);
        }
        /// <summary>
        /// 时间同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTimeSyn_Click(object sender, RoutedEventArgs e)
        {
            SendMasterCommand(CauseOfTransmissionList.Activation, new CP56Time2a(clockElement.TimeClock));
        }

        /// <summary>
        /// 请求脉冲
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCallCalculate_Click(object sender, RoutedEventArgs e)
        {
            SendMasterCommand(CauseOfTransmissionList.Activation, new QualifyCalculateCommad(QCCRequest.Request1, QCCFreeze.Read));
        }

        /// <summary>
        /// 更新电能脉冲参数
        /// </summary>
        private void UpdateElectricEnergy(APDU apdu)
        {
            try
            {
                var list = apdu.GetObjectListObject();


                string str = "";
                if (apdu.ASDU.IsSequence)
                {
                    var addr = ElementTool.CombinationByte(apdu.ASDU.InformationObject[0],
                        apdu.ASDU.InformationObject[1], apdu.ASDU.InformationObject[2]);
                    foreach (var m in list)
                    {
                        var sf = new ShortFloating(m);
                        var q = new QualityDescription(m[5]);
                        str += "电能召唤:" + addr.ToString("X2") + " 品质描述:" + q.QDS.ToString("X2") + " 值:" + sf.ToString();
                    }

                }


                //txtTest.Text = "";
                //txtTest.Text = str;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " UpdateElectricEnergy");
            }

        }


        /// <summary>
        /// 修改选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxTelesignalisation_Clicked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxTelesignalisation.IsChecked == true)
            {
                gridTelesignalisation.IsReadOnly = false;

                columnStateA.Visibility = System.Windows.Visibility.Visible;
                columnStateB.Visibility = System.Windows.Visibility.Visible;
                dgmenu.IsEnabled = true;
            }
            else if (CheckBoxTelesignalisation.IsChecked == false)
            {
                gridTelesignalisation.IsReadOnly = true;

                columnStateA.Visibility = System.Windows.Visibility.Hidden;
                columnStateB.Visibility = System.Windows.Visibility.Hidden;
                dgmenu.IsEnabled = false;
            }
        }

        private void CheckBoxTelemetering_Clicked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxTelemetering.IsChecked == true)
            {
              
                gridTelemetering.IsReadOnly = false;
                dgmenuTelemetering.IsEnabled = true;

            }
            else if (CheckBoxTelemetering.IsChecked == false)
            {
                gridTelemetering.IsReadOnly = true;
                dgmenuTelemetering.IsEnabled = false;
            }
        }

        private void CheckBoxProtectSetPoint_Clicked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxProtectSetPoint.IsChecked == true)
            {
                gridProtectSetPoint.IsReadOnly = false;

            }
            else if (CheckBoxProtectSetPoint.IsChecked == false)
            {
                gridProtectSetPoint.IsReadOnly = true;
            }
        }

        private void CheckBoxSystemCalibration_Clicked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxSystemCalibration.IsChecked == true)
            {
                gridSystemCalibration.IsReadOnly = false;

            }
            else if (CheckBoxSystemCalibration.IsChecked == false)
            {
                gridSystemCalibration.IsReadOnly = true;
            }
        }
        /// <summary>
        /// 事件记录修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxEventLog_Clicked(object sender, RoutedEventArgs e)
        {
            if (CheckBoxEventLog.IsChecked == true)
            {
                gridEventLog.IsReadOnly = false;

            }
            else if (CheckBoxEventLog.IsChecked == false)
            {
                gridEventLog.IsReadOnly = true;
            }
        }
        /// <summary>
        /// 清除SOE事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearSOE_Click(object sender, RoutedEventArgs e)
        {
            gridEventLog.Items.Clear();
        }



        /// <summary>
        /// 循环调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loopCallTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (SendCount++ < 10)
            {
                SendMasterCommand(CauseOfTransmissionList.Activation, QualifyOfInterrogationList.GeneralInterrogation);

            }
            else
            {

            }
        }

        /// <summary>
        /// 更新设备状态栏状态
        /// </summary>
        /// <param name="state">true-设备在线， false-设备离线</param>
        private void UpdateDeviceOnlineStatusBar(bool state)
        {
            if (state)
            {
                DeviceStateShowIndicate.Fill = new SolidColorBrush(Colors.Green);
                DeviceStateShow.Foreground = new SolidColorBrush(Colors.Green);
                DeviceStateShow.Text = "设备在线";

            }
            else
            {
                DeviceStateShowIndicate.Fill = new SolidColorBrush(Colors.Red);
                DeviceStateShow.Foreground = new SolidColorBrush(Colors.Red);
                DeviceStateShow.Text = "设备离线";
            }
        }
        /// <summary>
        /// 更新设备通讯状态栏状态
        /// </summary>
        /// <param name="state">true-通讯建立， false-通讯断开</param>
        private void UpdateDeviceComStatusBar(bool state)
        {
            if (state)
            {
                ComShowIndicate.Fill = new SolidColorBrush(Colors.Green);
                ComShow.Foreground = new SolidColorBrush(Colors.Green);
                ComShow.Text = "通讯正常";
            }
            else
            {
                ComShowIndicate.Fill = new SolidColorBrush(Colors.Red);
                ComShow.Foreground = new SolidColorBrush(Colors.Red);
                ComShow.Text = "通信断开";
            }
        }

        /// <summary>
        /// 重新启动服务，复位Tcp连接与接收处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRestartServer_Click(object sender, RoutedEventArgs e)
        {
            ResetServer();
        }
        /// <summary>
        /// 启动数据传输
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartDataTransmission_Click(object sender, RoutedEventArgs e)
        {
            SendTCFCommand(TransmissionCotrolFunction.StartDataTransmission);

        }
        /// <summary>
        /// 停止数据传输
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopDataTransmission_Click(object sender, RoutedEventArgs e)
        {
            //停止数据传输与此同时复归服务
            SendTCFCommand(TransmissionCotrolFunction.StopDataTransmission);
            //复归服务
            ResetServer();

        }

        /// <summary>
        /// 配置文件测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btncofigComtrade_Click(object sender, RoutedEventArgs e)
        {
            ComtradeUI ui = new ComtradeUI();
            ui.Show();
        }





        private void treeMessage_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // var tree = sender as TreeView;
            // var selectedItem = tree.SelectedItem as TreeViewItem;
            // MessageBox.Show(selectedItem.Header.ToString());
            try
            {
                if (sender is TreeView)
                {
                    var tree = sender as TreeView;
                    if (tree.SelectedItem is TreeViewItem)
                    {
                        var selectedItem = tree.SelectedItem as TreeViewItem;
                        switch (selectedItem.Header.ToString())
                        {
                            case "FTU类型":
                                {
                                    tabUI.SelectedItem = tabItemSystemParameter;
                                    break;
                                }
                            case "通讯信息":
                                {
                                    tabUI.SelectedItem = tabItemCommunication;
                                    break;
                                }
                            case "遥信":
                                {
                                    tabUI.SelectedItem = tabItemTelecommand;
                                    break;
                                }
                            case "遥测":
                                {
                                    tabUI.SelectedItem = tabItemTelemetering;
                                    break;
                                }
                            case "遥控":
                                {
                                    tabUI.SelectedItem = tabItemTelecontrol;
                                    break;
                                }
                            case "事件记录":
                                {
                                    tabUI.SelectedItem = tabItemEventLog;
                                    break;
                                }
                            case "录波采集":
                                {
                                    tabUI.SelectedItem = tabItemRecordWave;
                                    break;
                                }
                            case "保护定值":
                                {
                                    tabUI.SelectedItem = tabItemProtectSet;
                                    break;
                                }
                            case "系统校准":
                                {
                                    tabUI.SelectedItem = tabItemSystemCalibration;
                                    break;
                                }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "treeMessage_SelectedItemChanged");
            }

        }

        /// <summary>
        /// 选项功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OptionMenuItem_Click(object sender, RoutedEventArgs e)
        {

            var option = new OptionConfigUI(accountManager);
            option.MakeLogEvent += MainWindow_MakeLogEvent;
            option.ShowInTaskbar = false;
            option.ShowDialog();
        }


        /// <summary>
        /// 更新当前用户名
        /// </summary>
        private void updateAccountShow()
        {
            currentUserName.Text = accountManager.LoginAccount.UserName;
        }

        private void AuthorityMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var ui = new AuthoritySettingUI(authorityManager, this);
            ui.ShowInTaskbar = false;
            ui.Show();

        }

        /// <summary>
        /// 数据库配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatabaseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //var database = new SQLliteDatabase();
            //database.ConnectDatabase();

        }

       

    

      



    }
}
