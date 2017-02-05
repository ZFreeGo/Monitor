using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZFreeGo.FileOperation.Comtrade.ConfigContent
{
    /// <summary>
    /// 数据文件类型
    /// </summary>
    public class DataFileType : RowOperation, INotifyPropertyChanged
    {
        /// <summary>
        /// 二进制文件类型
        /// </summary>
        public const string BINARY = "BINARY";
        /// <summary>
        /// ASCII文件类型
        /// </summary>
        public const string ASCII = "ASCII";
        /// <summary>
        /// 数据文件类型 ASCII，BINARY
        /// </summary>
        string dataType;
        /// <summary>
        /// 数据文件类型 ASCII，BINARY
        /// </summary>
        public string DataType
        {
            set
            {
                var str = value.Trim().ToUpper();
                if ((str == ASCII) || (str == BINARY))
                {
                    dataType = str;
                    OnPropertyChanged(new PropertyChangedEventArgs("DataType"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("数据类型应该为ASCII或者BINARY");
                }
            }
            get
            {
                return dataType;
            }
        }


        /// <summary>
        /// 由行信息获取数据类型
        /// </summary>
        /// <returns>字符串</returns>
        public override string RowToString()
        {
            StringBuilder strBuilt = new StringBuilder(16);
            strBuilt.Append(dataType);

            strBuilt.AppendLine();
            return strBuilt.ToString();

        }

        /// <summary>
        ///获取 数据类型
        /// </summary>
        /// <param name="str">行字符串</param>
        /// <returns>false--转换成功 true--失败</returns>
        public override bool StringToRow(string str)
        {
            DataType = str;
            return true;
        }

        /// <summary>
        /// 初始化数据类型
        /// </summary>
        /// <param name="str">数据类型</param>
        public DataFileType(string str)
        {
            StringToRow(str);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {

            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
