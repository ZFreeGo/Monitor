using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.DASModel.DataItemSet
{
    /// <summary>
    /// 这是个电能脉冲
    /// </summary>
    public class ElectricPulse : ObservableObject
    {
        /// <summary>
        /// 电能脉冲公共地址
        /// </summary>
        public const UInt32 BasicAddress = 0x6401;

        private UInt32 _id;
        /// <summary>
        /// ID
        /// </summary>
        public UInt32 ID
        {
            get { return _id; }
            set
            {
                 _id = value;
                 RaisePropertyChanged("ID");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                 _name = value;
                 RaisePropertyChanged("Name");
            }
        }

        private double  _value;
        public double Value
        {
            get { return _value; }
            set
            {
                 _value = value;
                 RaisePropertyChanged("Value");
            }
        }
        private string _unit;
        public string Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                RaisePropertyChanged("Unit");
            }
        }
        private string _timeStamp;
        public string TimeStamp
        {
            get { return _timeStamp; }
            set
            {
                 _timeStamp = value;
                 RaisePropertyChanged("TimeStamp");
            }
        }
        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set
            {
                 _comment = value;
                 RaisePropertyChanged("Comment");
            }
        }
   

        
        
        /// <summary>
        /// 电能脉冲
        /// </summary>
        /// <param name="id">ID号</param>
        /// <param name="name">名称</param>
        /// <param name="inValue">值</param>
        /// <param name="unit">单位</param>
        /// <param name="timeStamp">时戳</param>
        /// <param name="comment">注释</param>
        public ElectricPulse(UInt32 id, string name, double inValue, string unit,
             string timeStamp,  string comment)
        {
            _id = id;
            _name = name;
            _value = inValue;
            _unit = unit;
            _timeStamp = timeStamp;
            _comment = comment;

        }

        public override string ToString()
        {
            return "EletricPulse";
        }

     
    }
}
