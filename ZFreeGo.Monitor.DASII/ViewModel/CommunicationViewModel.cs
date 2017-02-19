using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using ZFreeGo.Monitor.DASModel.GetData;
using ZFreeGo.Monitor.DASModel.Table;

namespace ZFreeGo.Monitor.DASII.ViewModel
{
   
    public class CommunicationViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the DataGridPageViewModel class.
        /// </summary>
        public CommunicationViewModel()
        {
           
            LoadDataCommand = new RelayCommand(ExecuteLoadDataCommand);
            StartTcpLink = new RelayCommand<string>(ExecuteStartTcpLink);
        }

       
        #region 加载数据命令：LoadDataCommand
        /// <summary>
        /// 加载数据
        /// </summary>
        public RelayCommand LoadDataCommand { get; private set; }

        //加载用户数据
        void ExecuteLoadDataCommand()
        {
            var get = new GetViewData();
            
        }
        #endregion

     
        /// <summary>
        /// 启动TCP连接
        /// </summary>
        public RelayCommand<string> StartTcpLink { get; private set; }

        //发送显示UserView的消息
        void ExecuteStartTcpLink(string name)
        {
            Messenger.Default.Send<string>("StartLink", "StartTcpLink");
        }
    }
}