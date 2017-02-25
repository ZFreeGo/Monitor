using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.BasicElement
{
    /// <summary>
    /// CP56Time2a 时间
    /// </summary>
    public class CP56Time2a
    {
        /// <summary>
        /// 时间的ms数
        /// </summary>
        public UInt16 Milliseconds
        {            
            set 
            { 
                byteDataArray[0] = (byte)(value & 0x00FF);
                byteDataArray[1] = (byte)(value >> 8); 
            }
            get
            {
                return (UInt16)(byteDataArray[0] + (UInt16)byteDataArray[1] << 8);
            }
        }
         /// <summary>
         /// Invlaid 0-有效 1-无效
         /// </summary>
      
        public UInt16 IV
        {
            set
            {
                if ((0x01 & value) == 1)
                {
                    byteDataArray[2] |= 0x80;
                }
                else
                {
                    byteDataArray[2] &= 0x7F;
                }
            }
            get
            {
                if ((byteDataArray[2] & 0x80) == 0x80)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

  
        /// <summary>
        /// 分钟
        /// </summary>
        public UInt16 Minutes
        {
            set
            {
                byteDataArray[2] =  (byte)(byteDataArray[2] & 0xC0);
                byteDataArray[2] = (byte)((value & 0x3F) | byteDataArray[2]);
            }
            get
            {
                return (byte)(byteDataArray[2] & 0x3F);
            }
        }
        /// <summary>
        /// 夏令时 0-标准时间 1-夏令时间
        /// </summary>
        public UInt16 SU
        {
            set
            {
                if ((0x01 & value) == 1)
                {
                    byteDataArray[3] |= 0x80;
                }
                else
                {
                    byteDataArray[3] &= 0x7F;
                }
            }
            get
            {
               if ((byteDataArray[3] & 0x80) == 0x80)
               {
                   return 1;
               }
               else
               {
                   return 0;
               }
            }
        }
  
        /// <summary>
        /// 小时
        /// </summary>
        public UInt16 Hours
        {
            set
            {
                byteDataArray[3] &= 0xE0;
                byteDataArray[3] |= (byte)value;
            }
            get
            {
                return byteDataArray[3] &= 0x1F;
            }
        }
        /// <summary>
        /// 周几
        /// </summary>
        public UInt16 DayOfWeek
        {
            set
            {
                byteDataArray[4] &= 0x1F;
                byteDataArray[4] |= (byte)(value << 5);

            }
            get
            {
                return (UInt16)((byteDataArray[4] & 0xE0) >> 5);
            }
        }
        /// <summary>
        /// 日
        /// </summary>
        public UInt16 DayOfMonth
        {
            set
            {
                byteDataArray[4] &= 0xE0;
                byteDataArray[4] |= (byte)value;
            }
            get
            {
                return (UInt16)(byteDataArray[4] & 0x1F);
            }
        }

    
        /// <summary>
        /// 月
        /// </summary>
        public UInt16 Months
        {
            set
            {
                byteDataArray[5] &= 0xF0;
                byteDataArray[5] |= (byte)value;
            }
            get
            {
                return byteDataArray[5] &= 0x0F;
            }
        }


        /// <summary>
        /// 年
        /// </summary>
        public UInt16 Years
        {
            set
            {
                byteDataArray[6] &= 0x80;
                byteDataArray[6] |= (byte)value;
            }
            get
            {
                return (UInt16)(byteDataArray[6] & 0x7F);
            }
        }
        /// <summary>
        /// 获取DateTime格式时间
        /// </summary>
        public DateTime Time
        {
            get
            {
                return new DateTime(Years, Months, DayOfMonth, Hours, Minutes, Milliseconds / 1000, Milliseconds % 1000);
            }
        }
        /// <summary>
        /// 存储时间的字节数组
        /// </summary>
        private byte[] byteDataArray;

        /// <summary>
        /// CP56Time2a 初始化
        /// </summary>
        /// <param name="time">使用某个进行时间初始化</param>
        public CP56Time2a(DateTime time)
        {
            byteDataArray = new byte[7];
            Years = (UInt16)(time.Year - 2000);
            Months = (UInt16)time.Month;
            DayOfMonth = (UInt16)time.Day;
            DayOfWeek = (UInt16)(time.DayOfWeek + 1);
            Hours = (UInt16)time.Hour;
            Minutes = (UInt16)time.Minute;
            Milliseconds = (UInt16)(time.Millisecond + time.Second * 1000);
        }
        /// <summary>
        /// CP56Time2a 初始化,使用7字节数组
        /// </summary>
        /// <param name="array">数据数组</param>
        public CP56Time2a(byte[] array)
        {
            byteDataArray = new byte[7];
            Array.Copy(array, 0, byteDataArray, 0, 7);
        }
        /// <summary>
        /// CP56Time2a 初始化,使用7字节数组
        /// </summary>
        /// <param name="array">数据数组</param>
        /// <param name="offset">偏移</param>
        public CP56Time2a(byte[] array, int offset)
        {
            byteDataArray = new byte[7];
            Array.Copy(array, offset, byteDataArray, 0, 7);
        }
        /// <summary>
        /// 获取字节数组
        /// </summary>
        /// <returns>字节数组</returns>
        public byte[] GetDataArray()
        {
            return byteDataArray;
        }

        public override string ToString()
        {
            string str = string.Format("{0:0000}年{1:00}月{2:00}日 {3:00}时{4:00}分{5:00}秒{6:000}毫秒", (Years + 2000), Months,
                DayOfMonth, Hours, Minutes, (int)Milliseconds / 1000, Milliseconds%1000);
            return str;

        }

        /// <summary>
        /// 比较两个时间戳所指的时间是否相等，
        /// </summary>
        /// <param name="time">另一个时间戳</param>
        /// <returns>比较结果，相同返回true，不同返回false</returns>
        public bool IsEqual(CP56Time2a time)
        {
            var data = time.GetDataArray();
            for(int  i =0 ; i < 7; i++)
            {
                if (data[i] != byteDataArray[i])
                {
                    return false;
                }
            }
            return true;
        }


        


    }
}
