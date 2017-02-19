using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.DASModel.Table
{
    /// <summary>
    /// 这是个遥测参数类
    /// </summary>
    public class Telemetering : ObservableObject
    {
        /// <summary>
        /// 遥测对象公共地址
        /// </summary>
        public const UInt32 BasicAddress = 0x4001;

        private int internalID;
        public int InternalID
        {
            get { return internalID; }
            set
            {
                internalID = value;
                 RaisePropertyChanged("InternalID");
            }
        }

        private string telemeteringName;
        public string TelemeteringName
        {
            get { return telemeteringName; }
            set
            {
                telemeteringName = value;
                 RaisePropertyChanged("TelemeteringName");
            }
        }

        private int telemeteringID;
        public int TelemeteringID
        {
            get { return telemeteringID; }
            set
            {
                telemeteringID = value;
                 RaisePropertyChanged("TelemeteringID");
            }
        }
        private double calibrationCoefficient;
        public double CalibrationCoefficient
        {
            get { return calibrationCoefficient; }
            set
            {
                calibrationCoefficient = value;
                 RaisePropertyChanged("CalibrationCoefficient");
            }
        }
        private double telemeteringValue;
        public double TelemeteringValue
        {
            get { return telemeteringValue; }
            set
            {
                telemeteringValue = value;
                 RaisePropertyChanged("TelemeteringValue");
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
        private string mark;
        public string Mark
        {
            get { return mark; }
            set
            {
                mark = value;
                 RaisePropertyChanged("Mark");
            }
        }
        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                 RaisePropertyChanged("Comment");
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
        /// 遥测数据初始化
        /// </summary>
        /// <param name="internalID">内部ID</param>
        /// <param name="telemeteringName">遥测名称</param>
        /// <param name="telemeteringID">遥测ID</param>
        /// <param name="calibrationCoefficient">校准系数</param>
        /// <param name="telemeteringValue">遥测值</param>
        /// <param name="unit">单位</param>
        /// <param name="mark">符号位</param>
        /// <param name="comment">注释</param>
        public Telemetering(int internalID, string telemeteringName, int telemeteringID, double calibrationCoefficient,
            double telemeteringValue, string unit, string mark, string comment)
        {
            this.internalID = internalID;
            this.telemeteringName = telemeteringName;
            this.telemeteringID = telemeteringID;
            this.calibrationCoefficient = calibrationCoefficient;
            this.telemeteringValue = telemeteringValue;
            this.unit = unit;
            this.mark = mark;
            this.comment = comment;

            TelemeteringID = (int)(internalID + Telemetering.BasicAddress  - 1);
        }

        public override string ToString()
        {
            return telemeteringName + " (" + telemeteringID + ")";
        }

     
    }
}
