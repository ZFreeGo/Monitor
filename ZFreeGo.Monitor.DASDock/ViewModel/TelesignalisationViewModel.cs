using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using ZFreeGo.Monitor.DASModel.GetViewData;
using ZFreeGo.Monitor.DASModel.DataItemSet;

namespace ZFreeGo.Monitor.DASDock.ViewModel
{
   
    public class TelesignalisationViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the DataGridPageViewModel class.
        /// </summary>
        public TelesignalisationViewModel()
        {
            _userData = new ObservableCollection<Telesignalisation>();
            LoadDataCommand = new RelayCommand(ExecuteLoadDataCommand);
            Messenger.Default.Register<MonitorViewData>(this, "LoadData", ExecuteLoadData);
        }

        private void ExecuteLoadData(MonitorViewData obj)
        {
            if (obj != null)
            {
                UserData = obj.GetTelesignalisationList();
            }
        }

        /************** 属性 **************/
        private ObservableCollection<Telesignalisation> _userData;
        /// <summary>
        /// 用户信息数据
        /// </summary>
        public ObservableCollection<Telesignalisation> UserData
        {
            get { return _userData; }
            set
            {
                _userData = value;
                RaisePropertyChanged("UserData");
            }
        }
        #region 加载数据命令：LoadDataCommand
        /// <summary>
        /// 加载数据
        /// </summary>
        public RelayCommand LoadDataCommand { get; private set; }

        //加载用户数据
        void ExecuteLoadDataCommand()
        {
            //var get = new GetViewData();
            //UserData = get.GetTelesignalisationList();
        }
        #endregion
    }
}