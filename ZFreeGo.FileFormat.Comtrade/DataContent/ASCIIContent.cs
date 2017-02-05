using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.Common.BasicTool;

namespace ZFreeGo.FileOperation.Comtrade.DataContent
{

    /// <summary>
    /// ASCII行信息
    /// </summary>
    public class ASCIIContent
    {

        /// <summary>
        /// 模拟数量
        /// </summary>
        public int AnalogCount;
        /// <summary>
        /// 数字数量
        /// </summary>
        public int DigitalCount;
        /// <summary>
        /// 采样编号
        /// </summary>
        Int64 sampleNum;
        /// <summary>
        /// 采样编号
        /// </summary>
        public Int64 SampleIndex
        {
            get
            {
                return sampleNum;
            }
        }

        /// <summary>
        /// 采样编号，字符[1,10]，必要
        /// </summary>
        public string SampleNum
        {
            
            set
            {
                Int64 n;
                
                if (!Int64.TryParse(value, out n))
                {
                    throw new ArgumentException("SampleNum，不能转化为整数");
                }
                if ((n >= 0) && (n <= 9999999999))
                {
                    sampleNum = n;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("通道总编号范围[0,9999999999]");
                }
            }
            get
            {
                return sampleNum.ToString();
            }
        }
        /// <summary>
        /// 时标
        /// </summary>
        Int64 timeStamp;
        /// <summary>
        /// 采样时标
        /// </summary>
        public Int64 SampleTimeStamp
        {
            get
            {
                return timeStamp;
            }
        }
        /// <summary>
        /// 采时标，字符[1,10]，可选
        /// </summary>
        public string TimeStamp
        {

            set
            {
                Int64 n;

                if (!Int64.TryParse(value, out n))
                {
                    throw new ArgumentException("TimeStamp，不能转化为整数");
                }
                if ((n >= 0) && (n <= 9999999999))
                {
                    timeStamp = n;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("TimeStamp，[0,9999999999]");
                }
            }
            get
            {
                return timeStamp.ToString();
            }
        }

        /// <summary>
        /// 模拟通道数据，数组
        /// </summary>
        int[] analogChannelData;

        /// <summary>
        /// 模拟通道数据，数组
        /// </summary>
        public int[] AnalogChannelData
        {
            get
            {
                return analogChannelData;
            }
        }

        /// <summary>
        /// 数字通道数据，数组
        /// </summary>
        int[] digitalChannelData;
        /// <summary>
        /// 数字通道数据，数组
        /// </summary>
        public int[] DigitalChannelData
        {
            get
            {
                return digitalChannelData;
            }
        }

        /// <summary>
        /// 模拟数据字符串转换为值类型
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>整数</returns>
       int AnalogStringToData(string str)
        {
            int n;
            
            if (!int.TryParse(str, out n))
            {
                throw new ArgumentException("AnalogStringToData，不能转化为整数");
            }
            if ((n >= -99999) && (n <= 99999))
            {
                return n;
            }
            else
            {
                throw new ArgumentOutOfRangeException("AnalogStringToData，[-99999,99999]");
            }
        }
       /// <summary>
       /// 数字字符串转化为整数
       /// </summary>
       /// <param name="str">字符串</param>
       /// <returns>整数</returns>
       int DigitalStringToData(string str)
       {
           int n;

           if (!int.TryParse(str, out n))
           {
               throw new ArgumentException("AnalogStringToData，不能转化为整数");
           }
           if ( (n==0) ||(n==1))
           {
               return n;
           }
           else
           {
               throw new ArgumentOutOfRangeException("AnalogStringToData，{0,1}");
           }
       }
       /// <summary>
       /// 生成 厂站名标识和版本号对应的行信息字符串
       /// </summary>
       /// <returns>生成字符串</returns>
       public  string RowToString()
       {
           StringBuilder strBuilt = new StringBuilder(64);
           strBuilt.Append(SampleNum);
           strBuilt.Append(",");
           strBuilt.Append(TimeStamp);
           
          
           foreach (var m in analogChannelData)
           {
               strBuilt.Append(",");
               strBuilt.Append(m.ToString());
           }
           foreach (var m in digitalChannelData)
           {
               strBuilt.Append(",");
               strBuilt.Append(m.ToString());
           }   
           strBuilt.AppendLine();
           return strBuilt.ToString();

       }

       /// <summary>
       ///获取行信息
       /// </summary>
       /// <param name="str">行字符串</param>
       /// <param name="count">通道数量</param>
       /// <returns>false--转换成功 true--失败</returns>
       public bool StringToRow(string str, int analogCount, int digitalCount)
       {
           try
           {
               char[] charSeparators = new char[] { ',' };
               int allcn = 2 + analogCount + digitalCount;
               var result = str.Split(charSeparators, allcn, StringSplitOptions.None);
               if (result.Length != allcn)
               {
                   throw new ArgumentOutOfRangeException("数据分隔符应该限定为" + allcn.ToString()
                       + "个，必须有" + (allcn - 1).ToString() + "个'，'分隔符");
               }
               SampleNum = result[0];
               TimeStamp = result[1];
               analogChannelData = new int[analogCount];
               for (int i = 0; i < analogCount; i++)
               {
                   analogChannelData[i] = AnalogStringToData(result[2 + i]);
               }
               digitalChannelData = new int[digitalCount];
               for (int i = 0; i < digitalCount; i++)
               {
                   digitalChannelData[i] = DigitalStringToData(result[2 + analogCount + i]);
               }

               return true;
           }
           catch(Exception ex)
           {
               throw ex;
           }
       }
        /// <summary>
        /// ASCII数据初始化
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="analogCount">模拟数据数</param>
        /// <param name="digitalCount">数字数量</param>
        public ASCIIContent(string str,int analogCount, int digitalCount )
       {
           AnalogCount = analogCount;
           DigitalCount = digitalCount;
           StringToRow(str, analogCount, digitalCount);
       }
        /// <summary>
        /// 由BINARY数据转换为ASCII数据
        /// </summary>
        /// <param name="binary">二进制数据</param>
        public ASCIIContent(BinaryContent binary)
        {
            AnalogCount = binary.AnalogCount;
            DigitalCount = binary.DigitalCount;
            sampleNum = binary.SampleNum;
            timeStamp = binary.TimeStamp;
            analogChannelData = new int[binary.AnalogChannelData.Length];

            Array.Copy(binary.AnalogChannelData, analogChannelData, binary.AnalogChannelData.Length);

            digitalChannelData = new int[binary.DigitalCount];
            for (int i = 0; i < binary.DigitalCount; i++ )
            {
                digitalChannelData[i] = GetDigitalBit(binary.DigitalChannelData, (ushort)i);
            }
        }
        /// <summary>
        /// 获取状态位
        /// </summary>
        /// <param name="bitArray">位数组</param>
        /// <param name="posit">位置</param>
        /// <returns>位状态</returns>
        int GetDigitalBit(UInt16[] bitArray, UInt16 posit)
        {

            int index = posit / 16;
            int offset = posit % 16;
            bool state = ByteOperation.GetBit(bitArray[index], offset);
            if (state)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
