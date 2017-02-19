using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ZFreeGo.Monitor.DASModel.Table;

namespace ZFreeGo.Monitor.DASModel.GetData
{
    public interface IGetData
    {
        ObservableCollection<EventLog> GetEventLogList();
        ObservableCollection<SystemCalibration> GetSystemCalibrationList();
        ObservableCollection<Telesignalisation> GetTelesignalisationList();
        ObservableCollection<Telemetering> GetTelemeteringList();
        ObservableCollection<SystemParameter> GetSystemParameterList();
        ObservableCollection<Telecontrol> GetTelecontrolList();
        ObservableCollection<ProtectSetPoint> GetProtectSetPointList();
    }
}
