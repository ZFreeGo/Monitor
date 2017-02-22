using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.DASModel.DataItemSet
{
    /// <summary>
    /// 遥控参数类
    /// </summary>
    public class Telecontrol : ObservableObject
    {

        /// <summary>
        /// 遥控对象基址
        /// </summary>
        public const UInt32 BasicAddress = 0x6001;

        private int telecontrolID;
        public int TelecontrolID
        {
            get { return telecontrolID; }
            set
            {
                telecontrolID = value;
                RaisePropertyChanged("TelecontrolID");
            }
        }

        private string telecontrolComment;
        public string TelecontrolComment
        {
            get { return telecontrolComment; }
            set
            {
        
                telecontrolComment = value;
                RaisePropertyChanged("TelecontrolComment");
            }
        }




        private string telecontrolState;
        public string TelecontrolState
        {
            get { return telecontrolState; }
            set
            {
                telecontrolState = value;
                RaisePropertyChanged("TelecontrolState");
            }
        }


        private string telecontrolOperate;
        public string TelecontrolOperate
        {
            get { return telecontrolOperate; }
            set
            {
                telecontrolOperate = value;
                RaisePropertyChanged("TelecontrolOperate");
            }
        } 
  

        private string telecontrolOperateState;
        public string TelecontrolOperateState
        {
            get { return telecontrolOperateState; }
            set
            {
                telecontrolOperateState = value;
                RaisePropertyChanged("TelecontrolOperateState");
            }
        } 

        private string deviceActionTime;
        public string DeviceActionTime
        {
            get { return deviceActionTime; }
            set
            {
                deviceActionTime = value;
                RaisePropertyChanged("DeviceActionTime");
            }
        } 

        private bool isChanged;
        /// <summary>
        /// 属性值是否被改变，true-初始化后被改变，false-没有
        /// </summary>
        public bool IsChanged
        {
            get { return isChanged; }
        }




        public Telecontrol(int telecontrolID, string telecontrolComment, string telecontrolState, string telecontrolOperate,
            string telecontrolOperateState, string deviceActionTime)
        {
            TelecontrolID = telecontrolID;
            TelecontrolComment = telecontrolComment;
            TelecontrolState =  telecontrolState;
            TelecontrolOperate = telecontrolOperate;
            TelecontrolOperateState = telecontrolOperateState;
            DeviceActionTime = deviceActionTime;

            isChanged = false; //首次初始化为false 
        }



        public override string ToString()
        {
            return TelecontrolComment + " (" + TelecontrolID + ")";
        }

  

    }
}
