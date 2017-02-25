using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.DASModel.Helper
{
    /// <summary>
    /// 通用的公共路径,  
    /// TODO：需要检查每个路径
    /// </summary>
    public class CommonPath
    {
        /// <summary>
        /// 账户xml路径
        /// </summary>
        public static string AccountXmlPath = @"log\Account\UserAccount.xml";
        // string pathxmlAccount = @"t2.xml";
        /// <summary>
        /// 账户xsd路径
        /// </summary>
        public static string AccountXsdPath = @"log\Account\UserAccount.xsd";

        /// <summary>
        /// 日志文件夹路径
        /// </summary>
        public static string LogDirectoryPath = @"log\log";


        /// <summary>
        /// 遥信XML表格路径
        /// </summary>
        public static string TelesignalisationXmlPath = @"Config\Telesignalisation.xml";

        /// <summary>
        /// 遥信XSD架构路径
        /// </summary>
        public static string TelesignalisationXsdPath = @"Config\Telesignalisation.xsd";

        /// <summary>
        /// 遥测XML路径
        /// </summary>
        public static string TelemeteringXmlPath = @"Config\Telemetering.xml";
        /// <summary>
        /// 遥测SD架构路径
        /// </summary>
        public static string TelemeteringXsdPath = @"Config\Telemetering.xsd";

        /// <summary>
        /// 系统参数XML路径
        /// </summary>
        public static string SystemParameterXmlPath = @"Config\SystemParameter.xml";
        /// <summary>
        /// 系统参数XSD架构路径
        /// </summary>
        public static string SystemParameterXsdPath = @"Config\SystemParameter.xsd";

        /// <summary>
        /// 遥控参数XML路径
        /// </summary>
        public static string TelecontrolXmlPath = @"Config\Telecontrol.xml";
        /// <summary>
        /// 遥控参数XSD架构路径
        /// </summary>
        public static string TelecontrolXsdPath = @"Config\Telecontrol.xsd";

        /// <summary>
        /// 保护定值参数XML路径
        /// </summary>
        public static string ProtectSetPointXmlPath = @"Config\ProtectSetPoint.xml";
        /// <summary>
        /// 保护定值XSD架构路径
        /// </summary>
        public static string ProtectSetPointXsdPath = @"Config\ProtectSetPoint.xsd";

        /// <summary>
        /// 系统校准XML路径
        /// </summary>
        public static string SystemCalibrationXmlPath = @"Config\SystemCalibration.xml";
        /// <summary>
        /// 系统校准XSD架构路径
        /// </summary>
        public static string SystemCalibrationXsdPath = @"Config\SystemCalibration.xsd";

        /// <summary>
        /// 事件记录XML路径
        /// </summary>
        public static string SOEXmlPath = @"Config\SOE.xml";
        /// <summary>
        /// 事件记录XSD架构路径
        /// </summary>
        public static string SOEXsdPath = @"Config\SOE.xsd";


        public static string DataBase =  @"config\Database\Das.db";
    }
}
