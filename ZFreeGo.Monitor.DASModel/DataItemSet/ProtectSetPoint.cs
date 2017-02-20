using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace ZFreeGo.Monitor.DASModel.DataItemSet
{
    /// <summary>
    /// 这个是保护设定值参数类
    /// </summary>
    public class ProtectSetPoint : ObservableObject
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
                 RaisePropertyChanged("InternalID");
            }
        }

        private string type;
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                 RaisePropertyChanged("Type");
            }
        }


        private int endPoint;
        public int EndPoint
        {
            get { return endPoint; }
            set
            {
                endPoint = value;
                 RaisePropertyChanged("EndPoint");
            }
        }

        private string protectSetName;
        public string ProtectSetName
        {
            get { return protectSetName; }
            set
            {
                protectSetName = value;
                 RaisePropertyChanged("ProtectSetName");
            }
        }
        private double parameterValue;
        public double ParameterValue
        {
            get { return parameterValue; }
            set
            {
                parameterValue = value;
                 RaisePropertyChanged("ParameterValue");
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

         private string range;
        public string Range
        {
            get { return range; }
            set
            {
                range = value;
                 RaisePropertyChanged("Range");
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

     
    }
}
