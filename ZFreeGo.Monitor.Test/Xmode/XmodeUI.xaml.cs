using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
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
using ZFreeGo.TransportProtocol.Xmodem;

namespace ZFreeGo.Monitor.Test.Xmode
{
    /// <summary>
    /// XmodeUI.xaml 的交互逻辑
    /// </summary>
    public partial class XmodeUI : Window
    {
         private SerialControlCenter serialControlCenter; //用于串口通讯
        public XmodeUI()
        {
            InitializeComponent();
            serialControlCenter = new SerialControlCenter();
            UpdatePortShow(serialControlCenter.SerialPort);

            serialControlCenter.SerialDataArrived += serialControlCenter_SerialDataArrived;

            //serialControlCenter
        }

        private void serialControlCenter_SerialDataArrived(object sender, SerialDataEvent e)
        {
            try
            {
                Action<string> fun = (ar) => { txtRecive.Text += ar; };
                var stdData = Encoding.ASCII.GetString(e.SerialData);
                Dispatcher.BeginInvoke(fun, stdData);
                xmodeSever.Enqueue(e.SerialData);
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "接收数据");
            }

        }

        //private

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


                    serialControlCenter.CloseCenter();
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

        private void btnClrearRecive_Click(object sender, RoutedEventArgs e)
        {
            txtRecive.Clear();
        }


  

        private void btnClearSendTxt_Click(object sender, RoutedEventArgs e)
        {
            txtSend.Clear();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(serialControlCenter.CommState)
                {
                    serialControlCenter.SendMessage(txtSend.Text);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "串口发送");
            }
        }

        private void radioASCII_Checked(object sender, RoutedEventArgs e)
        {
            if(radioASCII.IsChecked == true)
            {
                
            }
        }

        private void radioBinary_Checked(object sender, RoutedEventArgs e)
        {
            if (radioBinary.IsChecked == true)
            {

            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            serialControlCenter.CloseCenter();
            if (xmodeSever !=null)
            {
                xmodeSever.CloseServer();
            }
        }


        XmodeServer xmodeSever;
        XmodePacketManager xmdoePacketManager;
        byte[] testData;
        /// <summary>
        /// Xmode测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartTest_Click(object sender, RoutedEventArgs e)
        {
            if (serialControlCenter.CommState)
            {

                int len = 5000;
                testData = new byte[len];
                for (int i = 0; i < len; i++)
                {
                    testData[i] = (byte)(i %256);
                }
             
                xmdoePacketManager = new XmodePacketManager(testData, testData.Length, XmodeStartHeader.STX);
                xmodeSever = new XmodeServer();
              
                xmodeSever.ServerEvent += xmodeSever_ServerEvent;
                serialControlCenter.SerialPort.DiscardInBuffer();
                xmodeSever.StartServer(xmdoePacketManager, serialControlCenter.SendMessage);
               
            }
        }

 
        void xmodeSever_ServerEvent(object sender, XmodeServerEventArgs e)
        {
            MessageBox.Show(e.ServerState.ToString());
        }

    }
}
