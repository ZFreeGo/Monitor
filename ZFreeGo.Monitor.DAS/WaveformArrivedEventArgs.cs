using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.Monitor.AutoStudio.CommCenter;

namespace ZFreeGo.Monitor.AutoStudio
{
    public class WaveformArrivedEventArgs : EventArgs
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<RecordDataItem> RecordList;

        public WaveformArrivedEventArgs( List<RecordDataItem> wave)
        {
            RecordList = wave;
        }

    }
}
