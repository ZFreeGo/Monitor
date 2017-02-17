using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ZFreeGo.Monitor.CommCenter
{

    public class RtuFrameArrivedEventArgs : EventArgs
    {
        public RtuFrameArrivedEventArgs(UInt16[] frame)
        {
            UInt16[]  DataFrame = frame;
        }
        public UInt16[] DataFrame;
    }
}
