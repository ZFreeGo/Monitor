using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.Monitor.AutoStudio.CommCenter
{
    /// <summary>
    /// 录播数据结构
    /// </summary>
    public class RecordWaveform
    {
        /// <summary>
        /// 通道号
        /// </summary>
        public byte Channel
        {
            private set;
            get;
        }
        /// <summary>
        /// 周波号
        /// </summary>
        public byte WaveformNum
         {
            private set;
            get;
        }
        /// <summary>
        /// 组号
        /// </summary>
        public byte GroupNum
        {
            private set;
            get;
        }
        /// <summary>
        /// 时间戳
        /// </summary>
        public CP56Time2a TimeStamp
        {
            private set;
            get;
        }
        /// <summary>
        /// 原始ADC采样数组
        /// </summary>
        public UInt16[] RawAdcArray
        {
            private set;
            get;
        }
        /// <summary>
        /// 原始录波点字节数据
        /// </summary>
        public byte[] RawByteArray
        {
            private set;
            get;
        }

        /// <summary>
        /// 转换系数
        /// </summary>
        public double Factor
        {
            private set;
            get;
        }


        /// <summary>
        /// 录波数据初始化
        /// </summary>
        /// <param name="rawframe">字节数组</param>
        public  RecordWaveform(byte[] rawframe)
        {
            Channel = rawframe[7];
            WaveformNum = rawframe[8];
            GroupNum = rawframe[9];
            byte[] data = new byte[7];
            Array.Copy(rawframe, 10, data, 0, 7);
            TimeStamp = new CP56Time2a(data);
            int len = rawframe[1] - 7 - 3 - 3;
            RawByteArray = new byte[len];
            Array.Copy(rawframe, 17, RawByteArray, 0, len);
            RawAdcArray = new ushort[len/2];
            int count = len / 2;
            for(int i = 0; i < count ; i++)
            {
                RawAdcArray[count++] = ElementTool.CombinationByte(rawframe[2*i], rawframe[2*i+1]);
            }
        }
    }
}
