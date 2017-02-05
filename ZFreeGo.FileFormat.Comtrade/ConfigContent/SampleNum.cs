using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ZFreeGo.Common.BasicTool;

namespace ZFreeGo.FileOperation.Comtrade.ConfigContent
{
    /// <summary>
    /// 采样数
    /// </summary>
    public class SampleNum : RowOperation, INotifyPropertyChanged
    {
        /// <summary>
        /// 数据文件中的采样速率数,字符[1,3] ,必要
        /// </summary>
        int sampleRateNum;

        /// <summary>
        /// 数据文件中的采样速率数,字符[1,3] ,必要
        /// </summary>
        public string SampleRateNum
        {
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 1, 3))
                {
                    if (!int.TryParse(str,out sampleRateNum))
                    {
                        throw new ArgumentException("数据文件中的采样速率数,不能转换成整数");
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("SampleRateNum"));
                    
                }
                else
                {
                    throw new ArgumentOutOfRangeException("数据文件中的采样速率数,字符[1,3]");
                }
            }
            get
            {
                return sampleRateNum.ToString();
            }
        }

        /// <summary>
        /// 由行信息获取标称频率字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string RowToString()
        {
            StringBuilder strBuilt = new StringBuilder(16);
            strBuilt.Append(SampleRateNum);

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
            SampleRateNum = str;


            return true;
        }
        /// <summary>
        /// 初始化采样速率数
        /// </summary>
        /// <param name="rowStr">行信息</param>
        public SampleNum(string rowStr)
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
