using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZFreeGo.Common.FileDialog;
using ZFreeGo.FileOperation.Comtrade;
using ZFreeGo.FileOperation.Comtrade.ConfigContent;
using ZFreeGo.FileOperation.Comtrade.DataContent;
using ZFreeGo.ParamMonitor.PlotCurves;

namespace ZFreeGo.Monitor.AutoStudio.Comtrade
{
    /// <summary>
    /// Comtrade.xaml 的交互逻辑
    /// </summary>
    public partial class ComtradeUI : Window
    {
        private ComtradeGridManager comtradeGridManager;
        private PlotModelManager plotModeManager;

        string defaultDirectory = @"c:\";
        public ComtradeUI()
        {
            InitializeComponent();
            defaultDirectory = System.IO.Directory.GetCurrentDirectory();
        }


     

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //TestComtrade();
            //sUpdateComtradeConfig();
        }
        void UpdateComtradeConfig()
        {
            string path = @"E:\WorkProject\04-FTU终端\Comtrade\ComtradeFile";
            comtradeGridManager = new ComtradeGridManager(
            gridRowStationRev,
             gridRowChannelNumType,
             gridRowAnalogChannelInformation,
             gridRowDigitalChannelInformation,
             gridRowChannelFrequency,
             gridRowSampleNum,
             gridRowSampleRateInformation,
             gridRowFirstDateStamp,
             gridRowTriggerDateStamp,
             gridRowDataFileType,
             gridRowTimeStampMultiply);

            comtradeGridManager.ReadComtradeFile(path);
        }

        void TestComtrade()
        {

            Console.WriteLine("TestComtradeManager-TestStart");
            string path = @"E:\WorkProject\04-FTU终端\Comtrade\ComtradeFile";
            string cfgPath = System.IO.Path.Combine(path, "binary.cfg");
            string dataPath = System.IO.Path.Combine(path, "binary.dat");
            Console.WriteLine("In:");
            Console.WriteLine(path);
            Console.WriteLine(cfgPath);
            Console.WriteLine(dataPath);

            var comtradeManager = new ComtradeFileManager();
            comtradeManager.ReadFile(cfgPath, dataPath);


            Console.WriteLine("Out:");
            Console.WriteLine("配置文件内容:");
            string[] cfgRow;
            comtradeManager.ConfigFile.MakeConfigFile(out cfgRow);
             
            for (int index = 0; index < cfgRow.Length; index++)
            {
                Console.Write(string.Format("{0}：" + cfgRow[index], index + 1));

            }
            Console.WriteLine("数据文件内容:");
            for (int i = 0; i < comtradeManager.DataFile.AsciiData.Count; i++)
            {
                var ascii = new ASCIIContent(comtradeManager.DataFile.AsciiData[i],
                    comtradeManager.ConfigFile.RowChannelNumType.AnalogChannelCount,
                    comtradeManager.ConfigFile.RowChannelNumType.DigitalChannelCount);

                Console.Write(ascii.RowToString());
            }
            for (int j = 0; j < comtradeManager.DataFile.BinaryData.Count; j++)
            {
                var binanry = new BinaryContent(comtradeManager.DataFile.BinaryData[j],
                      comtradeManager.ConfigFile.RowChannelNumType.AnalogChannelCount,
                    comtradeManager.ConfigFile.RowChannelNumType.DigitalChannelCount);
                var assii = new ASCIIContent(binanry);
                Console.Write(assii.RowToString());
            }


            Console.WriteLine("TestComtradeManager-TestEnd");

            //gridRowStationRev.ItemsSource

            //var rowStationRev = new ObservableCollection<StationRev>();
            //rowStationRev.Add(comtradeManager.ConfigFile.RowStationRev);
            //gridRowStationRev.ItemsSource = rowStationRev;

            


        }

        /// <summary>
        /// 打开comtrade文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenComtradeFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                string filter = "config files (*.cfg)|*.cfg|data files (*.dat)|*.dat|All files (*.*)|*.*";
                int index = 1;                
                string path = FileOperateDialog.OpenFileDialog(defaultDirectory, filter, index, true);
                    
                string dir = System.IO.Path.GetDirectoryName(path);
                defaultDirectory = dir;
                string filename = System.IO.Path.GetFileNameWithoutExtension(path);
                string cfgPath = System.IO.Path.Combine(dir, filename + ".cfg");
                string dataPath = System.IO.Path.Combine(dir, filename + ".dat");


                if (!System.IO.File.Exists(cfgPath))
                {
                    throw new Exception("不存在选择的配置文件");
                }
                if (!System.IO.File.Exists(dataPath))
                {
                    throw new Exception("不存在选择的数据文件");
                }

                comtradeGridManager = new ComtradeGridManager(
                gridRowStationRev,
                 gridRowChannelNumType,
                 gridRowAnalogChannelInformation,
                 gridRowDigitalChannelInformation,
                 gridRowChannelFrequency,
                 gridRowSampleNum,
                 gridRowSampleRateInformation,
                 gridRowFirstDateStamp,
                 gridRowTriggerDateStamp,
                 gridRowDataFileType,
                 gridRowTimeStampMultiply);


                comtradeGridManager.ReadComtradeFile(cfgPath, dataPath);


                UpdataTxtConfigShow();
                initPlot();
                plotModeManager.LoadShowData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btnOpenComtradeFile");
            }
        }

        /// <summary>
        /// 更新显示配置文件
        /// </summary>
        void UpdataTxtConfigShow()
        {
            string[] str;
            if (comtradeGridManager.ContradeManger != null)
            {
                comtradeGridManager.ContradeManger.ConfigFile.MakeConfigFile(out str);
                foreach (var m in str)
                {
                    txtConfigShow.Text += m;
                }
            }
        }

        private void btnShowDataGrid_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnShowDataCurve_Click(object sender, RoutedEventArgs e)
        {

            

        }

        /// <summary>
        /// 初始化曲线配置
        /// </summary>
        void initPlot()
        {
            if (comtradeGridManager != null)
            {
                //先删除
                if (plotModeManager != null)
                {
                    foreach (var m in plotModeManager.PlotViewCollect)
                    {
                        stackDataPlot.Children.Remove(m);
                    }
                }
                plotModeManager = new PlotModelManager(comtradeGridManager.ContradeManger.ConfigFile, 
                    comtradeGridManager.ContradeManger.DataFile);
                
                
                foreach (var m in plotModeManager.PlotViewCollect)
                {
                    stackDataPlot.Children.Add(m);
                }

            }
        }

        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveComtradeFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              
                string filter = "config files (*.cfg)|*.cfg|data files (*.dat)|*.dat|All files (*.*)|*.*";
                int index = 1;
                string path = FileOperateDialog.SaveFileDialog(defaultDirectory, filter, index, true);
                string dir = System.IO.Path.GetDirectoryName(path);
                defaultDirectory = dir;
                string filename = System.IO.Path.GetFileNameWithoutExtension(path);
                string cfgPath = System.IO.Path.Combine(dir, filename + ".cfg");
                string dataPath = System.IO.Path.Combine(dir, filename + ".dat");

                //TestData();
                comtradeGridManager.ContradeManger.WriteFile(cfgPath, dataPath);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "btnSaveComtradeFile_Click");
            }
        }

        void TestData()
        {
            comtradeGridManager.ContradeManger.DataFile.AsciiData.Clear();
            comtradeGridManager.ContradeManger.ConfigFile.RowDataFileType.DataType = DataFileType.ASCII;
            comtradeGridManager.ContradeManger.ConfigFile.RowChannelNumType.SetChannelNum(12, 6, 6);
            comtradeGridManager.ContradeManger.ConfigFile.RowSampleRateInformation.EndSample = "1000";
            for(int i = 0; i< comtradeGridManager.ContradeManger.ConfigFile.RowSampleRateInformation.EndSampleCount; i++)
            {
                int x = i * 100;
                double rad = (double)x/10000 * 2*Math.PI;
                int a = (int)(Math.Cos(rad) * 2048);
                int b = (int)(Math.Cos(rad + Math.PI/3) * 2048);
                int c = (int)(Math.Cos(rad + 2 *Math.PI/3) * 2048);
                int d = (int)(Math.Cos(rad + 2 *Math.PI/3 +Math.PI/6 ) * 2048);
                int e = (int)(Math.Cos(rad + Math.PI/3 +Math.PI/6) * 2048);
                int f = (int)(Math.Cos(rad +Math.PI/6)*2048);
                int da = (int)Math.Abs(Math.Cos(rad + Math.PI / 3) * 1.9);
                int db = (int)Math.Abs(Math.Cos(rad + Math.PI / 3) * 1.9);
                int dc = (int)Math.Abs(Math.Cos(rad + 2 * Math.PI / 3) * 1.9);
                int dd = (int)Math.Abs(Math.Cos(rad + 2 * Math.PI / 3 + Math.PI / 6) * 1.9);
                int de = (int)Math.Abs(Math.Cos(rad + Math.PI / 3 + Math.PI / 6) * 1.9);
                int df = (int)Math.Abs(Math.Cos(rad + Math.PI / 6) * 1.9);




                string str = i.ToString() + "," + x.ToString() + "," + a.ToString() + "," +b.ToString() + "," +
                              c.ToString() + "," + d.ToString() + "," +
                               e.ToString() + "," +f.ToString() + "," +
                                 da.ToString() + "," +db.ToString() + "," +
                              dc.ToString() + "," + dd.ToString() + "," +
                               de.ToString() + "," +df.ToString() + "\n";

                comtradeGridManager.ContradeManger.DataFile.AsciiData.Add(str);
            }
        }
    }
}
