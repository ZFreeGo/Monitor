using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.CommCenter
{
    /// <summary>
    /// 单次录波数据合集
    /// </summary>
    public class RecordDatOneCollect
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<RecordDataItem> RecordList;

        /// <summary>
        /// 数据波形数据
        /// </summary>
        public event EventHandler<WaveformArrivedEventArgs> WaveformArrived;
        /// <summary>
        /// 添加计数
        /// </summary>
        private int AddCount;
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished;
        public RecordDatOneCollect()
        {
            RecordList = new List<RecordDataItem>();

            var item = new RecordDataItem(128, "A相电流", 1, 12);
            RecordList.Add(item);
            item = new RecordDataItem(128, "B相电流", 2 , 12);
            RecordList.Add(item);
            item = new RecordDataItem(128, "C相电流", 3, 12);
            RecordList.Add(item);
            item = new RecordDataItem(128, "零序电流", 4, 12);
            RecordList.Add(item);
            item = new RecordDataItem(128, "A相电压", 5, 12);
            RecordList.Add(item);
            item = new RecordDataItem(128, "B相电压", 6, 12);
            RecordList.Add(item);
            item = new RecordDataItem(128, "C相电压", 7, 12);
            RecordList.Add(item);
            item = new RecordDataItem(128, "零序电压", 8, 12);
          

            AddCount = 0;
            IsFinished = false;
        }


        /// <summary>
        /// 将波形记录信息添加到指定位置 
        /// </summary>
        /// <param name="rwf">一帧数据波形信息</param>
        /// <returns>是否完成添加 true-完成 false-未完成</returns>
        public bool AddRecordWaveform(RecordWaveform rwf)
        {
            var state = RecordList[rwf.Channel].AddInformation(rwf);
            
            if (++AddCount >= 8 * 12 * 2)
            {
                IsFinished = true;
                foreach(var m in  RecordList)
                {
                    if(m.IsFinished == false)
                    {
                        IsFinished = false;
                        break;
                    }
                }
                if (IsFinished)
                {
                    if (WaveformArrived != null)
                    {
                        WaveformArrived(this, new WaveformArrivedEventArgs(RecordList));
                    }
                }

            }

            return state;
        }
    }
}
