using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.DASModel.Table
{
    /// <summary>
    /// 这是个系统校准参数类
    /// </summary>
    public class SystemCalibration : INotifyPropertyChanged
    {
        /// <summary>
        /// 校准参数地址
        /// </summary>
        public const UInt32 BasicAddress = 0x4001;
       

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

        private string paramName;
        public string ParamName
        {
            get { return paramName; }
            set
            {
                paramName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParamName"));
            }
        }

        
        private double paramValue;
        public double ParamValue
        {
            get { return paramValue; }
            set
            {
                paramValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParamValue"));
            }
        }

       private double standardValue;
        public double StandardValue
        {
            get { return standardValue; }
            set
            {
                standardValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StandardValue"));
            }
        }


        private double callCoefficient;
        public double CallCoefficient
        {
            get { return callCoefficient; }
            set
            {
                callCoefficient = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CallCoefficient"));
            }
        }

         private double downloadCoefficient;
        public double DownloadCoefficient
        {
            get { return downloadCoefficient; }
            set
            {
                downloadCoefficient = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DownloadCoefficient"));
            }
        }

         private double averageValue;
        public double AverageValue
        {
            get { return averageValue; }
            set
            {
                averageValue = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AverageValue"));
            }
        }
       
       
         public double Data1
         {
             get { return data[0]; }
            set
            {
                data[0] = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data1"));
                CalculateDownloadCoefficient();
            }
         }
       

         public double Data2
         {
             get { return data[1]; }
            set
            {
                data[1] = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data2"));
                CalculateDownloadCoefficient();
            }
         }
        
         public double Data3
         {
             get { return data[2]; }
            set
            {
                data[2] = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data3"));
                CalculateDownloadCoefficient();
            }
         }

         public double Data4
         {
            get { return data[3]; }
            set
            {
                data[3] = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data4"));
                CalculateDownloadCoefficient();
            }
         }


         public double Data5
         {
             get { return data[4]; }
            set
            {
                data[4] = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data5"));
                CalculateDownloadCoefficient();
            }
         }


         public double Data6
         {
             get { return data[5]; }
            set
            {
                data[5] = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data6"));
                CalculateDownloadCoefficient();
            }
         }

       
         public double Data7
         {
             get { return data[6]; }
            set
            {
                data[6] = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data7"));
                CalculateDownloadCoefficient();
            }
         }

        
         public double Data8
         {
             get { return data[7]; }
            set
            {
                data[7] = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data8"));
                CalculateDownloadCoefficient();
            }
         }

   
         public double Data9
         {
             get { return data[8]; }
            set
            {
                data[8] = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data9"));
                CalculateDownloadCoefficient();
            }
         }

      
         public double Data10
         {
             get { return data[9]; }
            set
            {
                data[9] = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data10"));
                CalculateDownloadCoefficient();
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
        /// <summary>
        /// 实时数组
        /// </summary>
        private double[] data;

        /// <summary>
        /// 数据数组
        /// </summary>
        public double[] DataArray
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
        /// <summary>
        /// 上限,默认1.1
        /// </summary>
        public double UpLimit;
        /// <summary>
        /// 下限,默认0.9
        /// </summary>
        public double DownLimit;

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="i">索引0-9对应数据1-10</param>
        /// <param name="dt">数据</param>
        public void UpdateData(int i, double dt)
        {
            switch (i)
            {
                case 0:
                    {
                        Data1 = dt;
                        break;
                    }
                case 1:
                    {
                        Data2 = dt;
                        break;
                    }
                case 2:
                    {
                        Data3 = dt;
                        break;
                    }
                case 3:
                    {
                        Data4 = dt;
                        break;
                    }
                case 4:
                    {
                        Data5 = dt;
                        break;
                    }
                case 5:
                    {
                        Data6 = dt;
                        break;
                    }
                case 6:
                    {
                        Data7 = dt;
                        break;
                    }
                case 7:
                    {
                        Data8 = dt;
                        break;
                    }
                case 8:
                    {
                        Data9 = dt;
                        break;
                    }
                case 9:
                    {
                        Data10 = dt;
                        break;
                    }
            }
        }
        /// <summary>
        /// 校准参数初始化
        /// </summary>
        /// <param name="internalID">内部ID</param>
        /// <param name="endPoint">终端端点</param>
        /// <param name="paramName">参数</param>
        /// <param name="paramValue">参数值</param>
        /// <param name="standardValue">标准值</param>
        /// <param name="callCoefficient">召唤系数</param>
        /// <param name="downloadCoefficient">下载系数</param>
        /// <param name="averageValue">平均值</param>
        /// <param name="data1">数据1</param>
        /// <param name="data2">数据2</param>
        /// <param name="data3">数据3</param>
        /// <param name="data4">数据4</param>
        /// <param name="data5">数据5</param>
        /// <param name="data6">数据6</param>
        /// <param name="data7">数据7</param>
        /// <param name="data8">数据8</param>
        /// <param name="data9">数据9</param>
        /// <param name="data10">数据10</param>
        /// <param name="comment">注释</param>
        public SystemCalibration(int internalID, int endPoint, string paramName, double paramValue,
            double standardValue, double callCoefficient, double downloadCoefficient, double averageValue,
            double data1, double data2, double data3, double data4, double data5,
            double data6, double data7, double data8, double data9, double data10,
            string comment)
        {
            data = new double[10];

            InternalID = internalID;
            EndPoint = endPoint;
            ParamName = paramName;
            ParamValue = paramValue;
            StandardValue = standardValue;
            StandardValue = standardValue;
            DownloadCoefficient = downloadCoefficient;
            AverageValue = averageValue;
            Data1 = data1;
            Data2 = data2;
            Data3 = data3;
            Data4 = data4;
            Data5 = data5;
            Data6 = data6;
            Data7 = data7;
            Data8 = data8;
            Data9 = data9;
            Data10 = data10;
            Comment = comment;

            isChanged = false;

            EndPoint = (int)(internalID + SystemCalibration.BasicAddress  - 1);

            UpLimit = 1.1;
            DownLimit = 0.9;

            CallCoefficient = callCoefficient;
        }

        public override string ToString()
        {
            return paramName + " (" + endPoint + ")";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            isChanged = true;
            if (PropertyChanged != null)
                PropertyChanged(this, e);

           
        }


        /// <summary>
        /// 计算平均值与校准系数
        /// </summary>
        private void CalculateDownloadCoefficient()
        {
            double sum = 0;
            int count = 0;
            var upValue = UpLimit*StandardValue;
            var downValue = DownLimit*StandardValue;
            string str = "";
            int index = 0;
            foreach (var m in data)
            {
                if ((m >= downValue) && (m <= upValue))
                {
                    index++;
                    sum += m;
                    count++;
                    str += index.ToString() + ", ";
                }
            }
            if (count != 0)
            {
                AverageValue = sum / count;
                Comment = str;

               // DownloadCoefficient = standardValue * CallCoefficient / AverageValue;
            }

        }
    }
}
