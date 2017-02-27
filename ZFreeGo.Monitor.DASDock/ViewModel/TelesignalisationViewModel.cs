using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using ZFreeGo.Monitor.DASModel.GetViewData;
using ZFreeGo.Monitor.DASModel.DataItemSet;
using ZFreeGo.Monitor.DASModel;
using System;

namespace ZFreeGo.Monitor.DASDock.ViewModel
{
   
    public class TelesignalisationViewModel : ViewModelBase
    {
        private MonitorViewData monitorData;
        /// <summary>
        /// Initializes a new instance of the DataGridPageViewModel class.
        /// </summary>
        public TelesignalisationViewModel()
        {
            userData = new ObservableCollection<Telesignalisation>();

            Messenger.Default.Register<DASModelServer>(this, "DASModelServer", ExecuteDASModelServer);
            DataGridMenumSelected = new RelayCommand<string>(ExecuteDataGridMenumSelected);            

        }

        private void ExecuteDASModelServer(DASModelServer obj)
        {
            try
            {
                if (obj != null)
                {
                    //UserData = obj.DataFile.MonitorData.GetTelesignalisationList();
                    monitorData = obj.DataFile.MonitorData;
                   UserData = monitorData.ReadTelesignalisation(true);
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");
            }
        }

       

     

        /************** 属性 **************/
        private ObservableCollection<Telesignalisation> userData;
        /// <summary>
        /// 用户信息数据
        /// </summary>
        public ObservableCollection<Telesignalisation> UserData
        {
            get { return userData; }
            set
            {
              userData = value;
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
            try
            {
                switch (name)
                {
                    case "Reload":
                        {

                            UserData = monitorData.ReadTelesignalisation(true);
                            break;
                        }
                    case "Save":
                        {

                            monitorData.InsertTelesignalisation();
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
            catch(Exception ex)
            {
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage"); 
            }
        }
        #endregion
    }
}