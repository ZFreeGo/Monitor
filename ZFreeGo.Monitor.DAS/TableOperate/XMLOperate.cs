using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ZFreeGo.Monitor.AutoStudio.ElementParam;

namespace ZFreeGo.Monitor.AutoStudio
{
    public class XMLOperate
    {

        public DataSet ReadXml(string xmlPath, string xsdPath)

        {
            try
            {
                if (!File.Exists(xmlPath))
                {
                    throw new Exception("路径:" + xmlPath + ", 所指向的文件不存在，请重新选择");
                }
                if (!File.Exists(xsdPath))
                {
                    throw new Exception("路径:" + xsdPath + ", 所指向的文件不存在，请重新选择");
                }

                var ds = new DataSet();
                ds.ReadXmlSchema(xsdPath);
                ds.ReadXml(xmlPath);
                
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }

        /// <summary>
        /// 获取数据集合
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="typeEnum"></param>
        /// <returns></returns>
        public object GetDataCollect(DataSet ds, DataTypeEnum typeEnum )
            
        {
            try
            {
                
                
                switch(typeEnum)
                {
                    case DataTypeEnum.Telesignalisation:
                        {
                            var dataCollect = new ObservableCollection<Telesignalisation>();;
                            foreach (DataRow productRow in ds.Tables["Telesignalisation"].Rows)
                            {
                                dataCollect.Add(new Telesignalisation(
                                    (int)productRow["InternalID"],
                                    (string)productRow["TelesignalisationName"],
                                    (int)productRow["TelesignalisationID"],
                                    (string)productRow["IsNot"],
                                    (string)productRow["TelesignalisationValue"],
                                    (string)productRow["Date"],
                                    (string)productRow["Comment"]));

                            }
                            return dataCollect;                       
                            
                        }
                    case DataTypeEnum.Telemetering:
                        {
                            var dataCollect = new ObservableCollection<Telemetering>(); ;
                            foreach (DataRow productRow in ds.Tables["Telemetering"].Rows)
                            {
                                dataCollect.Add(
                                    new Telemetering(
                                (int) productRow["InternalID"], 
                                (string)productRow["TelemeteringName"], 
                                (int)productRow["TelemeteringID"], 
                                (double)productRow["CalibrationCoefficient"],
                                (double)productRow["TelemeteringValue"], 
                                (string)productRow["Unit"], 
                                (string)productRow["Mark"], 
                                (string)productRow["Comment"]
                                    ));

                            }
                            return dataCollect;                            
                        }
                    case DataTypeEnum.Telecontrol:
                        {
                            var dataCollect = new ObservableCollection<Telecontrol>(); ;
                            foreach (DataRow productRow in ds.Tables["Telecontrol"].Rows)
                            {
                                dataCollect.Add(
                                    new Telecontrol(
                                (int)productRow["TelecontrolID"],
                                (string)productRow["TelecontrolComment"],
                                (string)productRow["TelecontrolState"],
                                (string)productRow["TelecontrolOperate"],
                                (string)productRow["TelecontrolOperateState"],
                                (string)productRow["DeviceActionTime"] ));

                            }
                            return dataCollect;
                           
                        }
                    case DataTypeEnum.SystemParameter:
                        {
                            var dataCollect = new ObservableCollection<SystemParameter>(); ;
                            foreach (DataRow productRow in ds.Tables["SystemParameter"].Rows)
                            {
                                dataCollect.Add(
                                    new SystemParameter(
                                (int)productRow["InternalID"],
                                (string)productRow["paramName"],
                                (string)productRow["defaultValue"],
                                (string)productRow["description"]));
                            }
                            return dataCollect;                            
                        }
                    case DataTypeEnum.SystemCalibration:
                        {
                            var dataCollect = new ObservableCollection<SystemCalibration>(); ;
                            foreach (DataRow productRow in ds.Tables["SystemCalibration"].Rows)
                            {
                                dataCollect.Add(
                                    new SystemCalibration(
                                (int)productRow["InternalID"],
                                (int)productRow["EndPoint"],                                
                                (string)productRow["ParamName"],
                                (double)productRow["ParamValue"],
                                (double)productRow["StandardValue"],
                                (double)productRow["CallCoefficient"],
                                (double)productRow["DownloadCoefficient"],
                                (double)productRow["AverageValue"],
                                (double)productRow["Data1"],
                                (double)productRow["Data2"],
                                (double)productRow["Data3"],
                                (double)productRow["Data4"],
                                (double)productRow["Data5"],
                                (double)productRow["Data6"],
                                (double)productRow["Data7"],
                                (double)productRow["Data8"],
                                (double)productRow["Data9"],
                                (double)productRow["Data10"],
                                (string)productRow["Comment"]));
                            }
                            return dataCollect;                            
                        }
                    case DataTypeEnum.EventLog:
                        {
                            var dataCollect = new ObservableCollection<EventLog>(); ;
                            foreach (DataRow productRow in ds.Tables["EventLog"].Rows)
                            {
                                dataCollect.Add(
                                    new EventLog(
                                (int)productRow["EventID"],
                                (string)productRow["FaultStyle"],
                                (string)productRow["EventConent"],
                                (string)productRow["Date"],
                                (string)productRow["Unit"],
                                (string)productRow["Millisecond"]));
                            }
                            return dataCollect;                           
                        }
                    case DataTypeEnum.ProtectSetPoint:
                        {


                            var dataCollect = new ObservableCollection<ProtectSetPoint>(); ;
                            foreach (DataRow productRow in ds.Tables["ProtectSetPoint"].Rows)
                            {
                                dataCollect.Add(
                                    new ProtectSetPoint(
                                (int) productRow["InternalID"], 
                                (string)productRow["Type"], 
                                (int)productRow["EndPoint"],
                                (string)productRow["ProtectSetName"],
                                (double)productRow["ParameterValue"], 
                                (double)productRow["CalibrationCoefficient"], 
                                (string)productRow["Unit"],
                                (string)productRow["Range"],
                                (string)productRow["Comment"] ));

                            }
                            return dataCollect;                            
                        
                            
                        }
                    default:
                        {
                            throw new ArgumentOutOfRangeException("所使用的枚举值不在范围之内!");
                        }
                }

                throw new ArgumentOutOfRangeException("所使用的枚举值不在范围之内!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void  UpdateTable(DataSet ds, DataTypeEnum typeEnum, object dataObserver)
        {
            try
            {
               
                switch (typeEnum)
                {
                    case DataTypeEnum.Telesignalisation:
                        {
                            ds.Tables["Telesignalisation"].Rows.Clear();
                            var datas = dataObserver as ObservableCollection<Telesignalisation>;
                            foreach (var m in datas)
                            {
                                var productRow = ds.Tables["Telesignalisation"].NewRow();
                                productRow["InternalID"] = m.InternalID;
                                productRow["TelesignalisationName"] = m.TelesignalisationName;
                                productRow["TelesignalisationID"] = m.TelesignalisationID;
                                productRow["TelesignalisationValue"] = m.TelesignalisationResult;
                                productRow["IsNot"] = m.IsNot;
                                productRow["Date"] = m.Date;
                                productRow["Comment"] = m.Comment;

                                ds.Tables["Telesignalisation"].Rows.Add(productRow);
                            }
                            break;

                        }
                    case DataTypeEnum.Telemetering:
                        {

                            ds.Tables["Telemetering"].Rows.Clear();
                            var datas = dataObserver as ObservableCollection<Telemetering>;
                            foreach (var m in datas)
                            {
                                var productRow = ds.Tables["Telemetering"].NewRow();
                                productRow["InternalID"] = m.InternalID;
                                productRow["TelemeteringName"] = m.TelemeteringName;
                                productRow["TelemeteringID"] = m.TelemeteringID;
                                productRow["CalibrationCoefficient"] = m.CalibrationCoefficient;
                                productRow["TelemeteringValue"] = m.TelemeteringValue;
                                productRow["Unit"] = m.Unit;
                                productRow["Mark"] = m.Mark;
                                productRow["Comment"] = m.Comment;
                                ds.Tables["Telemetering"].Rows.Add(productRow);
                            }
                            break;
                           
                        }
                    case DataTypeEnum.Telecontrol:
                        {
                            ds.Tables["Telecontrol"].Rows.Clear();
                            var datas = dataObserver as ObservableCollection<Telecontrol>;
                            foreach (var m in datas)
                            {
                                var productRow = ds.Tables["Telecontrol"].NewRow();

                                 productRow["TelecontrolID"] = m.TelecontrolID;
                                 productRow["TelecontrolComment"] = m.TelecontrolComment;
                                 productRow["TelecontrolState"] = m.TelecontrolState;
                                 productRow["TelecontrolOperate"] = m.TelecontrolOperate;
                                 productRow["TelecontrolOperateState"] = m.TelecontrolOperateState;
                                 productRow["DeviceActionTime"] = m.DeviceActionTime;

                                ds.Tables["Telecontrol"].Rows.Add(productRow);
                            }
                            break;

                        }
                    case DataTypeEnum.SystemParameter:
                        {
                            ds.Tables["SystemParameter"].Rows.Clear();
                            var datas = dataObserver as ObservableCollection<SystemParameter>;
                            foreach (var m in datas)
                            {
                                var productRow = ds.Tables["SystemParameter"].NewRow();

                                productRow["InternalID"] =m.InternalID;
                                productRow["paramName"] = m.ParamName;
                                productRow["defaultValue"] = m.DefaultValue;
                                productRow["description"] = m.Description;

                                ds.Tables["SystemParameter"].Rows.Add(productRow);
                            }
                            break;
                        }

                    case DataTypeEnum.SystemCalibration:
                        {


                            ds.Tables["SystemCalibration"].Rows.Clear();
                            var datas = dataObserver as ObservableCollection<SystemCalibration>;
                            foreach (var m in datas)
                            {
                                var productRow = ds.Tables["SystemCalibration"].NewRow();

                                productRow["InternalID"] = m.InternalID;
                                productRow["EndPoint"] = m.EndPoint;
                                productRow["ParamName"] = m.ParamName;
                                productRow["ParamValue"] = m.ParamValue;
                                productRow["StandardValue"] = m.StandardValue;
                                productRow["CallCoefficient"] = m.CallCoefficient;
                                productRow["DownloadCoefficient"] = m.DownloadCoefficient;
                                productRow["AverageValue"] = m.AverageValue;
                                productRow["Data1"] = m.Data1;
                                productRow["Data2"] = m.Data2;
                                productRow["Data3"] = m.Data3; 
                                productRow["Data4"] = m.Data4;
                                productRow["Data5"] = m.Data5;
                                productRow["Data6"] = m.Data6;
                                productRow["Data7"] = m.Data7;
                                productRow["Data8"] = m.Data8;
                                productRow["Data9"] = m.Data9;
                                productRow["Data10"] = m.Data10;
                                productRow["Comment"] = m.Comment;

                                ds.Tables["SystemCalibration"].Rows.Add(productRow);
                            }
                            break;

                           
                        }
                    case DataTypeEnum.EventLog:
                        {
                            ds.Tables["EventLog"].Rows.Clear();
                            var datas = dataObserver as ObservableCollection<EventLog>;
                            foreach (var m in datas)
                            {
                                var productRow = ds.Tables["EventLog"].NewRow();

                                 productRow["EventID"] = m.EventID;
                                 productRow["FaultStyle"] = m.FaultStyle;
                                 productRow["EventConent"] = m.EventConent;
                                 productRow["Date"] = m.Date;
                                 productRow["Unit"] = m.Unit;
                                 productRow["Millisecond"] = m.Millisecond;

                                ds.Tables["EventLog"].Rows.Add(productRow);
                            }
                            break;
                        }
                    case DataTypeEnum.ProtectSetPoint:
                        {
                            ds.Tables["ProtectSetPoint"].Rows.Clear();
                            var datas = dataObserver as ObservableCollection<ProtectSetPoint>;
                            foreach (var m in datas)
                            {
                                var productRow = ds.Tables["ProtectSetPoint"].NewRow();

                                 productRow["InternalID"] = m.InternalID;
                                 productRow["Type"] = m.Type;
                                 productRow["EndPoint"] = m.EndPoint;
                                 productRow["ProtectSetName"] = m.ProtectSetName;
                                 productRow["ParameterValue"] = m.ParameterValue;
                                 productRow["CalibrationCoefficient"] = m.CalibrationCoefficient;
                                 productRow["Unit"] = m.Unit;
                                 productRow["Range"] = m.Range;
                                 productRow["Comment"] = m.Comment;

                                 ds.Tables["ProtectSetPoint"].Rows.Add(productRow);
                            }

                            break;
                        }
                    default:
                        {
                            throw new ArgumentOutOfRangeException("所使用的枚举值不在范围之内!");
                        }
                }

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        

    }
}
