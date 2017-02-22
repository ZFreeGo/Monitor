
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows;
using ZFreeGo.Monitor.AutoStudio.Log;
using ZFreeGo.Net;
using ZFreeGo.Net.Element;
using ZFreeGo.Net.UPNP;



namespace ZFreeGo.Monitor.AutoStudio
{

    public partial class MainWindow : Window
    {
        private IPAddress ip;
        private UpnpServer upnpServer;
        private const int reLinkMs = 2000;

        
        /// <summary>
        /// 网络连接成功标志 true--连接成功 false--连接失败 
        /// </summary>
        private bool isTcpRun = false;
        /// <summary>
        /// 当前Tcp连接模式
        /// </summary>
        private LinkNetMode linkMode;

        /// <summary>
        /// Tcp网络初始化
        /// </summary>
        void NetInit()
        {
            checkIsSub.IsChecked = false; //
            checkIsSub.IsEnabled = true;
            UpdateInputStateInit();

            radioClient.IsChecked = true; //默认作为服务器端

           // string strHostName = Dns.GetHostName(); //得到本机的主机名
          //  var ipAddr= Dns.GetHostAddresses(strHostName);
           // txtIp.Text = ipAddr[1].ToString();
            
        }
        /// <summary>
        /// 更新Tcp操作按钮状态，置为初始状态
        /// </summary>
        private void UpdateInputStateInit()
        {
            btnStartServer.IsEnabled = true;
            btnStopServer.IsEnabled = false;
            btnSend.IsEnabled = false;
        }
        /// <summary>
        /// 更新Tcp操作按钮状态,置为已经联网状态
        /// </summary>
        private void UpdateInputStateLink()
        {
            btnStartServer.IsEnabled = false;
            btnStopServer.IsEnabled = true;
            btnSend.IsEnabled = true;
        }
        /// <summary>
        /// 读取IP地址
        /// </summary>
        /// <returns>转换后的IP数据</returns>
        private IPAddress getIP()
        {
            var ip1 = txtIp.Text;
            return IPAddress.Parse(ip1);
        }
        /// <summary>
        /// 客户端接收配置
        /// </summary>
        private void ClientReciveConfig()
        {
                       
        }
        /// <summary>
        /// UNPN初始化
        /// </summary>
        /// <param name="state">true 为路由绑定， false 为 普通模式</param>
        /// <returns>成功为true</returns>
        bool NetUPNPInit(LinkNetMode state)
        {

            try
            {
                if (upnpServer !=null)
                {
                    throw new Exception("upnpServer 不为空，不能建立重复连接,必须首先断开");
                }
                //txtLinkMsg.Text = "";
                
                ip = getIP();
                int port = int.Parse(txtPort.Text);

                if (state == LinkNetMode.UPNPServer)
                {
                    txtLinkMsg.Text += "UPnp Server is running ... \n";
                    txtLinkMsg.ScrollToEnd();
                    upnpServer = new UpnpServer(port, port);
                    //作为服务器端，产生数据
                   // serialControlCenter.RtuFrameArrived += tcpTransmission_RtuFrameArrived;
                }
                else if (state == LinkNetMode.TcpServer)
                {
                    txtLinkMsg.Text += "TcpServer is running ... \n";
                    txtLinkMsg.ScrollToEnd();
                    upnpServer = new UpnpServer(ip, port);
                    //作为服务器端，产生数据
                   // serialControlCenter.RtuFrameArrived += tcpTransmission_RtuFrameArrived;   
                }
                else if (state == LinkNetMode.TcpClient)
                {
                    txtLinkMsg.Text += "TcpClient is running ... \n";
                    txtLinkMsg.ScrollToEnd();
                    upnpServer = new UpnpServer(ip, port, false);
                          
                }
                upnpServer.NetDataArrayArrived += NetData_Arrived;

                upnpServer.ExceptionArrived += Exception_TestArrived;
                upnpServer.LinkingMsg += LinkMsg_TestArrived;
                
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "UPNP net初始化");
                return false;
            }
        }
        /// <summary>
        /// NetUNP服务终止
        /// </summary>
        void NetUNPStop()
        {
            try
            {
                if (upnpServer != null)
                {
                    upnpServer.Stop();
                    upnpServer.NetDataArrayArrived -= NetData_Arrived;

                    upnpServer.ExceptionArrived -= Exception_TestArrived;
                    upnpServer.LinkingMsg -= LinkMsg_TestArrived;
                    upnpServer = null;
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "NetUNPStop");

            }
            
        }
        /// <summary>
        /// 根据输入状态选择相应的连接模式
        /// </summary>
        private void UPNPStartServer()
        {
            if (isTcpRun == false)
            {
                bool state = false;
                if (radioClient.IsChecked == true)
                {
                    linkMode = LinkNetMode.TcpClient;

                }
                else if (true == checkIsSub.IsChecked) //使用UNPN绑定
                {
                    linkMode = LinkNetMode.UPNPServer;
                }
                else
                {
                    linkMode = LinkNetMode.TcpServer;

                }
                state = NetUPNPInit(linkMode);
                if (state)
                {
                    checkIsSub.IsEnabled = false;
                    UpdateInputStateLink();
                }
            }


          

        }
        /// <summary>
        /// 停止当前网络连接
        /// </summary>
        private void UPNPStopServer()
        {

            isTcpRun = false; //结束连接标识
            if (true == checkIsSub.IsChecked)
            {
                NetUNPStop();
            }
            else
            {
                NetUNPStop();
            }
            checkIsSub.IsEnabled = true;
            UpdateInputStateInit();
        }
        /// <summary>
        /// 根据选择使用不同的协议类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartServer_Click(object sender, RoutedEventArgs e)
        {
           
                UPNPStartServer();           

        }
       /// <summary>
       /// 停止Tcp连接调用,与此同时复位104规约服务活动
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void btnStopServer_Click(object sender, RoutedEventArgs e)
        {
            MakeLogMessage(sender, "停止网络服务", LogType.Net);
            UPNPStopServer();

            

        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (isTcpRun)
            {
                upnpServer.SendMessage(txtSendMsg.Text);
            }
            else
            {
                MessageBox.Show("upnpServer未启动，发送数据无效");
            }

        }
       


        /// <summary>
        /// 服务数据到达
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NetData_Arrived(object sender, NetDataArrayEventArgs e)
        {
            lock (protocolServer.CheckGetMessage.ReciveQuene)
            {
                foreach (var m in e.DataArray)
                {
                    protocolServer.CheckGetMessage.ReciveQuene.Enqueue(m);

                }
                Console.WriteLine("字节数据接收");
            }

            string msg = Encoding.Unicode.GetString(e.DataArray, 0, e.DataArray.Length);
            string str = "";
            str += "HEX:";
            foreach (var m in e.DataArray)
            {
                str += m.ToString("X2") + " ";
            }
            msg += "\n" + str;
            Action<string> myDelegate = UpdateReciveTxt;
            Dispatcher.BeginInvoke(myDelegate, msg);

       
        }
        /// <summary>
        /// TCP连接出现异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exception_TestArrived(object sender, NetDataArrivedEventArgs e)
        {
            
            Action<string> myDelegate = UpdateLinkTxt;
            Dispatcher.BeginInvoke(myDelegate, e.DataMsg);

            Action bk = () =>
            {
                btnStopServer_Click(null, null);
                if (true == checkIsReStart.IsChecked)
                {
                    txtLinkMsg.Text += "服务器端将在" + reLinkMs.ToString() + "ms后重新连接;\n";
                    txtLinkMsg.ScrollToEnd();
                }
            };

            Dispatcher.BeginInvoke(bk);
            Thread.Sleep(reLinkMs);
            Action<int> restart = ReStartServer;
            Dispatcher.BeginInvoke(restart, reLinkMs);

        }


        /// <summary>
        /// 重启服务器
        /// </summary>
        /// <param name="ms">启动延时时间(ms)</param>
        private void ReStartServer(int ms)
        {
            if (true == checkIsReStart.IsChecked)
            {

                btnStartServer_Click(null, null);
            }

        }
        /// <summary>
        /// 网络连接信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkMsg_TestArrived(object sender, NetDataArrivedEventArgs e)
        {
            Action<string> myDelegate = UpdateLinkTxt;
           
            Dispatcher.BeginInvoke(myDelegate, e.DataMsg);

            isTcpRun = true; //连接成功标识
            //作为客户端需要启动接收
            if (linkMode == LinkNetMode.TcpClient)
            {
                ClientReciveConfig();

            }
           

           

        }
        /// <summary>
        /// 更新接收文本信息
        /// </summary>
        /// <param name="str"></param>
        private void UpdateReciveTxt(string str)
        {
            txtReciveMsg.Text += str + "\n";
        }
        /// <summary>
        /// 更新链路信息
        /// </summary>
        /// <param name="str"></param>
        private void UpdateLinkTxt(string str)
        {
            txtLinkMsg.Text += str + "\n";
            txtLinkMsg.ScrollToEnd();
            MakeLogMessage(this,"连接信息:" + str, LogType.Net);
        }



        /// <summary>
        /// 清除状态信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearState_Click(object sender, RoutedEventArgs e)
        {
            txtLinkMsg.Text = "";
            txtSendMsg.Text = "";
            txtReciveMsg.Text = "";
        }
        /// <summary>
        /// ping地址
        /// </summary>
        /// <param name="ip">要ping的Ip地址</param>
        /// <param name="timeout">超时时间 ms</param>
        /// <returns></returns>
        private bool PingAddress(string ip, int timeout)
        {
            try
            {
                Ping pingSender = new Ping();
                PingReply reply = pingSender.Send(ip, timeout);//第一个参数为ip地址，第二个参数为ping的时间 
                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (PingException )
            {
                
             //   MessageBox.Show(ex.Message, "PingAddress");
                return false;
            }
        }


        private bool NetSendData(byte[] data)
        {
            try
            {
                if (isTcpRun)
                {
                    upnpServer.SendMessage(data);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "NetSendData");
                return false;
            }
           
        }

      



    }
}
