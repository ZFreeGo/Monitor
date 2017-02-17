using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.ElementParam
{
    /// <summary>
    /// 参数类型代号
    /// </summary>
    public enum DataTypeEnum : int 
    {
        Telecontrol = 1,
        Telemetering ,
        Telesignalisation ,
        SystemParameter ,
        SystemCalibration ,
        EventLog ,
        CommunicationParamete ,
        ProtectSetPoint
    }
}
