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
            DataGridMenumSelected = new RelayCommand<string>(ExecuteDataGridMenumSelected);
            ItemSelected = new RelayCommand<string>(ExecuteItemSelected);

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

        #region DataGridMenumSelected
        public RelayCommand<string> ItemSelected { get; private set; }

        private void ExecuteItemSelected(string name)
         {
            
         }

        public RelayCommand<string> DataGridMenumSelected { get; private set; }

        private void ExecuteDataGridMenumSelected(string name)
        {
            switch (name)
            {
                case "itemLoadAs":
                    {
                        string path = "";
            //            OpenXmlFile(ref path, "xml");

            //            telesignalisation = DataLoad<Telesignalisation>(ref path, ref pathxsdTelesignalisation,
            //ref dataSetTelesignalisation, DataTypeEnum.Telesignalisation, gridTelesignalisation);
                        break;
                    }
                case "itemSaveAs":
                    {
         //               string path = "";
         //               SaveXmlFile(ref path);
         //               DataExport<Telesignalisation>(dataSetTelesignalisation, DataTypeEnum.Telesignalisation,
         //telesignalisation, path);

                        break;
                    }
                case "itemAddUp":
                    {
                        //if (gridTelesignalisation.SelectedIndex > -1)
                        //{
                        //    var item = new Telesignalisation(0, "xxx", 0, "否", 0, "xxx", "xxx", "StateA", "StateB");
                        //    var obser = (ObservableCollection<Telesignalisation>)telesignalisation;
                        //    obser.Insert(gridTelesignalisation.SelectedIndex, item);
                        //}


                        break;
                    }
                case "itemAddDown":
                    {

                        //if (gridTelesignalisation.SelectedIndex > -1)
                        //{
                        //    var item = new Telesignalisation(0, "xxx", 0, "否", 0, "xxx", "xxx", "StateA", "StateB");
                        //    var obser = (ObservableCollection<Telesignalisation>)telesignalisation;
                        //    if (gridTelesignalisation.SelectedIndex < gridTelesignalisation.Items.Count - 1)
                        //    {

                        //        obser.Insert(gridTelesignalisation.SelectedIndex + 1, item);
                        //    }
                        //    else
                        //    {
                        //        obser.Add(item);
                        //    }
                        //}
                        break;
                    }
                case "itemDeleteSelect":
                    {
                        //if (gridTelesignalisation.SelectedIndex > -1)
                        //{


                        //    var result = MessageBox.Show("是否删除选中行:" + gridTelesignalisation.SelectedItem.ToString(),
                        //        "确认删除", MessageBoxButton.OKCancel);
                        //    if (result == MessageBoxResult.OK)
                        //    {
                        //        var obser = (ObservableCollection<Telesignalisation>)telesignalisation;
                        //        obser.RemoveAt(gridTelesignalisation.SelectedIndex);
                        //    }
                        //}
                        break;
                    }
            }
        }
        #endregion
    }
}