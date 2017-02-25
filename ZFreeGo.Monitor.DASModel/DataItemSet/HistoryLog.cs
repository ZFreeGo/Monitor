using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.DASModel.DataItemSet
{
    /// <summary>
    /// 历史日志记录
    /// </summary>
    public class HistoryLog: ObservableObject
    {   
       

        private LogTypeNum _type;
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogTypeNum Type
        {
            get { return _type; }
            set
            {
                 _type = value;
                 RaisePropertyChanged("Type");
            }
        }
           private string _timeStamp;
        /// <summary>
        /// 时标
        /// </summary>
        public string TimeStamp
        {
            get { return _timeStamp; }
            set
            {
                 _timeStamp = value;
                 RaisePropertyChanged("TimeStamp");
            }
        }
        private string _conent;
        /// <summary>
        /// 内容
        /// </summary>
        public string Conent
        {
            get { return _conent; }
            set
            {
                 _conent = value;
                 RaisePropertyChanged("Conent");
            }
        }

        private int  _value;
        /// <summary>
        /// 信息值
        /// </summary>
        public int Value
        {
            get { return _value; }
            set
            {
                 _value = value;
                 RaisePropertyChanged("Value");
            }
        }


        /// <summary>
        /// 历史日志初始化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="time">时标</param>
        /// <param name="conent">内容</param>
        /// <param name="value">值</param>
        public HistoryLog(int type, string time, string conent, int value)
        {
            _type = (LogTypeNum)type;
            _timeStamp = time;
            _conent = conent;
            _value = value;

        }

        public override string ToString()
        {
            return "HistoryLog";
        }

     
    }
}
