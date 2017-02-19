using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace ZFreeGo.Monitor.DASModel.Table
{
    /// <summary>
    /// 事件日志参数
    /// </summary>
    public class EventLog : ObservableObject
    {

        private int eventID;
        public int EventID
        {
            get { return eventID; }
            set
            {
                eventID = value;
                RaisePropertyChanged("EventID");
            }
        }

        private string faultStyle;
        public string FaultStyle
        {
            get { return faultStyle; }
            set
            {
                faultStyle = value;
                RaisePropertyChanged("FaultStyle");
            }
        }


       

        private string eventConent;
        /// <summary>
        /// 是否取反
        /// </summary>
        public string EventConent
        {
            get { return eventConent; }
            set
            {
                eventConent = value;
                RaisePropertyChanged("EventConent");
            }
        }


        private string date;
        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                RaisePropertyChanged("Date");
            }
        }

        private string unit;
        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                RaisePropertyChanged("Unit");
            }
        }

        private string millisecond;
        public string Millisecond
        {
            get { return millisecond; }
            set
            {
                millisecond = value;
                RaisePropertyChanged("Millisecond");
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



        /// <summary>
        /// 事件记录初始化
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="faultStyle">故障类型</param>
        /// <param name="eventConent">事件内容</param>
        /// <param name="date">日期</param>
        /// <param name="unit">单位</param>
        /// <param name="millisecond">毫秒数</param>
        public EventLog(int eventID, string faultStyle, string eventConent, string date, string unit, string millisecond)
        {
            EventID = eventID;
            FaultStyle = faultStyle;
            EventConent = eventConent;
            Date = date;
            Unit = unit;
            Millisecond = millisecond;

            isChanged = false; //首次初始化为false 
        }



        public override string ToString()
        {
            return faultStyle + " (" + eventConent + ")";
        }


        
    }
}
