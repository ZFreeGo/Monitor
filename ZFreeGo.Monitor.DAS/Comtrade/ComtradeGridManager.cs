using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using ZFreeGo.FileOperation.Comtrade;
using ZFreeGo.FileOperation.Comtrade.ConfigContent;

namespace ZFreeGo.Monitor.AutoStudio.Comtrade
{
    /// <summary>
    /// contrade 表格显示管理
    /// </summary>
    public class ComtradeGridManager
    {
        DataGrid gridRowStationRev;
        DataGrid gridRowChannelNumType;
        DataGrid gridRowAnalogChannelInformation;
        DataGrid gridRowDigitalChannelInformation;
        DataGrid gridRowChannelFrequency;
        DataGrid gridRowSampleNum;
        DataGrid gridRowSampleRateInformation;
        DataGrid gridRowFirstDateStamp;
        DataGrid gridRowTriggerDateStamp;
        DataGrid gridRowDataFileType;
        DataGrid gridRowTimeStampMultiply;



        ObservableCollection<StationRev> collectRowStationRev;
        ObservableCollection<ChannelNumType> collectRowChannelNumType;
        ObservableCollection<AnalogChannelInformation> collectRowAnalogChannelInformation;
        ObservableCollection<DigitalChannelInformation> collectRowDigitalChannelInformation;
        ObservableCollection<ChannelFrequency> collectRowChannelFrequency;
        ObservableCollection<SampleNum> collectRowSampleNum;
        ObservableCollection<SampleRateInformation> collectRowSampleRateInformation;
        ObservableCollection<DateStamp> collectRowFirstDateStamp;
        ObservableCollection<DateStamp> collectRowTriggerDateStamp;
        ObservableCollection<DataFileType> collectRowDataFileType;
        ObservableCollection<TimeStampMultiply> collectRowTimeStampMultiply;

        ComtradeFileManager comtradeManager;

        /// <summary>
        /// 获取配置文件
        /// </summary>
        public ComtradeFileManager ContradeManger
        {
            get
            {
                return comtradeManager;
            }
        }

        public ComtradeGridManager(
            DataGrid ingridRowStationRev,
            DataGrid ingridRowChannelNumType,
            DataGrid ingridRowAnalogChannelInformation,
            DataGrid ingridRowDigitalChannelInformation,
            DataGrid ingridRowChannelFrequency,
            DataGrid ingridRowSampleNum,
            DataGrid ingridRowSampleRateInformation,
            DataGrid ingridRowFirstDateStamp,
            DataGrid ingridRowTriggerDateStamp,
            DataGrid ingridRowDataFileType,
            DataGrid ingridRowTimeStampMultiply)
        {
            gridRowStationRev = ingridRowStationRev;
            gridRowChannelNumType = ingridRowChannelNumType;
            gridRowAnalogChannelInformation = ingridRowAnalogChannelInformation;
            gridRowDigitalChannelInformation = ingridRowDigitalChannelInformation;
            gridRowChannelFrequency = ingridRowChannelFrequency;
            gridRowSampleNum = ingridRowSampleNum;
            gridRowSampleRateInformation = ingridRowSampleRateInformation;
            gridRowFirstDateStamp = ingridRowFirstDateStamp;
            gridRowTriggerDateStamp = ingridRowTriggerDateStamp;
            gridRowDataFileType = ingridRowDataFileType;
            gridRowTimeStampMultiply = ingridRowTimeStampMultiply;
        }


        /// <summary>
        /// 读取Comtrade文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public void ReadComtradeFile(string path)
        {
            string cfgPath = System.IO.Path.Combine(path, "binary.cfg");
            string dataPath = System.IO.Path.Combine(path, "binary.dat");

            comtradeManager = new ComtradeFileManager();
            comtradeManager.ReadFile(cfgPath, dataPath);
            updateGrid();
       }
        /// <summary>
        /// 读取Comtrade文件
        /// </summary>
        /// <param name="cfgpath">配置文件路径</param>
        /// <param name="dataPath">数据文件路径</param>
        public void ReadComtradeFile(string cfgPath,string dataPath)
        {


            comtradeManager = new ComtradeFileManager();
            comtradeManager.ReadFile(cfgPath, dataPath);
            updateGrid();

            
        }
        /// <summary>
        /// 更新配置文件的列表显示关联信息
        /// </summary>
        private void updateGrid()
        {

            //var rowStationRev = new ObservableCollection<StationRev>();
            //rowStationRev.Add(comtradeManager.ConfigFile.RowStationRev);
            //gridRowStationRev.ItemsSource = rowStationRev;
            updateGrid<StationRev>(ref collectRowStationRev, 
                comtradeManager.ConfigFile.RowStationRev, gridRowStationRev);
            updateGrid<ChannelNumType>(ref collectRowChannelNumType,
            comtradeManager.ConfigFile.RowChannelNumType, gridRowChannelNumType);
            updateGrid<AnalogChannelInformation>(ref collectRowAnalogChannelInformation,
            comtradeManager.ConfigFile.RowAnalogChannel, gridRowAnalogChannelInformation);
            updateGrid<DigitalChannelInformation>(ref collectRowDigitalChannelInformation,
            comtradeManager.ConfigFile.RowDigitalChannel, gridRowDigitalChannelInformation);
            updateGrid<ChannelFrequency>(ref collectRowChannelFrequency,
            comtradeManager.ConfigFile.RowChannelFrequency, gridRowChannelFrequency);
            updateGrid<SampleNum>(ref collectRowSampleNum,
            comtradeManager.ConfigFile.RowSampleNum, gridRowSampleNum);
            updateGrid<SampleRateInformation>(ref collectRowSampleRateInformation,
            comtradeManager.ConfigFile.RowSampleRateInformation, gridRowSampleRateInformation);
            updateGrid<DateStamp>(ref collectRowFirstDateStamp,
            comtradeManager.ConfigFile.RowFirstDateStamp, gridRowFirstDateStamp);
            updateGrid<DateStamp>(ref collectRowTriggerDateStamp,
            comtradeManager.ConfigFile.RowTriggerDateStamp, gridRowTriggerDateStamp);
            updateGrid<DataFileType>(ref collectRowDataFileType,
            comtradeManager.ConfigFile.RowDataFileType, gridRowDataFileType);
            updateGrid<TimeStampMultiply>(ref collectRowTimeStampMultiply,
            comtradeManager.ConfigFile.RowTimeStampMultiply, gridRowTimeStampMultiply);



        }
        /// <summary>
        /// 更新列表显示
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="collect">显示合集</param>
        /// <param name="row">行信息</param>
        /// <param name="grid">对应表格</param>
        private void updateGrid<T>(ref ObservableCollection<T> collect, T row, DataGrid grid)
        {

            collect = new ObservableCollection<T>();
            collect.Add(row);
            grid.ItemsSource = collect;
        }
        /// <summary>
        /// 更新列表显示
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="collect">显示合集</param>
        /// <param name="row">行信息</param>
        /// <param name="grid">对应表格</param>
        private void updateGrid<T>(ref ObservableCollection<T> collect, T[] row, DataGrid grid)
        {

            collect = new ObservableCollection<T>();
            foreach(var m in row)
            {
                collect.Add(m);
            }
            grid.ItemsSource = collect;
        }
    }
}
