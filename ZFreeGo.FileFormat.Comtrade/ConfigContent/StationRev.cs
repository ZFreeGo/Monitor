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
    /// 厂站名标识和版本号
    /// </summary>
    public class StationRev: RowOperation, INotifyPropertyChanged
    
    {
        /// <summary>
        /// 厂站名，可选，0-64字符
        /// </summary>
        string stationName;
        /// <summary>
        /// 厂站名，可选，0-64字符
        /// </summary>
        public string StationName
        {
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 0, 64))
                {
                    stationName = str;
                    OnPropertyChanged(new PropertyChangedEventArgs("StationName"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("厂站名 字符串长度应不大于64");
                }
            }
            get
            {
                if (StringOperation.CheckStringLength(ref stationName, 0, 64))
                {
                    return stationName;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("StationName,字符[0,64]");
                }
            }
        }

        /// <summary>
        /// 记录装置的标识标号，可选，0-64字符
        /// </summary>
        string recordDeviceID;
        /// <summary>
        /// 记录装置的标识标号，可选，0-64字符
        /// </summary>
        public string RecordDeviceID
        {
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 0, 64))
                {
                    
                    recordDeviceID = str;
                    OnPropertyChanged(new PropertyChangedEventArgs("RecordDeviceID"));
                }
                else
                {
                    throw new ArgumentOutOfRangeException("记录装置的标识 标号字符串长度应不大于64");
                }
            }
            get
            {
                if (StringOperation.CheckStringLength(ref recordDeviceID, 0, 64))
                {
                    return recordDeviceID;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(" RecordDeviceID，字符[0,64]");
                }
               
            }
        }

        /// <summary>
        /// 版本标准年号 4个字符， 缺省或空解释为1991
        /// </summary>
        string versionYear;
        /// <summary>
        /// 版本标准年号 4个字符， 缺省或空解释为1991
        /// </summary>
        public string VersionYear
        {
            set
            {
                var str = value;
                if (StringOperation.CheckStringLength(ref str, 0, 4))
                {
                    int year = 0;
                    if (int.TryParse(str,out year)) //检测能否顺利转换成年份
                    {
                        versionYear = str.Trim();
                        OnPropertyChanged(new PropertyChangedEventArgs("VersionYear"));
                    }
                    else
                    {
                        if (str.Length == 0) //是否缺省
                        {
                            versionYear = str;
                            OnPropertyChanged(new PropertyChangedEventArgs("VersionYear"));
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("版本标准年号 字符串应为0-9999范围之内的数字字符");
                        }
                    }
                    
                }
                else
                {
                    throw new ArgumentOutOfRangeException("版本标准年号 字符串长度应不大于4");
                }
            }
            get
            {
                return versionYear;
            }
        }

        /// <summary>
        /// 初始化 厂站名标识和版本号
        /// </summary>
        /// <param name="stationName">站名称</param>
        /// <param name="recordDeviceID">设备标识</param>
        /// <param name="versionYear">版本号</param>
        public StationRev(string stationName, string recordDeviceID, string versionYear)
        {
            SetStationRev(stationName, recordDeviceID,  versionYear);
        }
        /// <summary>
        /// 设置 厂站名标识和版本号
        /// </summary>
        /// <param name="stationName">站名称</param>
        /// <param name="recordDeviceID">设备标识</param>
        /// <param name="versionYear">版本号</param>
        public void SetStationRev(string stationName, string recordDeviceID, string versionYear)
        {
            StationName = stationName;
            RecordDeviceID = recordDeviceID;
            VersionYear = versionYear;

        }
        /// <summary>
        /// 使用row字符串初始化站信息
        /// </summary>
        /// <param name="rowStr">字符串</param>
        public StationRev(string rowStr)
        {
            StringToRow(rowStr);
        }


        /// <summary>
        /// 生成 厂站名标识和版本号对应的行信息字符串
        /// </summary>
        /// <returns>生成字符串</returns>
        public override string RowToString()
        {
            StringBuilder strBuilt = new StringBuilder(64);
            strBuilt.Append(StationName);
            strBuilt.Append(",");
            strBuilt.Append(RecordDeviceID);
            strBuilt.Append(",");
            strBuilt.Append(VersionYear);
            strBuilt.AppendLine();
            return strBuilt.ToString();

        }

        /// <summary>
        ///获取厂站信息 将行字符串转化为对应行信息。
        /// </summary>
        /// <param name="str">行字符串</param>
        /// <returns>false--转换成功 true--失败</returns>
        public override bool StringToRow(string str)
        {
            char[] charSeparators = new char[] {','};
            var result = str.Split(charSeparators, 3, StringSplitOptions.None);
            if (result.Length !=3)
            {
                throw new ArgumentOutOfRangeException("数据分隔符应该限定为3个，必须有俩个'，'分隔符");
            }
            StationName = result[0];
            RecordDeviceID = result[1];
            VersionYear = result[2];
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
