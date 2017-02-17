using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using ZFreeGo.TransmissionProtocols.FileSever;
using ZFreeGo.TransmissionProtocols.BasicElement;

namespace ZFreeGo.Monitor.AutoStudio
{
    public partial class MainWindow
    {

        /// <summary>
        /// 召唤文件目录服务
        /// </summary>
        CallFileDirectoryServer callDirectoryServer;
       
        /// <summary>
        /// 召唤文件目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadDirectory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isTcpRun)
                {
                    MessageBox.Show("Tcp未启动");
                    return;
                }

                UInt32 id = UInt32.Parse(txtDirID.Text);
                string name = txtDirName.Text;
                DateTime start = DateTime.Now;
                DateTime end = DateTime.Now;
                FileCallFlag call = FileCallFlag.All;
                if(checkMeetTime.IsChecked == true)
                {
                    start = DateTime.Parse(txtStartTime.Text);
                    end = DateTime.Parse(txtEndTime.Text);
                    call = FileCallFlag.MeetTime;
                }
                var packet = new FileDirectoryCalledPacket(id, name, call, new CP56Time2a(start), new CP56Time2a(end));    
                callDirectoryServer = new CallFileDirectoryServer();
                callDirectoryServer.CallFileDirectoryEvent += callDirectoryServer_CallFileDirectoryEvent;
                callDirectoryServer.CallFileEndEvent +=callDirectoryServer_CallFileEndEvent;
               // callDirectoryServer.StartServer(SendFileServerMessage, packet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "btnReadDirectory_Click");
            }
        }

        
        /// <summary>
        /// 文件目录召唤结束事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void callDirectoryServer_CallFileEndEvent(object sender, CallFileEndEventArgs e)
        {
            try
            {
                if (e.AttributeList != null)
                {
                    var collect = new ObservableCollection<FileAttribute>(e.AttributeList);
                    gridFileDir.DataContext = collect;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "callDirectoryServer_CallFileEndEvent");
            }
        }
        /// <summary>
        /// 召唤文件目录服务事件响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void callDirectoryServer_CallFileDirectoryEvent(object sender, FileServerEventArgs<FileDirectoryCalledAckPacket> e)
        {
           try
           {
               MessageBox.Show(e.Message);
               if (e.Operation == OperatSign.ReadDirectoryACK)
               {
                   if (e.Packet != null)
                   {
                       var data = e.Packet.GetAPDUDataArray();
                       BeginInvokeUpdateHistory(data, data.Length, "召唤文件目录响应");
                      
                   }
               }

           }
            catch(Exception ex)
           {
               MessageBox.Show(ex.Message, "召唤文件目录服务事件响应");
           }
        }
    }
}
