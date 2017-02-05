using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ZFreeGo.Common.BasicTool;

namespace ZFreeGo.FileOperation.Comtrade.ConfigContent
{
    /// <summary>
    /// 采样速率
    /// </summary>
    public class SampleRateInformation : RowOperation, INotifyPropertyChanged
    {
       
        /// <summary>
        /// 数据文件中的采样速率,字符[1,32]，可选 
        double sampleRate;

        /// <summary>
        /// 数据文件中的采样速率,字符[1,32],可选
        /// </summary>
        public string SampleRate
        {
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 1, 32))
                {
                    if (!double.TryParse(str, out sampleRate))
                    {
                        throw new ArgumentException("数据文件中的采样速率,不能转换成浮点数");
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("SampleRate"));

                }
                else
                {
                    throw new ArgumentOutOfRangeException("数据文件中的采样速率,字符[1,3]");
                }
            }
            get
            {
                return sampleRate.ToString();
            }
        }
        

        /// <summary>
        /// 数据文件中的最终采样数,字符[1,10] ,必要
        /// </summary>
        int endSample;

        /// <summary>
        /// 数据文件中的最终采样数
        /// </summary>
        public int EndSampleCount
        {
            get
            {
                return endSample;
            }
        }

        /// <summary>
        /// 数据文件中的最终采样数,字符[1,10] ,必要
        /// </summary>
        public string EndSample
        {
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 1, 10))
                {
                    if (!int.TryParse(str, out endSample))
                    {
                        throw new ArgumentException("数据文件中的最终采样数,不能转换成整数");
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("EndSample"));

                }
                else
                {
                    throw new ArgumentOutOfRangeException("数据文件中的最终采样数,字符[1,10]");
                }
            }
            get
            {
                return endSample.ToString();
            }
        }


        /// <summary>
        /// 由行信息获取采样字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string RowToString()
        {
            StringBuilder strBuilt = new StringBuilder(16);
            strBuilt.Append(SampleRate);
            strBuilt.Append(",");
            strBuilt.Append(EndSample);
            strBuilt.AppendLine();
            return strBuilt.ToString();

        }

        /// <summary>
        ///获取 采样速率字符
        /// </summary>
        /// <param name="str">行字符串</param>
        /// <returns>false--转换成功 true--失败</returns>
        public override bool StringToRow(string str)
        {
            char[] charSeparators = new char[] { ',' };
            var result = str.Split(charSeparators, 2, StringSplitOptions.None);
            if (result.Length != 2)
            {
                throw new ArgumentOutOfRangeException("数据分隔符应该限定为2个，必须有1个'，'分隔符");
            }
            SampleRate = result[0];
            EndSample = result[1];

            return true;
        }
        /// <summary>
        /// 由行字符串初始化行信息
        /// </summary>
        /// <param name="rowStr">行字符串</param>
        public SampleRateInformation(string rowStr)
        {
            StringToRow(rowStr);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {

            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
