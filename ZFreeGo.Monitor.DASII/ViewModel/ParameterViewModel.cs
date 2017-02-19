using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using ZFreeGo.Monitor.DASModel.GetData;
using ZFreeGo.Monitor.DASModel.Table;

namespace ZFreeGo.Monitor.DASII.ViewModel
{
   
    public class ParameterViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the DataGridPageViewModel class.
        /// </summary>
        public ParameterViewModel()
        {
            _userData = new ObservableCollection<SystemParameter>();
            LoadDataCommand = new RelayCommand(ExecuteLoadDataCommand);
        }

        /************** 属性 **************/
        private ObservableCollection<SystemParameter> _userData;
        /// <summary>
        /// 用户信息数据
        /// </summary>
        public ObservableCollection<SystemParameter> UserData
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
            var get = new GetViewData();
            UserData = get.GetSystemParameterList();
        }
        #endregion
    }
}