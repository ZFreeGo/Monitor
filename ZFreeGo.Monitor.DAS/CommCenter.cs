using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ZFreeGo.Monitor.CommCenter;

namespace ZFreeGo.Monitor.AutoStudio
{
    public partial class MainWindow
    {

        private void OpenSerialPort_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!serialControlCenter.CommState)
                {


                    serialControlCenter.SerialPort.PortName = portName.SelectedItem as string;
                    serialControlCenter.SerialPort.BaudRate = (int)baudRate.SelectedItem;
                    serialControlCenter.SerialPort.DataBits = (int)dataBits.SelectedItem;
                    serialControlCenter.SerialPort.Parity = (Parity)portParity.SelectedItem;

                    serialControlCenter.SerialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopBits.SelectedItem.ToString());

                    serialControlCenter.OpenSerialPort();
                    openSerialPort.Content = "关闭串口";

                    commAGrid.IsEnabled = false;

                   // systemState.CommonStateA = "串口正常";

                }
                else
                {


                    serialControlCenter.CloseSerialPort();
                    openSerialPort.Content = "打开串口";
                    commAGrid.IsEnabled = true;
                  //  systemState.CommonStateA = "串口关闭";


                }




            }
            catch (Exception ex)
            {
                MessageBox.Show("开启或关闭端口::" + ex.Message);
                Trace.WriteLine("开启或关闭端口::" + ex.Message);
            }
        }
      
        public void UpdatePortShow(SerialPort serialPort)
        {
            //DSP控制通讯
            UpdatePortName(portName, serialPort.PortName);
            UpdatePortBaudRate(baudRate, serialPort.BaudRate);
            UpdatePortDataBits(dataBits, serialPort.DataBits);
            UpdatePortParity(portParity, serialPort.Parity);
            UpdatePortStopBits(stopBits, serialPort.StopBits);

        }
        
        public void UpdatePortName(ComboBox comb, string defaultPortName)
        {
            foreach (string s in SerialPort.GetPortNames())
            {
        
                comb.Items.Add(s);
            }
            comb.SelectedIndex = 0;
        }

        public void UpdatePortBaudRate(ComboBox comb, int defaultPortBaudRate)
        {
            comb.Items.Add(1200);
            comb.Items.Add(2400);
            comb.Items.Add(4800);
            comb.Items.Add(9600);
            comb.Items.Add(14400);
            comb.Items.Add(28800);
            comb.Items.Add(38400);
            comb.Items.Add(57600);
            comb.Items.Add(115200);

            comb.SelectedItem = defaultPortBaudRate;
        }
        public void UpdatePortDataBits(ComboBox comb, int defaultPortDataBits)
        {
            comb.Items.Add(5);
            comb.Items.Add(6);
            comb.Items.Add(7);
            comb.Items.Add(8);

            comb.SelectedItem = defaultPortDataBits;
            // return int.Parse(dataBits);
        }
        public void UpdatePortParity(ComboBox comb, Parity defaultPortParity)
        {
            comb.Items.Add(Parity.Even);
            comb.Items.Add(Parity.Odd);
            comb.Items.Add(Parity.Mark);
            comb.Items.Add(Parity.Space);
            comb.Items.Add(Parity.None);

            comb.SelectedItem = defaultPortParity;
            // return (Parity)Enum.Parse(typeof(Parity), parity);
        }

        public void UpdatePortStopBits(ComboBox comb, StopBits defaultPortStopBits)
        {
            //  stopBits.Items.Add(StopBits.None);
           comb.Items.Add(StopBits.One);
           comb.Items.Add(StopBits.OnePointFive);
           comb.Items.Add(StopBits.Two);

           comb.SelectedItem = StopBits.One;
            //return (StopBits)Enum.Parse(typeof(StopBits), stopBits);
        }
    }
}
