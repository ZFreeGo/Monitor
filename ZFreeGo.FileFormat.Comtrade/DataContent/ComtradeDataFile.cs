using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.FileOperation.Comtrade.DataContent
{
    /// <summary>
    ///Comtrade 数据文件
    /// </summary>
    public class ComtradeDataFile
    {
        /// <summary>
        /// 数据文件内容
        /// </summary>
        List<byte[]> binaryData;

        /// <summary>
        /// 数据文件内容
        /// </summary>
        public List<byte[]> BinaryData
        {
            get
            {
                return binaryData;
            }
            set
            {
                binaryData = value;
            }
        }

        /// <summary>
        /// 数据文件行数
        /// </summary>
        int rowCount;



        /// <summary>
        /// ASCII文件内容
        /// </summary>
        public List<string> AsciiData;

        /// <summary>
        /// Comtrade 数据文件初始化
        /// </summary>
        public ComtradeDataFile()
        {
            binaryData = new List<byte[]>();
            AsciiData = new List<string>();
        }

        /// <summary>
        /// ASCII数据文件信息转换成行信息
        /// </summary>
        /// <param name="fileStr">文件内容</param>
        public void ASCIIFileToRowMessage(string fileStr)
        {
            try
            {
                string[] rowString;
                fileStr.Trim();
                rowCount = FileToRowString(fileStr, out rowString);
                
                AsciiData.AddRange(rowString);
                //删除其中的空白字符
                for(int i = AsciiData.Count-1; i > 0; i--)
                {
                    if (AsciiData[i] == "")
                    {
                        AsciiData.Remove(AsciiData[i]);
                        i = AsciiData.Count - 1;
                    }
                   
                }

            }
            catch(Exception ex)
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
           
            return rowStr.Length;
           
        }
    }
}
