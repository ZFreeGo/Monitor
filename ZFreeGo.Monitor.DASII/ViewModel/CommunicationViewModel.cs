using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    }
}