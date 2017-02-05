using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZFreeGo.FileOperation.Comtrade.ConfigContent
{
    /// <summary>
    /// 时标倍率因子
    /// </summary>
    public class TimeStampMultiply : RowOperation, INotifyPropertyChanged
    {
        /// <summary>
        /// 时标倍率因子,字符[1,32] ,必要
        /// </summary>
        double timeMultiply;

        /// <summary>
        /// 时标倍率因子,字符[1,32] ,必要
        /// </summary>
        public string TimeMultiply
        {
            get
            {
                return  timeMultiply.ToString();
            }
            set
            {
                double n ;
                if (!double.TryParse(value, out n))
                {
                    throw new ArgumentOutOfRangeException("TimeMultiply，不能转化为浮点数");
                }
                if (n > 0)
                {
                 timeMultiply = n;
                 OnPropertyChanged(new PropertyChangedEventArgs("TimeMultiply"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("时标倍率因子，需要是个能转换为浮点数且大于0的字符串");
                }
            }
        }


        /// <summary>
        /// 由行信息获取标称频率字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string RowToString()
        {
            StringBuilder strBuilt = new StringBuilder(16);
            strBuilt.Append(TimeMultiply);
           
            strBuilt.AppendLine();
            return strBuilt.ToString();

        }

        /// <summary>
        ///获取 通道频率
        /// </summary>
        /// <param name="str">行字符串</param>
        /// <returns>false--转换成功 true--失败</returns>
        public override bool StringToRow(string str)
        {
            TimeMultiply = str;


            return true;
        }
        /// <summary>
        /// 初始化通道频率
        /// </summary>
        /// <param name="str">时标倍率字符</param>
        public TimeStampMultiply(string str)
        {
            StringToRow(str);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {

            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }


    }
}
