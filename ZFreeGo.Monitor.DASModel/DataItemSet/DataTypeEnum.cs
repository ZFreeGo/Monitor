using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.DASModel.DataItemSet
{
    public enum DataTypeEnum : int
    {
        Telecontrol = 1,
        Telemetering,
        Telesignalisation,
        SystemParameter,
        SystemCalibration,
        EventLog,
        CommunicationParamete,
        ProtectSetPoint
    }
}
