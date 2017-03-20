using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ZFreeGo.Monitor.DASModel.Helper;
using ZFreeGo.Monitor.DASModel.DataItemSet;
using System.Threading.Tasks;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.FileSever;


namespace ZFreeGo.Monitor.DASModel.GetViewData
{
    /// <summary>
    /// 监控视图数据，包含三遥，保护定值，被控对象系统参数，系统校准，事件记录
    /// </summary>
    public class MonitorViewData : IGetData
    {
        DataSet dataSetTelesignalisation;
        ObservableCollection<Telesignalisation> telesignalisation;

        DataSet dataSetTelemetering;
        ObservableCollection<Telemetering> telemetering;

        DataSet dataSetSystemParameter;
        ICollection<SystemParameter> systemParameter;

        private DataSet dataSetTelecontrol;
        private ICollection<Telecontrol> telecontrol;

        DataSet dataSetProtectSetPoint;
        ICollection<ProtectSetPoint> protectSetPoint;

        DataSet dataSetSystemCalibration;
        ICollection<SystemCalibration> systemCalibration;

        DataSet dataSetSOE;
        ICollection<SOE> soeLog;

        XMLOperate getData;


        private SQLliteDatabase dataBase;

        /// <summary>
        /// 电能表格数据
        /// </summary>
        ObservableCollection<ElectricPulse> electricPulse;



        private ObservableCollection<FileAttributeItem> directoryList;

        private readonly TaskScheduler syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        public MonitorViewData()
        {
            getData = new XMLOperate();
            dataBase = new SQLliteDatabase(CommonPath.DataBase);
        }

        //private void TelesignalisationExport_Click()
        //{
        //    DataExport<Telesignalisation>(dataSetTelesignalisation, DataTypeEnum.Telesignalisation,
        //      telesignalisation, CommonPath.TelesignalisationXmlPath);

        //}
        //private void TelesignalisationLoad_Click()
        //{
        //    try
        //    {
        //        telesignalisation = DataLoad<Telesignalisation>(ref pathxmlTelesignalisation, ref pathxsdTelesignalisation,
        //         ref dataSetTelesignalisation, DataTypeEnum.Telesignalisation);


        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        /// <summary>
        /// 打开XML配置文件
        /// </summary>
        /// <param name="path">路劲</param>
        /// <param name="style">类型</param>
        /// <returns>是否成功打开</returns>
        private bool OpenXmlFile(ref string path, string style)
        {
            return true;

            //Stream myStream;
            //var openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.Filter = style + "files   (*." + style + ")|*." + style;
            //openFileDialog1.FilterIndex = 2;
            //openFileDialog1.RestoreDirectory = true;

            //if (openFileDialog1.ShowDialog() == true)
            //{
            //    if ((myStream = openFileDialog1.OpenFile()) != null)
            //    {
            //        using (myStream)
            //        {
            //            path = openFileDialog1.FileName;
            //            return true;
            //        }

            //    }
            //}
            //return false;

        }
        /// <summary>
        /// 存储XML文件
        /// </summary>
        /// <param name="path">存储路径</param>
        /// <returns>是否成功存储</returns>
        private bool SaveXmlFile(ref string path)
        {
            return true;
            //Stream myStream;
            //var saveFileDialog1 = new SaveFileDialog();

            //saveFileDialog1.Filter = "xml files   (*.xml)|*.xml";
            //saveFileDialog1.FilterIndex = 2;
            //saveFileDialog1.RestoreDirectory = true;

            //if (saveFileDialog1.ShowDialog() == true)
            //{
            //    if ((myStream = saveFileDialog1.OpenFile()) != null)
            //    {
            //        using (myStream)
            //        {
            //            path = saveFileDialog1.FileName;
            //            return true;
            //        }

            //    }
            //}
            //return false;

        }
        /// <summary>
        /// 检测文件是否存在，若不存在选择一个文件。
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="style"></param>
        /// <returns>文件是否存在</returns>
        private bool IsExists(ref string path, string style)
        {
            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                if (OpenXmlFile(ref path, style))
                {
                    return true;
                }
                else
                {
                    throw new Exception("未选择文件");
                }
            }


        }
        /// <summary>
        /// 将数据导出
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="ds">数据表格</param>
        /// <param name="en">数据类型</param>
        /// <param name="observa">数据列表</param>
        /// <param name="path">路径</param>
        private void DataExport<T>(DataSet ds, DataTypeEnum en, ICollection<T> observa, string path)
        {
            try
            {
                getData.UpdateTable(ds, en, observa);
                ds.WriteXml(path);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "DataExport");
            }
        }
        /// <summary>
        /// 载入数据列表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="pathxml">xml路径</param>
        /// <param name="pathxsd">xsd路径</param>
        /// <param name="ds">数据表</param>
        /// <param name="en">数据类型</param>
        /// <param name="dg">表格</param>
        /// <returns>返回可观察的列表</returns>
        private ICollection<T> DataLoad<T>(ref string pathxml, ref string pathxsd, ref DataSet ds, DataTypeEnum en)
        {
            try
            {
                if (IsExists(ref pathxml, "xml") && IsExists(ref pathxsd, "xsd"))
                {
                    ds = getData.ReadXml(pathxml, pathxsd);
                    var icollect = getData.GetDataCollect(ds, en) as ICollection<T>;
                    if (icollect != null)
                    {
                        return icollect;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region 遥信数据获取
        /// <summary>
        /// 获取遥信数据
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Telesignalisation> GetTelesignalisationList()
        {
            if (telesignalisation == null)
            {
                ObservableCollection<Telesignalisation> list = new ObservableCollection<Telesignalisation>();
                telesignalisation = (ObservableCollection<Telesignalisation>)DataLoad<Telesignalisation>(ref CommonPath.TelesignalisationXmlPath, ref CommonPath.TelesignalisationXsdPath,
                       ref dataSetTelesignalisation, DataTypeEnum.Telesignalisation);
                list = (ObservableCollection<Telesignalisation>)telesignalisation;
                return list;
            }
            else
            {
                return (ObservableCollection<Telesignalisation>)telesignalisation;
            }

        }
        #endregion

        #region 遥信数据 数据库操作

        /// <summary>
        /// 创建TelesignalisationTable
        /// </summary>
        public void CrateTelesignalisationTable()
        {
            string sql =
            "CREATE TABLE Telesignalisation(InternalID int,TelesignalisationName Text,TelesignalisationID int,IsNot BOOLEAN, TelesignalisationResult int, Time Datetime, TelesignalisationState string,  Comment Text,StateA Text, StateB Text )";
            dataBase.CreateTale(sql);
        }

        /// <summary>
        /// 清空历史数据，将数据插入表格
        /// </summary>       
        public void InsertTelesignalisation()
        {
            var collect = telesignalisation;
            var listStr = new List<String>();
            foreach (var m in collect)
            {
                string sql = string.Format("INSERT INTO  Telesignalisation VALUES({0},\'{1}\',{2},{3},{4},\'{5}\',\'{6}\',\'{7}\',\'{8}\',\'{9}\')",
                    m.InternalID, m.TelesignalisationName, m.TelesignalisationID,(m.IsNot?1:0), m.TelesignalisationResult, m.Date, 
                   m.TelesignalisationState, m.Comment, m.StateA, m.StateB);
                listStr.Add(sql);
            }
            if (listStr.Count > 0)
            {
                string sqlClear = "delete from  Telesignalisation";
                dataBase.InsertTable(listStr, sqlClear);
                
            }
        }

        /// <summary>
        /// 读取遥信
        /// </summary>
        /// <param name="flag">true--重新更新, false--若当前已存在则直接使用</param>
        /// <returns>遥信合集</returns>       
        public ObservableCollection<Telesignalisation> ReadTelesignalisation(bool flag)
        {
            try
            {
                if (telesignalisation == null || flag)
                {
                    telesignalisation = new ObservableCollection<Telesignalisation>();
                    string sql = "SELECT * from Telesignalisation";
                    dataBase.ReadTable(sql, GetTelesignalisation);
                    return telesignalisation;
                }
                else
                {
                    return telesignalisation;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool GetTelesignalisation(System.Data.SQLite.SQLiteDataReader reader)
        {
            telesignalisation.Add(new Telesignalisation(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2),
                reader.GetBoolean(3), reader.GetInt32(4), reader.GetDateTime(5).ToString(), reader.GetString(7), 
                reader.GetString(8), reader.GetString(9)));

            return true;
        }


        #endregion

        #region 遥测数据获取
        /// <summary>
        /// 获取遥测数据
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Telemetering> GetTelemeteringList()
        {
            if (telemetering == null)
            {
                ObservableCollection<Telemetering> list = new ObservableCollection<Telemetering>();
                telemetering = (ObservableCollection<Telemetering>)DataLoad<Telemetering>(ref CommonPath.TelemeteringXmlPath, ref CommonPath.TelemeteringXsdPath,
                       ref dataSetTelemetering, DataTypeEnum.Telemetering);

                list = (ObservableCollection<Telemetering>)telemetering;
                return list;
            }
            else
            {
                return (ObservableCollection<Telemetering>)telemetering;
            }

        }
        #endregion

        #region 遥测数据 数据库操作

        /// <summary>
        /// 创建TelemeteringTable
        /// </summary>
        public void CrateTelemeteringTable()
        {
            string sql =
            "CREATE TABLE Telemetering(InternalID int, TelemeteringName Text,TelemeteringID int, CalibrationCoefficient double, TelemeteringValue double, Unit Text, Mark Text, Comment Text)";
            dataBase.CreateTale(sql);
        }

        /// <summary>
        /// 清空历史数据，将数据插入表格
        /// </summary>       
        public void InsertTelemetering()
        {
            var collect = telemetering;
            var listStr = new List<String>();
            foreach (var m in collect)
            {
                string sql = string.Format("INSERT INTO  Telemetering VALUES({0},\'{1}\',{2},{3},{4},\'{5}\',\'{6}\',\'{7}\')",
                   m.InternalID, m.TelemeteringName, m.TelemeteringID, m.CalibrationCoefficient,
                   m.TelemeteringValue, m.Unit, m.Mark, m.Comment);
                listStr.Add(sql);
            }
            if (listStr.Count > 0)
            {
                string sqlClear = "delete from  Telemetering";
                dataBase.InsertTable(listStr, sqlClear);
            }
        }

        /// <summary>
        /// 读取电能数据表格
        /// </summary>
        /// <param name="flag">true--重新更新, false--若当前已存在则直接使用</param>
        /// <returns>电能合集</returns>       
        public ObservableCollection<Telemetering> ReadTelemetering(bool flag)
        {
            try
            {
                if (telemetering == null || flag)
                {
                    telemetering = new ObservableCollection<Telemetering>();
                    string sql = "SELECT * from Telemetering";
                    dataBase.ReadTable(sql, GetTelemetering);
                    return telemetering;
                }
                else
                {
                    return telemetering;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private bool GetTelemetering(System.Data.SQLite.SQLiteDataReader reader)
        {
            reader.GetInt32(0);
            reader.GetString(1);
            reader.GetInt32(2);
            reader.GetDouble(3);
            reader.GetDouble(4);
            reader.GetString(5);
            var name = reader.GetDataTypeName(6);
            reader.GetString(6);
           
            reader.GetString(7);


            telemetering.Add(new Telemetering(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2),
                reader.GetDouble(3), reader.GetDouble(4), reader.GetString(5), reader.GetString(6), reader.GetString(7)));

            return true;
        }


        #endregion

        #region 系统参数数据获取
        /// <summary>
        /// 获取系统参数数据
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SystemParameter> GetSystemParameterList()
        {
            if (systemParameter == null)
            {
                ObservableCollection<SystemParameter> list = new ObservableCollection<SystemParameter>();
                systemParameter = DataLoad<SystemParameter>(ref CommonPath.SystemParameterXmlPath, ref CommonPath.SystemParameterXsdPath,
                       ref dataSetSystemParameter, DataTypeEnum.SystemParameter);
                list = (ObservableCollection<SystemParameter>)systemParameter;
                return list;
            }
            else
            {
                return (ObservableCollection<SystemParameter>)systemParameter;
            }
        }
        #endregion

        #region 遥控参数数据获取
        /// <summary>
        /// 获取系统参数数据
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Telecontrol> GetTelecontrolList()
        {
            if (telecontrol == null)
            {
                ObservableCollection<Telecontrol> list = new ObservableCollection<Telecontrol>();
                telecontrol = DataLoad<Telecontrol>(ref CommonPath.TelecontrolXmlPath, ref CommonPath.TelecontrolXsdPath,
                       ref dataSetTelecontrol, DataTypeEnum.Telecontrol);
                list = (ObservableCollection<Telecontrol>)telecontrol;

                return list;
            }
            else
            {
                return (ObservableCollection<Telecontrol>)telecontrol;
            }
        }
        #endregion


        #region 保护定值数据获取
        /// <summary>
        /// 获取保护定值参数
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<ProtectSetPoint> GetProtectSetPointList()
        {
            if (protectSetPoint == null)
            {
                ObservableCollection<ProtectSetPoint> list = new ObservableCollection<ProtectSetPoint>();
                protectSetPoint = DataLoad<ProtectSetPoint>(ref CommonPath.ProtectSetPointXmlPath, ref CommonPath.ProtectSetPointXsdPath,
                       ref dataSetProtectSetPoint, DataTypeEnum.ProtectSetPoint);
                list = (ObservableCollection<ProtectSetPoint>)protectSetPoint;

                return list;
            }
            else
            {
                return (ObservableCollection<ProtectSetPoint>)protectSetPoint;
            }
        }
        #endregion

        #region 系统校准数据获取
        /// <summary>
        /// 获取系统校准参数
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SystemCalibration> GetSystemCalibrationList()
        {
            if (systemCalibration == null)
            {
                ObservableCollection<SystemCalibration> list = new ObservableCollection<SystemCalibration>();
                systemCalibration = DataLoad<SystemCalibration>(ref CommonPath.SystemCalibrationXmlPath, ref CommonPath.SystemCalibrationXsdPath,
                       ref dataSetSystemCalibration, DataTypeEnum.SystemCalibration);
                list = (ObservableCollection<SystemCalibration>)systemCalibration;

                return list;
            }
            else
            {
                return (ObservableCollection<SystemCalibration>)systemCalibration;
            }
        }
        #endregion



        #region 事件记录数据获取
        /// <summary>
        /// 获取事件记录参数
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<SOE> GetSOEList()
        {
            if (soeLog == null)
            {
                ObservableCollection<SOE> list = new ObservableCollection<SOE>();
                soeLog = DataLoad<SOE>(ref CommonPath.SOEXmlPath, ref CommonPath.SOEXsdPath,
                       ref dataSetSOE, DataTypeEnum.SOE);
                if (soeLog == null)
                {
                    soeLog = list;
                }
                list = (ObservableCollection<SOE>)soeLog;

                return list;
            }
            else
            {
                return (ObservableCollection<SOE>)soeLog;
            }
        }

        #endregion

        #region EletricPulse数据库操作

        /// <summary>
        /// 创建EletricPulseTable
        /// </summary>
        public void CrateEletricPulseTable()
        {
            string sql = 
                "CREATE TABLE EletricPulse(ID int, Name Text,Value Double,Unit Text, TimeStamp Text, Comment Text)";        
            dataBase.CreateTale(sql);
        }

        /// <summary>
        /// 清空历史数据，将数据插入表格
        /// </summary>
        /// <param name="collect"></param>
        public void InsertEletricPulse()
        {
            var collect = electricPulse;
            var listStr = new List<String>();
            foreach (var m in collect)
            {
                string sql = string.Format("INSERT INTO  EletricPulse VALUES({0},\'{1}\',{2},\'{3}\',\'{4}\',\'{5}\')", 
                    m.ID, m.Name, m.Value, m.Unit, m.TimeStamp, m.Comment);
                listStr.Add(sql);
            }           
            if (listStr.Count > 0)
            {
                string sqlClear = "delete from  EletricPulse";
                dataBase.InsertTable(listStr, sqlClear);
            }            
        }

        /// <summary>
        /// 读取电能数据表格
        /// </summary>
        /// <param name="flag">true--重新更新, false--若当前已存在则直接使用</param>
        /// <returns>电能合集</returns>       
        public ObservableCollection<ElectricPulse>  ReadEletricPulse(bool flag)
        {            
            if (electricPulse == null || flag)
            {
                electricPulse = new ObservableCollection<ElectricPulse>();
                string sql = "SELECT * from EletricPulse";
                dataBase.ReadTable(sql, GetEletricPulse);
                return electricPulse;
            }
            else
            {
                return electricPulse;
            }           
        }
        private bool GetEletricPulse(System.Data.SQLite.SQLiteDataReader reader)
        {
            //var a1 = reader.GetInt32(0);
            
            var a2 = reader.GetSchemaTable();

            
            //
            //var t = a2.Columns[0];
            //var a3 = reader.GetDouble(2);
           
            var a4 = reader.GetString(3);
            var a5 = reader.GetString(4);
            var a6 = reader.GetString(5);
            electricPulse.Add(new ElectricPulse((UInt32)reader.GetInt32(0), reader.GetString(1), reader.GetDouble(2),
                reader.GetString(3), reader.GetString(4), reader.GetString(5)));
            
            return true;
        }


        #endregion
        #region 遥信
        /// <summary>
        /// 更新遥信信息--为什么此处任务调度
        /// </summary>
        /// <param name="e">信息事件参数</param>
        public void UpdateStatusEvent( TransmissionProtocols.MonitorProcessInformation.StatusEventArgs<List<Tuple<uint, byte>>> e)
        {
            try
            {

                var collect = GetTelesignalisationList();
                foreach (var ele in e.Message)
                {
                    for (int k = 0; k < collect.Count; k++)
                    {
                        var t = collect[k];
                        if ((t.InternalID + Telesignalisation.BasicAddress - 1) == ele.Item1)
                        {
                            t.Date = DateTime.Now.ToLongTimeString();
                            t.TelesignalisationResult = (byte)ele.Item2;
                            if (e.ID == ZFreeGo.TransmissionProtocols.BasicElement.TypeIdentification.M_DP_NA_1)
                            {
                                t.IsSingle = true;
                            }
                            else
                            {
                                t.IsSingle = false;
                            }
                            
                        }

                    }
                }
            }
            catch(Exception ex)
            {

            }


        }
        #endregion

        #region 遥测
        /// <summary>
        /// 更新遥测信息
        /// </summary>
        /// <param name="e">事件</param>
        public void UpdateTelemeteringEvent(TransmissionProtocols.
            MonitorProcessInformation.StatusEventArgs<List<Tuple<uint, float,
            ZFreeGo.TransmissionProtocols.BasicElement.QualityDescription>>> e)
        {
          
            var m = GetTelemeteringList();
            var list = e.Message;
            foreach (var ele in list)
            {
                for (int k = 0; k < m.Count; k++)
                {
                    var t = m[k];
                    if ((t.InternalID + Telemetering.BasicAddress - 1) == ele.Item1)
                    {
                        t.TelemeteringValue = (float)ele.Item2;
                        t.TelemeteringID = (int)(t.InternalID + Telemetering.BasicAddress - 1);

                    }

                }
            }


        }
        #endregion

        #region SOE
        /// <summary>
        /// 更新SOE信息
        /// </summary>
        /// <param name="e">信息事件</param>
        public void UpdateSOEEvent(TransmissionProtocols.MonitorProcessInformation.
            StatusEventArgs<List<Tuple<uint, byte, ZFreeGo.TransmissionProtocols.BasicElement.CP56Time2a>>> e)
        {


            var list = GetTelesignalisationList();
            var collect = GetSOEList();

            foreach (var ele in e.Message)
            {

                var time = ele.Item3;
                //通过遥信ID，查找所需要的元素
                Telesignalisation result = null;
                foreach (var find in list)
                {
                    if (find.InternalID == (int)ele.Item1)
                    {
                        result = find;
                        //result = new Telesignalisation(find.InternalID, find.TelesignalisationName, find.TelesignalisationID,
                        //    find.IsNot, find.TelesignalisationResult, "", "");
                        break;
                    }
                }

                SOE alog;
                if (result != null)
                {
                    result.TelesignalisationResult = ele.Item2;//同步更新单点遥信信息
                    alog = new SOE(result.InternalID, "SOE", result.TelesignalisationName + ":" + result.TelesignalisationState,
                   time.ToString(), "", time.Milliseconds.ToString());
                }
                else
                {
                    alog = new SOE((int)ele.Item1, "ID未定义", ele.Item2.ToString(),
                   time.ToString(), "", time.Milliseconds.ToString());
                }


                //任务调度器，用于快线程更新UI
                Task.Factory.StartNew(() => collect.Add(alog),
                    new System.Threading.CancellationTokenSource().Token, TaskCreationOptions.None, syncContextTaskScheduler).Wait();
            }


        }
        #endregion
        #region 故障信息
        public void UpdateFaultMessageEvent(TransmissionProtocols.MonitorProcessInformation.
            EventLogEventArgs<Tuple<uint, byte, CP56Time2a>, Tuple<uint, float>> e)
        {
            try
            {
                var collect = GetSOEList();
                StringBuilder strBuild = new StringBuilder();

                strBuild.AppendFormat("ID:{0} 故障遥信数目:{1}.", e.StatusID.ToString(), e.StatusMessage.Count);
                foreach (var m in e.StatusMessage)
                {
                    strBuild.AppendFormat("点号:{0:X2}  码值{1:X2}  时间{2}\n", m.Item1, m.Item2, m.Item3.ToString());
                }
                strBuild.AppendFormat("ID:{0} 故障遥测数目:{1}.", e.MeteringID.ToString(), e.MeteringMessage.Count);
                foreach (var m in e.MeteringMessage)
                {
                    strBuild.AppendFormat("点号:{0:X2}  {1:X2}\n", m.Item1, m.Item2);
                }

                var alog = new SOE(0, "故障事件", strBuild.ToString(),
                   DateTime.Now.ToLongTimeString(), "", DateTime.Now.Millisecond.ToString());
                 //任务调度器，用于快线程更新UI
                Task.Factory.StartNew(() => collect.Add(alog),
                    new System.Threading.CancellationTokenSource().Token, TaskCreationOptions.None, syncContextTaskScheduler).Wait();
            }
            catch(Exception ex)
            {

            }
        }



        #endregion

        #region 电能脉冲
        /// <summary>
        /// 更新带有时标的电能脉冲
        /// </summary>
        /// <param name="e"></param>
        public void UpdateEletricMessageEvent(TransmissionProtocols.MonitorProcessInformation.
            StatusEventArgs<List<Tuple<uint, float, QualityDescription, CP56Time2a>>> e)
        {
            var m = ReadEletricPulse(false);
            var list = e.Message;
            foreach (var ele in list)
            {
                for (int k = 0; k < m.Count; k++)
                {
                    var t = m[k];
                    if (t.ID == ele.Item1)
                    {
                        t.Value = ele.Item2;
                        t.TimeStamp = ele.Item4.ToString();
                    }

                }
            }
        }
        /// <summary>
        /// 更新电能脉冲
        /// </summary>
        /// <param name="e"></param>
        public void UpdateEletricMessageEvent(TransmissionProtocols.MonitorProcessInformation.
           StatusEventArgs<List<Tuple<uint, float, QualityDescription>>> e)
        {
            var m = ReadEletricPulse(false);
            var list = e.Message;
            foreach (var ele in list)
            {
                for (int k = 0; k < m.Count; k++)
                {
                    var t = m[k];
                    if (t.ID == ele.Item1)
                    {
                        t.Value = ele.Item2;                        
                    }

                }
            }
        }

        #endregion


        #region 文件操作
        /// <summary>
        /// 获取目录列表信息
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<FileAttributeItem> GetDirectoryList()
        {
            if (directoryList == null)
            {
                directoryList = new ObservableCollection<FileAttributeItem>();
            }
            return directoryList;
        }

        /// <summary>
        /// 更新目录
        /// </summary>
        /// <param name="attributeList"></param>
        public void UpdateDirectoryList(List<FileAttribute> attributeList)
        {
            //先提取
            var temp = new ObservableCollection<FileAttributeItem>();
            foreach(var m in attributeList)
            {
                temp.Add(new FileAttributeItem(m));              
            }
            //转显示
            directoryList = temp;
        }
        #endregion


        #region 更新校准信息

        /// <summary>
        ///更新召唤系数
        /// </summary>
        /// <param name="frame">系数帧</param>
        public void UpdateCalibrationFact(UInt32[] frame)
        {
            try
            {
                var observale = (ObservableCollection<SystemCalibration>)systemCalibration;
                if (frame.Length != observale.Count)
                {
                    throw new Exception("接收长度与列表长度不一致");
                }
                int len = Math.Min(frame.Length, observale.Count);

                for (int i = 0; i < len; i++)
                {
                    for (int j = 0; j < observale.Count; j++)
                    {
                        if (observale[j].InternalID == (i + 1))
                        {
                            observale[j].CallCoefficient = frame[i];
                            //召唤系数与下载系数相同
                            observale[j].DownloadCoefficient = observale[j].CallCoefficient;

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
                
            }
        }

        /// <summary>
        /// 更新校准数据
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="updateIndex">更新索引</param>
        public void UpdateCalbrationData(List<Tuple<uint, float, QualityDescription>> list,ref int updateIndex)
        {

            var m = (ObservableCollection<SystemCalibration>)systemCalibration;
            foreach (var ele in list)
            {
                for (int k = 0; k < m.Count; k++)
                {
                    var t = m[k];
                    if ((t.InternalID + Telemetering.BasicAddress - 1) == ele.Item1)
                    {
                        t.UpdateData(updateIndex, (float)(ele.Item2));
                    }
                }
            }

            if (updateIndex < 10)
            {
                updateIndex++;
            }
            else
            {
                updateIndex = 0;
            }
        }

        #endregion


    }
       





}
   


