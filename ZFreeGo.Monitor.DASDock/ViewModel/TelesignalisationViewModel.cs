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
            
            Messenger.Default.Register<MonitorViewData>(this, "LoadData", ExecuteLoadData);
            DataGridMenumSelected = new RelayCommand<string>(ExecuteDataGridMenumSelected);            

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
        

        #region DataGridMenumSelected

        private int selectedIndex = 0;
        /// <summary>
        /// 选择索引
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }
        private bool fixCheck = false;
        /// <summary>
        /// 检测使能
        /// </summary>
        public bool  FixCheck
        {
            get
            {
                return fixCheck;
            }
            set
            {
                fixCheck = value;
                RaisePropertyChanged("FixCheck");
                RaisePropertyChanged("MenumEnable");
                RaisePropertyChanged("ReadOnly");
            }
        }
        
        /// <summary>
        /// 检测使能
        /// </summary>
        public bool MenumEnable
        {
            get
            {
                return fixCheck;
            }
         
        }
         /// <summary>
        /// 检测使能
        /// </summary>
        public bool  ReadOnly
        {
            get
            {
                return !fixCheck;
            }
        
        }

        public RelayCommand<string> DataGridMenumSelected { get; private set; }

        private void ExecuteDataGridMenumSelected(string name)
        {
            switch (name)
            {
                case "Reload":
                    {
                        string path = "";
            //            OpenXmlFile(ref path, "xml");

            //            telesignalisation = DataLoad<Telesignalisation>(ref path, ref pathxsdTelesignalisation,
            //ref dataSetTelesignalisation, DataTypeEnum.Telesignalisation, gridTelesignalisation);
                        break;
                    }
                case "Save":
                    {
         //               string path = "";
         //               SaveXmlFile(ref path);
         //               DataExport<Telesignalisation>(dataSetTelesignalisation, DataTypeEnum.Telesignalisation,
         //telesignalisation, path);

                        break;
                    }
                case "AddUp":
                    {
                        if (SelectedIndex > -1)
                        {
                            var item = new Telesignalisation(0, "xxx", 0, "否", 0, "xxx", "xxx", "StateA", "StateB");                          
                            UserData.Insert(SelectedIndex, item);
                        }
                        break;
                    }
                case "AddDown":
                    {

                        if (SelectedIndex > -1)
                        {
                            var item = new Telesignalisation(0, "xxx", 0, "否", 0, "xxx", "xxx", "StateA", "StateB");                           
                            if (SelectedIndex < UserData.Count - 1)
                            {

                                UserData.Insert(SelectedIndex + 1, item);
                            }
                            else
                            {
                                UserData.Add(item);
                            }
                        }
                        break;
                    }
                case "DeleteSelect":
                    {
                        if (SelectedIndex > -1)
                        {
                            //var result = MessageBox.Show("是否删除选中行:" + gridTelesignalisation.SelectedItem.ToString(),
                            //    "确认删除", MessageBoxButton.OKCancel);
                            var result = true;
                            if (result)
                            {
                                UserData.RemoveAt(SelectedIndex);
                            }
                        }
                        break;
                    }
            }
        }
        #endregion
    }
}