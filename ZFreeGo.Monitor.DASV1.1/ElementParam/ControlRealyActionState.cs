using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.ElementParam
{
    /// <summary>
    /// 遥控激活与预制状态
    /// </summary>
    public class ControlRealyActionState : INotifyPropertyChanged
    {
        public ControlRealyActionState()
        {
            ReadyCloseEnabled = true;
            ActionCloseEnabled = false;
            ReadyOpenEnabled = true;
            ActionOpenEnabled = false;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }


       
        /// <summary>
        ///合闸预制状态true/false
        /// </summary>
        private bool readyCloseEnabled;
        public bool ReadyCloseEnabled
        {
            get { return readyCloseEnabled; }
            set
            {
                readyCloseEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReadyCloseEnabled"));
               
            }
        }
        /// <summary>
        ///合闸执行状态 true/false
        /// </summary>
        private bool actionCloseEnabled;
        public bool ActionCloseEnabled
        {
            get { return actionCloseEnabled; }
            set
            {
                actionCloseEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionCloseEnabled"));

            }
        }


        /// <summary>
        ///分闸预制状态true/false
        /// </summary>
        private bool readyOpenEnabled;
        public bool ReadyOpenEnabled
        {
            get { return readyOpenEnabled; }
            set
            {
                readyOpenEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReadyOpenEnabled"));

            }
        }
        /// <summary>
        ///分闸执行状态 true/false
        /// </summary>
        private bool actionOpenEnabled;
        public bool ActionOpenEnabled
        {
            get { return actionOpenEnabled; }
            set
            {
                actionOpenEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ActionOpenEnabled"));

            }
        }

    }
}
