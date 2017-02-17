using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.CommCenter
{
    /// <summary>
    /// 录波信息
    /// </summary>
    public class RecordWaveformEventArgs : EventArgs
    {
        /// <summary>
        /// 录波数据
        /// </summary>
        public RecordWaveform Waveform;
        /// <summary>
        /// 接收帧数据数组
        /// </summary>
        public byte[] DataArray;

        /// <summary>
        /// 录波数据事件参数
        /// </summary>
        /// <param name="wave">录播数据结构体</param>
        /// <param name="array">字节数组</param>
        public RecordWaveformEventArgs(RecordWaveform wave, byte[] array)
        {
            Waveform = wave;
            DataArray = array;
        }



    }
}
