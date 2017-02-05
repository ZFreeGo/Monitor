using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZFreeGo.Common.BasicTool;

namespace ZFreeGo.FileOperation.Comtrade.ConfigContent
{
    /// <summary>
    /// 模拟通道信息
    /// </summary>
    public class AnalogChannelInformation : RowOperation, INotifyPropertyChanged
    {
        /// <summary>
        /// 通道索引编号,字符[1,32], [0-999999],必要
        /// </summary>
        int channelIndex;

        /// <summary>
        /// 通道索引编号,字符[1,32], [0-999999],必要
        /// </summary>
        public string ChannelIndex
        {
            get
            {
                return channelIndex.ToString();
            }
            set
            {
                int n ;
                if (!int.TryParse(value, out n))
                {
                    throw new ArgumentOutOfRangeException("ChannelIndex，不能转化为整数");
                }
                if ((n >= 0) && (n <= 999999))
                {
                    channelIndex = n;
                    OnPropertyChanged(new PropertyChangedEventArgs("ChannelIndex"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("通道总编号范围[0,999999]");
                }
            }
        }

        /// <summary>
        /// 通道标识，字符[0,64]，可选
        /// </summary>
        string channelID;

        /// <summary>
        /// 通道标识，字符[0,64]，可选
        /// </summary>
        public string ChannelID
        {
            get
            {
                return channelID;
            }
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 0, 64))
                {
                    channelID = str;
                    OnPropertyChanged(new PropertyChangedEventArgs("ChannelID"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("通道标识 字符串长度[0,64]");
                }
            }
        }
    
        /// <summary>
        /// 通道相别标识,字符[0,2],可选
        /// </summary>
        string channelPhaseID;
         /// <summary>
        /// 通道相别标识,字符[0,2],可选
        /// </summary>
        public string ChannelPhaseID
         {
            get
            {
                return channelPhaseID;
            }
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 0, 2))
                {
                    channelPhaseID = str;
                    OnPropertyChanged(new PropertyChangedEventArgs("ChannelPhaseID"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("通道相别标识 字符串长度[0,2]");
                }
            }
        }
        /// <summary>
        /// 被监视的电路元件，字符[0,64]，可选
        /// </summary>
        string monitorComponent;
        /// <summary>
        /// 被监视的电路元件，字符[0,64]，可选
        /// </summary>
        public string MonitorComponent
        {
            get
            {
                return monitorComponent;
            }
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 0, 64))
                {
                    monitorComponent = str;
                    OnPropertyChanged(new PropertyChangedEventArgs("MonitorComponent"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("被监视的电路元件 字符串长度[0,64]");
                }
            }
        }
        /// <summary>
        /// 通道单位，字符[1,32]，必要
        /// </summary>
        string channelUnit;
        /// <summary>
        /// 通道单位，字符[1,32]，必要
        /// </summary>
        public string ChannelUnit
        {
            get
            {
                return channelUnit;
            }
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 1, 32))
                {
                    channelUnit = str;
                    OnPropertyChanged(new PropertyChangedEventArgs("ChannelUnit"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("通道单位 字符串长度[1,32]");
                }
            }
        }

        /// <summary>
        /// 通道增益系数，字符[1,32]，必要
        /// </summary>
        double channelGain;
        /// <summary>
        /// 通道增益系数，字符[1,32]，必要
        /// </summary>
        public string ChannelGain
        {
            get
            {
                return channelGain.ToString(); 

            }
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 1, 32))
                {
                    if (!double.TryParse(str, out channelGain))
                    {
                        throw new ArgumentOutOfRangeException("channelGain，不能转化为浮点数");
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("ChannelUnit"));

                }
                else
                {
                    throw new ArgumentOutOfRangeException("通道增益 字符串长度[1,32]");
                }
            }
        }

        /// <summary>
        /// 通道偏移因子，字符[1,32]，必要
        /// </summary>
        double channelOffset;
        /// <summary>
        /// 通道偏移因子，字符[1,32]，必要
        /// </summary>
        public string ChannelOffset
        {
            get
            {
                return channelOffset.ToString();

            }
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 1, 32))
                {
                    if (!double.TryParse(str, out channelOffset))
                    {
                        throw new ArgumentOutOfRangeException("通道偏移因子，不能转化为浮点数");
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("ChannelOffset"));

                }
                else
                {
                    throw new ArgumentOutOfRangeException("通道偏移因子 字符串长度[1,32]");
                }
            }
        }
        
        /// <summary>
        /// 从采样时段起始的通道时间时滞,字符长度[1,32],可选。
        /// </summary>
        double skewing;
        /// <summary>
        /// 从采样时段起始的通道时间时滞,字符长度[1,32],可选。
        /// </summary>
        public string Skewing
        {
            get
            {
                return skewing.ToString();

            }
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 1, 32))
                {
                     
                    if (!double.TryParse(str, out skewing))
                    {
                        throw new ArgumentOutOfRangeException("时滞，不能转化为浮点数");
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("Skewing"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("时滞 字符串长度[1,32]");
                }
            }
        }


        /// <summary>
        /// 该通道数值范围中的最小值,1-6字符，[-999999,99999]，必要
        /// </summary>
        int min;
        /// <summary>
        /// 该通道数值范围中的最小值,1-6字符，[-999999,99999]，必要
        /// </summary>
        public String Min
        {
            get
            {
                return min.ToString();

            }
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 1, 6))
                {
               
                    if (!int.TryParse(str, out min))
                    {
                        throw new ArgumentOutOfRangeException("最小值,不能转换为整数");
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("Min"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("最小值 字符串长度[1,6]");
                }
            }
        }
        /// <summary>
        /// 该通道数值范围内的最大值，1-6字符，[-999999,99999]，必要
        /// </summary>
        int max;
        /// <summary>
        /// 该通道数值范围中的最大值,1-6字符，[-999999,99999]，必要
        /// </summary>
        public String Max
        {
            get
            {
                return max.ToString();

            }
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 1, 6))
                {
                    if (!int.TryParse(str, out max))
                    {
                        throw new ArgumentOutOfRangeException("max,不能转换为整数");
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("Max"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("最大值 字符串长度[1,6]");
                }
            }
        }
        /// <summary>
        /// 通道电压或电流互感器变比一次因子，字符[1,32]，必要
        /// </summary>
        double primary;
        /// <summary>
        /// 通道电压或电流互感器变比一次因子，字符[1,32]，必要
        /// </summary>
        public String Primary
        {
            get
            {
                return primary.ToString();

            }
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 1, 32))
                {
                    if (!double.TryParse(str, out primary))
                    {
                        throw new ArgumentOutOfRangeException("Primary,不能转换为浮点数");
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("Primary"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Primary 字符串长度[1,32]");
                }
            }
        }



        /// <summary>
        /// 通道电压或电流互感器变比二次因子，字符[1,32]，必要
        /// </summary>
        double secondary;
        /// <summary>
        /// 通道电压或电流互感器变比二次因子，字符[1,32]，必要
        /// </summary>
        public String Secondary
        {
            get
            {
                return secondary.ToString();

            }
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 1, 32))
                {

                    if (!double.TryParse(str, out secondary))
                    {
                        throw new ArgumentOutOfRangeException("secondary,不能转换为浮点数");
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("Secondary"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("secondary 字符串长度[1,32]");
                }
            }
        }
        /// <summary>
        /// 通道还原的为一次值（P）还是二次值（S）,1个字符，有效为P,p,S,s，必要
        /// </summary>
        string ps;
        /// <summary>
        /// 通道还原的为一次值（P）还是二次值（S）,1个字符，有效为P,p,S,s，必要
        /// </summary>
        public string PS
        {
            get
            {
                return ps;

            }
            set
            {

                string str = value.Trim().ToUpper();
                if ((str == "P") || (str == "S"))
                {
                    ps = str;
                    OnPropertyChanged(new PropertyChangedEventArgs("PS"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("有效为P,p,S,s");
                }

            }
        }

        /// <summary>
        /// 初始化模拟信息通道信息
        /// </summary>
        /// <param name="rowStr">行信息</param>
        public AnalogChannelInformation(string rowStr)
        {
            StringToRow(rowStr);
        }
        /// <summary>
        /// 生成 模拟通道信息
        /// </summary>
        /// <returns>生成字符串</returns>
        public override string RowToString()
        {
            StringBuilder strBuilt = new StringBuilder(128);
            strBuilt.Append(ChannelIndex);
            strBuilt.Append(",");
            strBuilt.Append(ChannelID);
            strBuilt.Append(",");
            strBuilt.Append(MonitorComponent);
            strBuilt.Append(",");
            strBuilt.Append(ChannelPhaseID);
            strBuilt.Append(",");
            strBuilt.Append(ChannelUnit);
            strBuilt.Append(",");
            strBuilt.Append(ChannelGain);
            strBuilt.Append(",");
            strBuilt.Append(ChannelOffset);
            strBuilt.Append(",");
            strBuilt.Append(Skewing);
            strBuilt.Append(",");
            strBuilt.Append(Min);
            strBuilt.Append(",");
            strBuilt.Append(Max);
            strBuilt.Append(",");
            strBuilt.Append(Primary);
            strBuilt.Append(",");
            strBuilt.Append(Secondary);
            strBuilt.Append(",");
            strBuilt.Append(PS);
            strBuilt.AppendLine();
            return strBuilt.ToString();

        }

        /// <summary>
        ///获取 模拟通道信息
        /// </summary>
        /// <param name="str">行字符串</param>
        /// <returns>false--转换成功 true--失败</returns>
        public override bool StringToRow(string str)
        {
            char[] charSeparators = new char[] { ',' };           
            var result = str.Split(charSeparators, 13, StringSplitOptions.None);
            if (result.Length != 13)
            {
                throw new ArgumentOutOfRangeException("数据分隔符应该限定为13个，必须有12个'，'分隔符");
            }
            ChannelIndex = result[0];
            ChannelID = result[1];            
            MonitorComponent= result[2];            
            ChannelPhaseID= result[3];            
            ChannelUnit= result[4];            
            ChannelGain= result[5];            
            ChannelOffset= result[6];            
            Skewing= result[7];            
            Min= result[8];            
            Max= result[9];            
            Primary= result[10];            
            Secondary= result[11];            
            PS= result[12];


            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {

            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }







    }
}
