using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using ZFreeGo.Monitor.DASModel.DataItemSet;

namespace ZFreeGo.Monitor.DASModel.GetViewData
{
    /// <summary>
    /// 串口参数列表
    /// </summary>
    public class SerialPortParameterItem : ObservableObject
    {

        /// <summary>
        /// 波特率
        /// </summary>
        public ObservableCollection<SerialPortParamer<int>> Baud;

        /// <summary>
        /// 数据位
        /// </summary>
        public ObservableCollection<SerialPortParamer<int>> DataBit;

        /// <summary>
        /// 校验位
        /// </summary>
        public ObservableCollection<SerialPortParamer<Parity>> ParityBit;

        /// <summary>
        /// 停止位
        /// </summary>
        public ObservableCollection<SerialPortParamer<StopBits>> StopBit;


        /// <summary>
        /// 串口号
        /// </summary>
        public ObservableCollection<SerialPortParamer<String>> CommonPort; 
        /// <summary>
        /// 初始化一个串口参数合集
        /// </summary>
        public SerialPortParameterItem()
        {
            Baud = new ObservableCollection<SerialPortParamer<int>>();
            Baud.Add(new SerialPortParamer<int>(1200));
            Baud.Add(new SerialPortParamer<int>(2400));
            Baud.Add(new SerialPortParamer<int>(4800));
            Baud.Add(new SerialPortParamer<int>(9600));
            Baud.Add(new SerialPortParamer<int>(14400));
            Baud.Add(new SerialPortParamer<int>(28800));
            Baud.Add(new SerialPortParamer<int>(38400));
            Baud.Add(new SerialPortParamer<int>(57600));
            Baud.Add(new SerialPortParamer<int>(115200));


            DataBit = new ObservableCollection<SerialPortParamer<int>>();
            DataBit.Add(new SerialPortParamer<int>(5));
            DataBit.Add(new SerialPortParamer<int>(6));
            DataBit.Add(new SerialPortParamer<int>(7));
            DataBit.Add(new SerialPortParamer<int>(8));

            ParityBit = new ObservableCollection<SerialPortParamer<Parity>>();
            ParityBit.Add(new SerialPortParamer<Parity>(Parity.Even));
            ParityBit.Add(new SerialPortParamer<Parity>(Parity.Odd));
            ParityBit.Add(new SerialPortParamer<Parity>(Parity.Mark));
            ParityBit.Add(new SerialPortParamer<Parity>(Parity.Space));
            ParityBit.Add(new SerialPortParamer<Parity>(Parity.None));

            StopBit = new ObservableCollection<SerialPortParamer<StopBits>>();
            StopBit.Add(new SerialPortParamer<StopBits>(StopBits.One));
            StopBit.Add(new SerialPortParamer<StopBits>(StopBits.OnePointFive));
            StopBit.Add(new SerialPortParamer<StopBits>(StopBits.Two));


            CommonPort = new ObservableCollection<SerialPortParamer<string>>();
            foreach (string s in SerialPort.GetPortNames())
            {
                CommonPort.Add(new SerialPortParamer<string>(s));
            }
        }
    }
}
