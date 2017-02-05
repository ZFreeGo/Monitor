using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZFreeGo.FileOperation.Comtrade.ConfigContent;
using ZFreeGo.FileOperation.Comtrade.DataContent;

namespace ZFreeGo.FileOperation.Comtrade
{
    /// <summary>
    /// Comtrade文件管理器
    /// </summary>
    public class ComtradeFileManager
    {
        /// <summary>
        /// 配置文件路径
        /// </summary>
        string configFilePath;

        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string ConfigFilePath
        {
            get
            {
                return configFilePath;
            }
        }

        /// <summary>
        /// 数据文件路径
        /// </summary>
        string dataFilePath;

        /// <summary>
        /// 数据文件路径
        /// </summary>
        public string DataFilePath
        {
            get
            {
                return dataFilePath;
            }
        }
        /// <summary>
        /// 配置文件
        /// </summary>
        public ComtradeConfigFile ConfigFile;
        /// <summary>
        /// 数据文件
        /// </summary>
        public ComtradeDataFile DataFile;

        /// <summary>
        /// Comtrade文件管理器初始化
        /// </summary>
        public ComtradeFileManager()
        {
            ConfigFile = new ComtradeConfigFile();
            DataFile = new ComtradeDataFile();
        }



        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="configPath">配置文件路径</param>
        /// <param name="dataPath">数据文件路径</param>
        public void ReadFile(string configPath, string dataPath)
        {
            try
            {
                configFilePath = configPath;
                dataFilePath = dataPath;
                ReadConfigFile(configFilePath);
                ReadDataFile(dataFilePath, ConfigFile);


            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="path">配置文件路径</param>
        public void ReadConfigFile(string path)
        {
            try
            {
                string configFile;
                using (var file = File.OpenRead(path))
                {
                    StreamReader stream = new StreamReader(file);

                    configFile = stream.ReadToEnd();

                }
                ConfigFile.FileToRowMessage(configFile);
                
           }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读取数据文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="config">配置文件</param>
        public void ReadDataFile(string path,ComtradeConfigFile config)
        {
            try
            {
                if (config.RowDataFileType.DataType == DataFileType.ASCII)
                {

                    string dataFileStr;
                    using (var file = File.OpenRead(path))
                    {
                        StreamReader stream = new StreamReader(file);
                        dataFileStr = stream.ReadToEnd();

                    }

                    DataFile.ASCIIFileToRowMessage(dataFileStr);


                    int configCount = config.RowSampleRateInformation.EndSampleCount;
                    if (DataFile.AsciiData.Count != configCount)
                    {
                        throw new ArgumentException("数据文件行数与配置文件定义数不一致");
                    }
                    
                }
                else if (config.RowDataFileType.DataType == DataFileType.BINARY)
                {
                    using (var file = File.OpenRead(path))
                    {
                        //计算所需要的存储长度
                       int len = ConfigFile.GetBinaryDataRowByteLen(ConfigFile.RowChannelNumType.AnalogChannelCount,
                           ConfigFile.RowChannelNumType.DigitalChannelCount);
                        
                        int configCount = config.RowSampleRateInformation.EndSampleCount;
                        for (int i = 0; i < configCount; i++)
                        {
                            byte[] array = new byte[len];



                            file.Read(array, 0, len);
                            
                            DataFile.BinaryData.Add(array);
                        }
                        
                        

                    }
                }
                else
                {
                    throw new ArgumentException("无法识别文件类型");
                }



            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 写配置文件
        /// </summary>
        /// <param name="configPath">配置文件路径</param>
        public void WriteConfigFile(string cfgpath)
        {
            FileStream file = new FileStream(cfgpath, FileMode.Create);
            using (StreamWriter stream = new StreamWriter(file))
            {
                
                string[] configStr;
                ConfigFile.MakeConfigFile(out configStr);
                foreach(var m in configStr)
                {
                    stream.Write(m);
                }
                stream.Flush();
                
            }

        }
        /// <summary>
        /// 写ASCII数据文件
        /// </summary>
        /// <param name="datapath">数据文件保存路径</param>
        public void WriteASCIIDataFile(string datapath)
        {
            FileStream file = new FileStream(datapath, FileMode.Create);
            using (StreamWriter stream = new StreamWriter(file))
            {
                foreach (var m in DataFile.AsciiData)
                {
                    stream.Write(m);
                }
                stream.Flush();

            }

        }
        /// <summary>
        /// 写BINARY数据文件
        /// </summary>
        /// <param name="datapath">数据文件保存路径</param>
        public void WriteBINARYDataFile(string datapath)
        {
            using (var file = File.Create(datapath))
             {
                foreach(var m in DataFile.BinaryData)
                {
                    file.Write(m, 0, m.Length);
                }
                file.Flush();
             }
        }

        /// <summary>
        /// 写配置文件
        /// </summary>
        /// <param name="configPath">配置文件路径</param>
        /// <param name="dataPath">数据文件路径</param>
        public void WriteFile(string configPath, string dataPath)
        {
            try
            {
                WriteConfigFile(configPath);
                if (ConfigFile.RowDataFileType.DataType == DataFileType.BINARY)
                {
                    WriteBINARYDataFile(dataPath);

                }
                else if (ConfigFile.RowDataFileType.DataType == DataFileType.ASCII)
                {
                    WriteASCIIDataFile(dataPath);
                }
                else
                {
                    throw new Exception("未识别的数据类型");
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 检测当前存储文件格式
        /// </summary>
        /// <returns>若检测通过返回true</returns>
        public bool CheckFileFormat()
        {
            return true;
        }
        

    }
}
