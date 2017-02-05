using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.FileOperation.Comtrade.ConfigContent
{
    /// <summary>
    /// Comtrade配置文件
    /// </summary>
    public class ComtradeConfigFile
    {
        /// <summary>
        /// 最小行数
        /// </summary>
        const int  minRowCount = 9;
        /// <summary>
        /// 行的数量
        /// </summary>
        int rowCount;
        /// <summary>
        /// 行信息
        /// </summary>
        List<RowOperation> rowList;
        /// <summary>
        /// 行信息对应字符串
        /// </summary>
        string[] rowString;
        /// <summary>
        /// 整个文件信息
        /// </summary>
        string fileString;

        /// <summary>
        /// 站，版本信息
        /// </summary>
        public StationRev RowStationRev;
        /// <summary>
        /// 通道数与通道类型信息
        /// </summary>
        public ChannelNumType RowChannelNumType;
        /// <summary>
        /// 模拟通道信息合集
        /// </summary>
        public AnalogChannelInformation[] RowAnalogChannel;
        /// <summary>
        /// 数字通道信息合集
        /// </summary>
        public DigitalChannelInformation[] RowDigitalChannel;
        /// <summary>
        /// 通道标称频率
        /// </summary>
        public ChannelFrequency RowChannelFrequency;
        /// <summary>
        /// 采样数信息
        /// </summary>
        public SampleNum RowSampleNum;

        /// <summary>
        /// 采样速率信息
        /// </summary>
        public SampleRateInformation RowSampleRateInformation;
        /// <summary>
        /// 第一数据点时标
        /// </summary>
        public DateStamp RowFirstDateStamp;
        /// <summary>
        /// 第一触发点时标
        /// </summary>
        public DateStamp RowTriggerDateStamp;
        /// <summary>
        /// 数据类型
        /// </summary>
        public DataFileType RowDataFileType;
        /// <summary>
        /// 时间戳倍率
        /// </summary>
        public TimeStampMultiply RowTimeStampMultiply;

        /// <summary>
        /// rowList添加所有行信息到列表。
        /// </summary>
         void rowListAdd()
         {
             rowList = new List<RowOperation>();
             rowList.Add(RowStationRev);
             rowList.Add(RowChannelNumType);
             if (RowAnalogChannel != null)
             {
                 rowList.AddRange(RowAnalogChannel);
             }
             if (RowDigitalChannel != null)
             {
                 rowList.AddRange(RowDigitalChannel);
             }
             rowList.Add(RowChannelFrequency);
             rowList.Add(RowSampleNum);
             rowList.Add(RowSampleRateInformation);
             rowList.Add(RowFirstDateStamp);
             rowList.Add(RowTriggerDateStamp);
             rowList.Add(RowDataFileType);
             rowList.Add(RowTimeStampMultiply);
         }
        /// <summary>
        /// 验证原始文件信息合法性并进行相应的行信息转换
        /// </summary>
        /// <param name="fileStr">配置文件内容</param>
        public void FileToRowMessage(string fileStr)
        {
            try
            {
                int index = 0;
                fileString = fileStr;
                rowCount = FileToRowString(fileStr, out rowString);

                
                RowStationRev = new StationRev(rowString[index++]);
                RowChannelNumType = new ChannelNumType(rowString[index++]);
                if (minRowCount + RowChannelNumType.ChannelCount > rowCount)
                {
                    throw new ArgumentException("FileToRowMessage行数不匹配,请检查通道编号");
                }
                if (RowChannelNumType.AnalogChannelCount !=0)
                {
                    RowAnalogChannel = new AnalogChannelInformation[RowChannelNumType.AnalogChannelCount];
                    for (int i = 0; i < RowChannelNumType.AnalogChannelCount ;i++)
                    {
                        RowAnalogChannel[i] = new AnalogChannelInformation(rowString[index++]);
                    }
                }
                if (RowChannelNumType.DigitalChannelCount != 0)
                {
                    RowDigitalChannel = new DigitalChannelInformation[RowChannelNumType.DigitalChannelCount];
                    for (int i = 0; i < RowChannelNumType.DigitalChannelCount; i++)
                    {
                        RowDigitalChannel[i] = new DigitalChannelInformation(rowString[index++]);
                    }
                }

               
                RowChannelFrequency = new ChannelFrequency(rowString[index++]);               
                RowSampleNum = new SampleNum(rowString[index++]);
                RowSampleRateInformation = new SampleRateInformation(rowString[index++]);
                RowFirstDateStamp = new DateStamp(rowString[index++]);
                RowTriggerDateStamp = new DateStamp(rowString[index++]);
                RowDataFileType = new DataFileType(rowString[index++]);
                RowTimeStampMultiply = new TimeStampMultiply(rowString[index++]);

                rowListAdd();
            }
            catch (Exception ex)
            { 
                
                throw ex;
            }

        }

        /// <summary>
        /// 将整个文件信息提取成行信息字符串数组
        /// </summary>
        /// <param name="fileStr">文件信息</param>
        /// <param name="rowString">字符串数组</param>
        public int FileToRowString(string fileStr,out string[] rowStr)
        {
            char[] charSeparators = new char[] { '\n' };
            rowStr = fileStr.Split(charSeparators,  StringSplitOptions.None);
            if (rowStr.Length < minRowCount)
            {
                throw new ArgumentOutOfRangeException("FileToRowString至少9行");
            }
            return rowStr.Length;
           
        }

        /// <summary>
        /// 生成配置文件
        /// TODO:检测当前配置信息是否合规，若合规则则生成
        /// </summary>
        /// <returns>true-转换成功</returns>
        public bool MakeConfigFile(out string[] rowStr)
        {
            try
            {
                if(!((RowChannelNumType.AnalogChannelCount != 0) 
                    &&(RowAnalogChannel.Length == RowChannelNumType.AnalogChannelCount)))
                {
                    throw new ArgumentException("模拟通道数不匹配");
                }
                if (!((RowChannelNumType.DigitalChannelCount != 0)
                    && (RowDigitalChannel.Length == RowChannelNumType.DigitalChannelCount)))
                {
                    throw new ArgumentException("数字通道数不匹配");
                }
                rowListAdd();

                rowStr = new string[RowChannelNumType.ChannelCount + minRowCount];
                for(int i = 0; i < rowStr.Length; i++)
                {
                    rowStr[i] = rowList[i].RowToString();
                }

                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取二进制数据文件每行信息字节数
        /// </summary>
        /// <param name="analogCount">模拟通道数</param>
        /// <param name="digitalCount">数字通道数</param>
        /// <returns>所需要字节数</returns>
       public int GetBinaryDataRowByteLen(int analogCount, int digitalCount)
        {
            int digital = GetUshortCount(digitalCount);
            int len = 4 + 4 + analogCount * 2 + digital * 2;
            return len;
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
    }
}
