using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ZFreeGo.Common.BasicTool;

namespace ZFreeGo.FileOperation.Comtrade.ConfigContent
{
    /// <summary>
    /// 时标日期
    /// </summary>
    public class DateStamp : RowOperation, INotifyPropertyChanged
    {
        /// <summary>
        /// 天，字符[1,2],可选
        /// </summary>
        int dayOfMonth;
        /// <summary>
        /// 天，字符[1,2],可选
        /// </summary>
        public string DayOfMonth
        {
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 1, 2))
                {
                    int data;
                    if (int.TryParse(str, out data))
                    {
                        if ((data >=1) && (data <=31))
                        {
                            dayOfMonth = data;
                            OnPropertyChanged(new PropertyChangedEventArgs("DayOfMonth"));
                        }
                        else
                        {
                            throw new ArgumentException("天,[1,31]");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("天,不能转换成整数");
                    }


                }
                else
                {
                    throw new ArgumentOutOfRangeException("天,字符[1,2]");
                }
            }
            get
            {
                return dayOfMonth.ToString().PadLeft(2, '0');
            }
        }
        /// <summary>
        /// 月，字符[1,2],可选
        /// </summary>
        int month;
        /// <summary>
        /// 月，字符[1,2],可选
        /// </summary>
        public string Month
        {
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 1, 2))
                {
                    int data;
                    if (int.TryParse(str, out data))
                    {
                        if ((data >= 1) && (data <= 12))
                        {
                            month = data;
                            OnPropertyChanged(new PropertyChangedEventArgs("Month"));
                        }
                        else
                        {
                            throw new ArgumentException("月,[1,12]");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("月,不能转换成整数");
                    }


                }
                else
                {
                    throw new ArgumentOutOfRangeException("月,字符[1,2]");
                }
            }
            get
            {
                return month.ToString().PadLeft(2, '0');
            }
        }

        /// <summary>
        /// 年，字符[4,4],必要
        /// </summary>
        int year;
        /// <summary>
        /// 年，字符[4,4],必要
        /// </summary>
        public string Year
        {
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 4, 4))
                {
                    int data;
                    if (int.TryParse(str, out data))
                    {
                        if ((data >= 1900) && (data <= 9999))
                        {
                            year= data;
                            OnPropertyChanged(new PropertyChangedEventArgs("Year"));
                        }
                        else
                        {
                            throw new ArgumentException("年,[1900,9999]");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("年,不能转换成整数");
                    }


                }
                else
                {
                    throw new ArgumentOutOfRangeException("年,字符[1,2]");
                }
            }
            get
            {
                return year.ToString().PadLeft(4, '0');
            }
        }
        /// <summary>
        /// 小时，字符[2,2],必要
        /// </summary>
        int hour;
        /// <summary>
        /// 小时，字符[2,2],必要
        /// </summary>
        public string Hour
        {
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 2, 2))
                {
                    int data;
                    if (int.TryParse(str, out data))
                    {
                        if ((data >= 0) && (data <= 23))
                        {
                            hour = data;
                            OnPropertyChanged(new PropertyChangedEventArgs("Hour"));
                        }
                        else
                        {
                            throw new ArgumentException("小时,[0,23]");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("小时,不能转换成整数");
                    }


                }
                else
                {
                    throw new ArgumentOutOfRangeException("小时,字符[2,2]");
                }
            }
            get
            {
                return hour.ToString().PadLeft(2, '0');
            }
        }

        /// <summary>
        /// 分钟，字符[2,2],必要
        /// </summary>
        int minute;
        /// <summary>
        /// 分钟，字符[2,2],必要
        /// </summary>
        public string Minute
        {
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 2, 2))
                {
                    int data;
                    if (int.TryParse(str, out data))
                    {
                        if ((data >= 0) && (data <= 59))
                        {
                            minute = data;
                            OnPropertyChanged(new PropertyChangedEventArgs("Minute"));
                        }
                        else
                        {
                            throw new ArgumentException("分钟,[0,59]");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("分钟,不能转换成整数");
                    }


                }
                else
                {
                    throw new ArgumentOutOfRangeException("分钟,字符[2,2]");
                }
            }
            get
            {

                return minute.ToString().PadLeft(2, '0');
            }
        }
        /// <summary>
        /// 秒，字符[9,9],必要
        /// </summary>
        double second;
        /// <summary>
        /// 秒，字符[9,9],必要
        /// </summary>
        public string Second
        {
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 9, 9))
                {
                    double data;
                    if (double.TryParse(str, out data))
                    {
                        if ((data >= 0) && (data <= 59.999999))
                        {
                            second = data;
                            OnPropertyChanged(new PropertyChangedEventArgs("Second"));
                        }
                        else
                        {
                            throw new ArgumentException("秒,[0,59.999999]");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("秒,不能转换成整数");
                    }


                }
                else
                {
                    throw new ArgumentOutOfRangeException("秒,字符[9,9]");
                }
            }
            get
            {
                var t = string.Format("{0:f6}", second);
                return t.PadLeft(9, '0');
            }
        }

        /// <summary>
        /// 由行信息获取采样字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string RowToString()
        {
            StringBuilder strBuilt = new StringBuilder(16);
            strBuilt.Append(DayOfMonth);
            strBuilt.Append("/");
            strBuilt.Append(Month);
            strBuilt.Append("/");
            strBuilt.Append(Year);
            strBuilt.Append(",");
            strBuilt.Append(Hour);
            strBuilt.Append(":");
            strBuilt.Append(Minute);
            strBuilt.Append(":");
            strBuilt.Append(Second);
            strBuilt.AppendLine();
            return strBuilt.ToString();

        }

        /// <summary>
        ///获取 时间戳字符串
        /// </summary>
        /// <param name="str">行字符串</param>
        /// <returns>false--转换成功 true--失败</returns>
        public override bool StringToRow(string str)
        {
            //以逗号进行分割
            char[] charSeparators = new char[] { ',' };
            var result = str.Split(charSeparators, 2, StringSplitOptions.None);
            if (result.Length != 2)
            {
                throw new ArgumentOutOfRangeException("数据分隔符应该限定为2个，必须有1个'，'分隔符");
            }

            //以‘/’进行分割
            char[] charSeparats = new char[] { '/' };
            var date = result[0].Split(charSeparats, 3, StringSplitOptions.None);
            if (date.Length != 3)
            {
                throw new ArgumentException("数据分隔符应该限定为3个，必须有3个'/'分隔符");
            }
            DayOfMonth = date[0];
            Month = date[1];
            Year = date[2];

            //以 '：'进行分割
            char[] charSep = new char[] { ':' };
            var time = result[1].Split(charSep, 3, StringSplitOptions.None);
            if (time.Length != 3)
            {
                throw new ArgumentException("数据分隔符应该限定为3个，必须有3个':'分隔符");
            }
            Hour = time[0];
            Minute = time[1];
            Second = time[2];

            return true;
        }


        /// <summary>
        /// 初始化时标信息
        /// </summary>
        /// <param name="rowStr">时标字符串</param>
        public DateStamp(string rowStr)
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
