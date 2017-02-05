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
    /// 通道编号和类型
    /// </summary>
    public class ChannelNumType : RowOperation, INotifyPropertyChanged
    {
        /// <summary>
        /// 通道总编号
        /// </summary>
        int channelTotalNum;
        /// <summary>
        /// 通道总数
        /// </summary>
        public int ChannelCount
        {
            get { return channelTotalNum; }
        }
    
        /// <summary>
        /// 通道总编号,1-7数字字符
        /// </summary>
        public string ChannelTotalNum
        {
           
            get
            {
                return channelTotalNum.ToString();
            }
            set
            {
                channelTotalNum =   RemoveLastCharToParse(value, 'X');
                OnPropertyChanged(new PropertyChangedEventArgs("ChannelTotalNum"));
            }
        }

        
        /// <summary>
        /// 模拟通道编号 1-6个数字字符 + "A"
        /// </summary>
        int analogChannelNum;
        /// <summary>
        /// 模拟通道总数
        /// </summary>
        public int AnalogChannelCount
        {
            get { return analogChannelNum; }
        }
        /// <summary>
        /// 模拟通道编号
        /// </summary>
        public string AnalogChannelNum
        {
           
            get
            {
                return analogChannelNum.ToString() + "A";
            }
            set
            {
                analogChannelNum = RemoveLastCharToParse(value, 'A');
                OnPropertyChanged(new PropertyChangedEventArgs("AnalogChannelNum"));
            }
        }

        /// <summary>
        /// 状态通道编号 1-6个数字字符 + "D"
        /// </summary>
        int digitalChannelNum;
        /// <summary>
        /// 数字通道总数
        /// </summary>
        public int DigitalChannelCount
        {
            get { return digitalChannelNum; }
        }
        /// <summary>
        /// 状态通道编号
        /// </summary>
        public string DigitalChannelNum
        {
           
            get
            {
                return digitalChannelNum.ToString() + "D";

            }
            set
            {
                digitalChannelNum  = RemoveLastCharToParse(value, 'D');
                OnPropertyChanged(new PropertyChangedEventArgs("DigitalChannelNum"));
            }
        }

        /// <summary>
        /// 设置通道号，模拟与数字通道之和需要等于总通道号
        /// </summary>
        /// <param name="channelNum">总通道号</param>
        /// <param name="analogNum">模拟通道号</param>
        /// <param name="digitalNum">数字通道号</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool SetChannelNum(int channelNum, int analogNum, int digitalNum)
        {
            if ((analogNum + digitalNum) == channelNum)
            {
                channelTotalNum = channelNum;
                analogChannelNum = analogNum;
                digitalChannelNum = digitalNum;
                return true;
            }
            else
            {
                throw new ArgumentException("模拟与数字通道之和不等于等于总通道号");
            }

        }
        /// <summary>
        /// 初始化通道类型
        /// </summary>
        /// <param name="str">通道类型字符串</param>
        public ChannelNumType(string str)
        {
            StringToRow(str);
        }
        /// <summary>
        /// 初始化通道类型与编号
        /// </summary>
        /// <param name="channelNum"></param>
        /// <param name="analogNum"></param>
        /// <param name="digitalNum"></param>
        public ChannelNumType(int channelNum, int analogNum, int digitalNum)
        {
            SetChannelNum(channelNum, analogNum, digitalNum);
        }

        /// <summary>
        /// 生成 通道编号和类型字符串
        /// </summary>
        /// <returns>生成字符串</returns>
        public override string RowToString()
        {
            StringBuilder strBuilt = new StringBuilder(32);
            strBuilt.Append(ChannelTotalNum);
            strBuilt.Append(",");
            strBuilt.Append(AnalogChannelNum);
            strBuilt.Append(",");
            strBuilt.Append(DigitalChannelNum);
            strBuilt.AppendLine();
            return strBuilt.ToString();

        }

        /// <summary>
        ///获取 通道编号和类型 将行字符串转化为对应行信息。
        /// </summary>
        /// <param name="str">行字符串</param>
        /// <returns>false--转换成功 true--失败</returns>
        public override bool StringToRow(string str)
        {
            char[] charSeparators = new char[] { ',' };
            var result = str.Split(charSeparators, 3, StringSplitOptions.None);
            if (result.Length != 3)
            {
                throw new ArgumentOutOfRangeException("数据分隔符应该限定为3个，必须有俩个'，'分隔符");
            }
            var allnNum = RemoveLastCharToParse(result[0], 'X');
            var aNum = RemoveLastCharToParse(result[1], 'A');
            var dNum = RemoveLastCharToParse(result[2], 'D');
            SetChannelNum(allnNum, aNum, dNum);

            return true;
        }

        /// <summary>
        /// 移除最后一个字节，并进行转换
        /// </summary>
        /// <param name="value">待转换的字符串</param>
        /// <param name="ch">移除的字符</param>
        private int RemoveLastCharToParse(string value, char ch)
        {
            value = value.Trim();
            if (ch != 'X')
            {

                if (char.ToUpper(value[value.Length - 1]) != ch)
                {
                    throw new ArgumentException("状态通道编号结尾需要以" + ch + "结束");
                }
             value = value.Remove(value.Length - 1, 1);
            }
            
            int num;
            if (!int.TryParse(value, out num))
            {
                throw new ArgumentOutOfRangeException("通道号不能转换为整数");
            }


            if ((0 <= num) && (num <= 999999))
            {
                return num;
            }
            else
            {
                throw new ArgumentOutOfRangeException("通道总编号范围[0,999999]");
            }


        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {

            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    
    }
}
