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
    /// 数据（状态）通道信息
    /// </summary>
    public class DigitalChannelInformation : RowOperation, INotifyPropertyChanged
    {
        /// <summary>
        /// 通道索引编号,字符[1,6],范围[1,999999],必要
        /// </summary>
        int channelIndex;

        /// <summary>
        /// 通道索引编号,字符[1,6],范围[1,999999],必要
        /// </summary>
        public string ChannelIndex
        {
            get
            {
                return channelIndex.ToString();
            }
            set
            {
                int n;
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
        /// 状态通道正常状态（仅用于状态通道）
        /// </summary>
        int statusNormal;
        /// <summary>
        /// 状态通道正常状态（仅用于状态通道）,1字符,1或0
        /// </summary>
        public string StatusNormal
        {
            get
            {
                return statusNormal.ToString();
            }
            set
            {
                var str = value.Trim();
                if ((str == "1") || (str == "0"))
                {
                    statusNormal = int.Parse(str);
                    OnPropertyChanged(new PropertyChangedEventArgs("StatusNormal"));
                   
                }
                else
                {
                    throw new ArgumentOutOfRangeException("状态通道正常状态 字符只能为‘1’或‘0’");
                }
            }
        }

        /// <summary>
        /// 初始化数字(状态通道信息)
        /// </summary>
        /// <param name="rowStr">行信息</param>
        public DigitalChannelInformation(string rowStr)
        {
            StringToRow(rowStr);
        }

        /// <summary>
        /// 生成 数字（模拟）通道信息
        /// </summary>
        /// <returns>生成字符串</returns>
        public override string RowToString()
        {
            StringBuilder strBuilt = new StringBuilder(64);
            strBuilt.Append(ChannelIndex);
            strBuilt.Append(",");
            strBuilt.Append(ChannelID);
            strBuilt.Append(",");
            strBuilt.Append(MonitorComponent);
            strBuilt.Append(",");
            strBuilt.Append(ChannelPhaseID);
            strBuilt.Append(",");
            strBuilt.Append(StatusNormal);

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
            var result = str.Split(charSeparators, 5, StringSplitOptions.None);
            if (result.Length != 5)
            {
                throw new ArgumentOutOfRangeException("数据分隔符应该限定为5个，必须有4个'，'分隔符");
            }
            ChannelIndex = result[0];
            ChannelID = result[1];
            MonitorComponent = result[2];
            ChannelPhaseID = result[3];
            StatusNormal = result[4];
            

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
