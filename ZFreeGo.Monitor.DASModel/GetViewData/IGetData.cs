using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ZFreeGo.Monitor.DASModel.DataItemSet;

namespace ZFreeGo.Monitor.DASModel.GetViewData
{
    public interface IGetData
    {
        ObservableCollection<SOE> GetSOEList();
        ObservableCollection<SystemCalibration> GetSystemCalibrationList();
        ObservableCollection<Telesignalisation> GetTelesignalisationList();
        ObservableCollection<Telemetering> GetTelemeteringList();
        ObservableCollection<SystemParameter> GetSystemParameterList();
        ObservableCollection<Telecontrol> GetTelecontrolList();
        ObservableCollection<ProtectSetPoint> GetProtectSetPointList();
    }
}
