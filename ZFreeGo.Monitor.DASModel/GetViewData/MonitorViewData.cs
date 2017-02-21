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

namespace ZFreeGo.Monitor.DASModel.GetViewData
{
    /// <summary>
    /// 监控视图数据，包含三遥，保护定值，被控对象系统参数，系统校准，事件记录
    /// </summary>
    public class MonitorViewData : IGetData
    {
        DataSet dataSetTelesignalisation;
        ICollection<Telesignalisation> telesignalisation;

        DataSet dataSetTelemetering;
        ICollection<Telemetering> telemetering;

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


        private readonly TaskScheduler syncContextTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        public MonitorViewData()
        {
            getData = new XMLOperate();
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
                telesignalisation = DataLoad<Telesignalisation>(ref CommonPath.TelesignalisationXmlPath, ref CommonPath.TelesignalisationXsdPath,
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
                telemetering = DataLoad<Telemetering>(ref CommonPath.TelemeteringXmlPath, ref CommonPath.TelemeteringXsdPath,
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
                        result = new Telesignalisation(find.InternalID, find.TelesignalisationName, find.TelesignalisationID,
                            find.IsNot, find.TelesignalisationResult, "", "");
                        break;
                    }
                }

                SOE alog;
                if (result != null)
                {
                    result.TelesignalisationResult = ele.Item2;//同步更新单点遥信信息
                    alog = new SOE(result.InternalID, result.TelesignalisationName, result.TelesignalisationState,
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
    }
       





}
   


