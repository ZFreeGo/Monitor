using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocols.BasicElement;


namespace ZFreeGo.Monitor.AutoStudio.CommCenter
{
    /// <summary>
    /// 一条录波数据条目
    /// </summary>
    public class RecordDataItem
    {

        ///// <summary>
        ///// 录波数据
        ///// </summary>
        //public double[] WaveData
        //{
        //    private set;
        //    get;
        //}
        /// <summary>
        /// 录波数据,ADC值
        /// </summary>
        public UInt16[] WaveDataAdc
        {
            private set;
            get;
        }
        /// <summary>
        /// 录播数据时间戳
        /// </summary>
        public CP56Time2a TimeStamp
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
        /// 描述词
        /// </summary>
        public String Desrc;


        private bool isFinished;
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool IsFinished
        {
            private set
            {
                isFinished = value;
            }
            get
            {
                if ( isFinished)
                {
                    return true;
                }
                bool state = true;
                //比较所有标志位
                while(state )
                {
                    foreach(var m in  RecordMask)
                    {
                        foreach(var ele in m)
                        {
                            if (ele  == false)
                            {
                                return false;
                            }
                        }
                    }
                    isFinished = true;
                    return true;
                }
                isFinished = true;
                return true;
            }
        }
        /// <summary>
        /// 对应通道号
        /// </summary>
        public byte Channel
        {
            private get;
            set;
        }

        /// <summary>
        /// 每周波点数
        /// </summary>
        public int WavePointNum;

        /// <summary>
        /// 波形数量
        /// </summary>
        private int WaveCount;
        /// <summary>
        /// 接收记录表
        /// </summary>
        private bool[][] RecordMask
        {
            set;
            get;
        }

        /// <summary>
        /// 一条记录条目
        /// </summary>
        /// <param name=" waveNum">每周波点数</param>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="drs">描述</param>
        /// <param name="channelNum">通道号</param>
        /// <param name="waveCount">波形数量</param>
        public RecordDataItem(int waveNum, string drs, int channelNum, int waveCount)
        {
            WavePointNum = waveNum;
            //WaveData = new double[waveNum * waveCount];
            WaveDataAdc = new ushort[waveNum * waveCount];
            WaveCount = waveCount;
            isFinished = false;
            Desrc = drs;
            RecordMask = new bool[waveCount][];
            for (int i = 0; i < waveCount; i++)
            {
                RecordMask[i] = new bool[] {false, false};
            }
        }

        /// <summary>
        /// 添加波形片段
        /// </summary>
        /// <param name="rwf">波形信息</param>
        /// <returns>添加结果 true-添加成功， fasle-添加失败</returns>
        public bool AddInformation(RecordWaveform rwf)
        {
            //判断通道号
            if (Channel ==  rwf.Channel)
            {
                if (TimeStamp == null)
                {
                    //初始化时间戳
                    TimeStamp = new CP56Time2a(rwf.TimeStamp.GetDataArray());                    
                }
                else
                {
                    //判断时间戳是否相同，不同则返回
                   if(! TimeStamp.IsEqual(rwf.TimeStamp))
                   {
                       return false;
                   }
                }
                if (RecordMask[rwf.WaveformNum - 1][rwf.GroupNum - 1] == false)
                {
                    RecordMask[rwf.WaveformNum - 1][rwf.GroupNum - 1] = true;
                    //计算数据存储索引
                    int index = (rwf.WaveformNum - 1) * WavePointNum + (rwf.GroupNum - 1) * 64;
                    Array.Copy(rwf.RawAdcArray, 0, WaveDataAdc, index, rwf.RawAdcArray.Length);
                    return true;
                }
                else
                {
                    throw new Exception("重复添加波形--RecordDataItem-AddInformation");
                }


            }
            return false;
        }

        






    }
}
