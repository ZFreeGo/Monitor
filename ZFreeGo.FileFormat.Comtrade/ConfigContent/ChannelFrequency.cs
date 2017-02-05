using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZFreeGo.FileOperation.Comtrade.ConfigContent
{
    /// <summary>
    /// 标称通道频率
    /// </summary>
    public class ChannelFrequency : RowOperation, INotifyPropertyChanged
    {
        /// <summary>
        /// 通道标称频率,字符[1,32] ,可选
        /// </summary>
        double channelFrequency;

        /// <summary>
        /// 通道标称频率,字符[1,32] ,可选
        /// </summary>
        public string ChannelFrequencyNormal
        {
            get
            {
                return  channelFrequency.ToString();
            }
            set
            {
                double n ;
                if (!double.TryParse(value, out n))
                {
                    throw new ArgumentOutOfRangeException("ChannelFrequencyNormal，不能转化为浮点数");
                }
                if (n > 0)
                {
                 channelFrequency = n;
                 OnPropertyChanged(new PropertyChangedEventArgs("ChannelFrequencyNormal"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("通道标称频率，需要是个能转换为浮点数且大于0的字符串");
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
            strBuilt.Append(ChannelFrequencyNormal);
           
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
            ChannelFrequencyNormal = str;


            return true;
        }
        /// <summary>
        /// 初始化通道频率
        /// </summary>
        /// <param name="str">频率字符</param>
        public ChannelFrequency(string str)
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
