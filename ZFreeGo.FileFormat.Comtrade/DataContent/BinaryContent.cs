using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.Common.BasicTool;

namespace ZFreeGo.FileOperation.Comtrade.DataContent
{
    /// <summary>
    /// Binary行信息
    /// </summary>
    public class BinaryContent
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
        UInt32 sampleNum;
        /// <summary>
        /// 采样编号
        /// </summary>
        public UInt32 SampleNum
        {
            get
            {
                return sampleNum;
            }
        }

        /// <summary>
        /// 时标
        /// </summary>
        UInt32 timeStamp;
        /// <summary>
        /// 时标
        /// </summary>
        public  UInt32 TimeStamp
        {
            get
            {
                return timeStamp;
            }
        }
        /// <summary>
        /// 模拟通道数据，数组
        /// </summary>
        UInt16[] analogChannelData;
        public Int16[] AnalogChannelData
        {
            get
            {
                return UIntToIntArray16(analogChannelData);
            }

        }
        /// <summary>
        /// 数字通道数据,数组
        /// </summary>
        UInt16[] digitalChannelData;
        /// <summary>
        /// 数字通道数据,数组
        /// </summary>
        public UInt16[]  DigitalChannelData
        {
            get
            {
                return digitalChannelData;
            }

        }
        /// <summary>
        /// 无符号16bit数组强制转化为有符号16bit数组
        /// </summary>
        /// <param name="data">无符号16bit数组</param>
        /// <returns>有符号16bit数组</returns>

        Int16[] UIntToIntArray16(UInt16[] data)
        {
             Int16[] array = new Int16[ data.Length];
             for (int i = 0; i < data.Length; i++ )
             {
                 array[i] = (Int16)data[i];
             }
             return array;
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
       /// 生成 二进制字节数组
       /// </summary>
       /// <returns>生成字符串</returns>
       public byte[] RowToByteArray()
       {
           try
           {
            int len = 4 + 4 + analogChannelData.Length * 2 +digitalChannelData.Length * 2;
           byte[] data = new byte[len];
           int index = 0;
           data[index++] = ByteOperation.GetBit7_0(sampleNum);
           data[index++] = ByteOperation.GetBit15_8(sampleNum);
           data[index++] = ByteOperation.GetBit23_16(sampleNum);
           data[index++] = ByteOperation.GetBit31_24(sampleNum);
           data[index++] = ByteOperation.GetBit7_0(timeStamp);
           data[index++] = ByteOperation.GetBit15_8(timeStamp);
           data[index++] = ByteOperation.GetBit23_16(timeStamp);
           data[index++] = ByteOperation.GetBit31_24(timeStamp);
           foreach (var m in analogChannelData)
           {
                data[index++] = ByteOperation.GetBit7_0(m);
                data[index++] = ByteOperation.GetBit15_8(m);
           }
           foreach (var m in digitalChannelData)
           {
                data[index++] = ByteOperation.GetBit7_0(m);
                data[index++] = ByteOperation.GetBit15_8(m);
           }
               return data;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

        /// <summary>
        /// 获取16bit数据个数
        /// </summary>
        /// <param name="digitalCount">数字量数目</param>
        /// <returns>需要16bit数目</returns>
        int GetUshortCount(int digitalCount)
       {
             int digital = 0;
           
           if ((digitalCount % 16) > 0)
           {
               digital = digital / 16 + 1;
           }
           else
           {
               digital = digital / 16;
           }
           return digital;
       }
       /// <summary>
       ///获取行信息
       /// </summary>
       /// <param name="str">行字符串</param>
       /// <param name="count">通道数量</param>
       /// <returns>false--转换成功 true--失败</returns>
       public bool ByteToRow(byte[] data, int analogCount, int digitalCount)
       {
           try
           {

               //TODO:每次调用重复计算，影响效率
               int digital = GetUshortCount(digitalCount);               

                int len = 4 + 4 + analogCount * 2 + digital * 2;
               if (data.Length < len)
               {
                   throw new ArgumentException("ByteToRow，字节数组长度小于要求值");
               }
               analogChannelData = new UInt16[analogCount];
               digitalChannelData = new UInt16[digital];

               int index = 0;
               sampleNum = ByteOperation.CombinationByte(data[index++],data[index++],data[index++],data[index++] );
               timeStamp = ByteOperation.CombinationByte(data[index++],data[index++],data[index++],data[index++] );
               for(int j = 0; j < analogCount; j++)
               {
                  analogChannelData[j] = ByteOperation.CombinationByte(data[index++],data[index++]);
               }
               for(int j = 0; j < digital; j++)
               {
                   digitalChannelData [j] = ByteOperation.CombinationByte(data[index++],data[index++]);
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
       /// <param name="data">字节数组</param>
        /// <param name="analogCount">模拟数据数</param>
        /// <param name="digitalCount">数字数量</param>
       public BinaryContent(byte[] data, int analogCount, int digitalCount)
       {
           AnalogCount = analogCount;
           DigitalCount= digitalCount;
           ByteToRow(data, analogCount, digitalCount);
       }
       public BinaryContent(ASCIIContent assiiContent)
       {
           AnalogCount = assiiContent.AnalogCount;
           DigitalCount= assiiContent.DigitalCount;
           sampleNum =(UInt32) assiiContent.SampleIndex;
           timeStamp = (UInt32)assiiContent.SampleTimeStamp;
           analogChannelData = new ushort[AnalogCount];

           for (int i = 0; i < AnalogCount; i++)
           {
               analogChannelData[i] = (ushort)assiiContent.AnalogChannelData[i];
           }
             //  Array.Copy(assiiContent.AnalogChannelData, analogChannelData, AnalogCount);

           //int digital = GetUshortCount(DigitalCount);
           //digitalChannelData = new ushort[digital];
           digitalChannelData = ByteOperation.BitAssemblyUsort16(assiiContent.DigitalChannelData);


       }


    }
}
