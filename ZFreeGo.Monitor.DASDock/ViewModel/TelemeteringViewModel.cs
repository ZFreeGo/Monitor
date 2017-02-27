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
   
    public class TelemeteringViewModel : ViewModelBase
    {
        private MonitorViewData monitorData;

        /// <summary>
        /// Initializes a new instance of the DataGridPageViewModel class.
        /// </summary>
        public TelemeteringViewModel()
        {
            userData = new ObservableCollection<Telemetering>();
            LoadDataCommand = new RelayCommand(ExecuteLoadDataCommand);
           
            Messenger.Default.Register<DASModelServer>(this, "DASModelServer", ExecuteDASModelServer);
            DataGridMenumSelected = new RelayCommand<string>(ExecuteDataGridMenumSelected);
        }

        private void ExecuteDASModelServer(DASModelServer obj)
        {
            try
            {
                if (obj != null)
                {
                    UserData = obj.DataFile.MonitorData.ReadTelemetering(true);
                    //UserData = obj.DataFile.MonitorData.GetTelemeteringList();
                    monitorData = obj.DataFile.MonitorData;

                }
            }
            catch(Exception ex)
            {             
                Messenger.Default.Send<Exception>(ex, "ExceptionMessage");            
            }
        }

  
        /************** 属性 **************/
        private ObservableCollection<Telemetering> userData;
        /// <summary>
        /// 用户信息数据
        /// </summary>
        public ObservableCollection<Telemetering> UserData
        {
            get { return userData; }
            set
            {
                userData = value;
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
            //UserData = get.GetTelemeteringList();
        }
        #endregion

        #region 表格操作
        private bool fixCheck = false;
        /// <summary>
        /// 检测使能
        /// </summary>
        public bool FixCheck
        {
            get
            {
                return fixCheck;
            }
            set
            {
                fixCheck = value;
                RaisePropertyChanged("FixCheck");
                RaisePropertyChanged("ReadOnly");

            }
        }

        /// <summary>
        /// 检测使能
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return !fixCheck;
            }

        }


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




        public RelayCommand<string> DataGridMenumSelected { get; private set; }

        private void ExecuteDataGridMenumSelected(string name)
        {
            try
            {
                if (!FixCheck)
                {
                    return;
                }
                switch (name)
                {
                    case "Reload":
                        {

                            UserData = monitorData.ReadTelemetering(true);
                            break;
                        }
                    case "Save":
                        {
                            monitorData.InsertTelemetering();
                            break;
                        }
                    case "AddUp":
                        {
                            if (SelectedIndex > -1)
                            {
                                var item = new Telemetering(0, "", 0, 1, 1, "", "", "");
                                UserData.Insert(SelectedIndex, item);
                            }
                            break;
                        }
                    case "AddDown":
                        {
                            if (SelectedIndex > -1)
                            {
                                var item = new Telemetering(0, "", 0, 1, 1, "", "", "");
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