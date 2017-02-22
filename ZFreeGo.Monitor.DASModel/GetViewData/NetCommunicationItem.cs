using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.Monitor.DASModel.DataItemSet;

namespace ZFreeGo.Monitor.DASModel.GetViewData
{

    public class NetCommunicationItem
    {
        private NetParameter _netParameter;

        public NetParameter NetParameter
        {
            get
            {
                return _netParameter;
            }

        }

        public NetCommunicationItem()
        {
            _netParameter = new NetParameter();
        }
    }
}
