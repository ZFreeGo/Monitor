using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.DASModel.Table
{
    /// <summary>
    /// 这个是保护设定值参数类
    /// </summary>
    public class ProtectSetPoint : INotifyPropertyChanged
    {
        /// <summary>
        ///保护定值基址
        /// </summary>
        public const UInt32 BasicAddress = 0x6201;

        private int internalID;
        public int InternalID
        {
            get { return internalID; }
            set
            {
                internalID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InternalID"));
            }
        }

        private string type;
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Type"));
            }
        }


        private int endPoint;
        public int EndPoint
        {
            get { return endPoint; }
            set
            {
                endPoint = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndPoint"));
            }
        }

        private string protectSetName;
        public string ProtectSetName
        {
            get { return protectSetName; }
            set
            {
                protectSetName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProtectSetName"));
            }
        }
        private double parameterValue;
        public double ParameterValue
        {
            get { return parameterValue; }
            set
            {
                parameterValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParameterValue"));
            }
        }
        
        private double calibrationCoefficient;
        public double CalibrationCoefficient
        {
            get { return calibrationCoefficient; }
            set
            {
                calibrationCoefficient = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CalibrationCoefficient"));
            }
        }

        private string unit;
        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Unit"));
            }
        }

         private string range;
        public string Range
        {
            get { return range; }
            set
            {
                range = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Range"));
            }
        }

         private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comment"));
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



        public ProtectSetPoint(int internalID,string type,int endPoint,string protectSetName,
            double parameterValue, double calibrationCoefficient, string unit, string range, string comment)
        {
            InternalID = internalID;
            Type = type;
            EndPoint =  endPoint;
            ProtectSetName =   protectSetName;
            ParameterValue =  parameterValue;
            CalibrationCoefficient = calibrationCoefficient;
            Unit =  unit;
            Range = range;
            Comment =  comment; 

            isChanged  = false;

            EndPoint = (int)(internalID + ProtectSetPoint.BasicAddress - 1);
        }



        public override string ToString()
        {
            return protectSetName + " (" + parameterValue + ")";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            isChanged = true;
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
