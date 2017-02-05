using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement
{
    /// <summary>
    /// 参数类别 
    /// </summary>
    public enum KindParameter : byte
    {
        /// <summary>
        /// 未用
        /// </summary>
        NoUsed = 0,
        /// <summary>
        /// 门限值
        /// </summary>
        Thresholdvalue = 1,
        /// <summary>
        /// 平滑系数（滤波时间常数）
        /// </summary>
         SmoothingFactor = 2,
        /// <summary>
        /// 传送测量值的下限
        /// </summary>
        LowLimit = 3,
        /// <summary>
        /// 传送测量值的上限
        /// </summary>
        UpLimit = 4,
    }
    /// <summary>
    /// 当地参数改变
    /// </summary>
    public enum LocalParameterChange : byte
    {
        /// <summary>
        /// 未改变
        /// </summary>
        NoChange = 0,
        /// <summary>
        /// 改变
        /// </summary>
        Change = 1,
    }
    /// <summary>
    /// 参数在运行 
    /// </summary>
    public enum ParameterRun : byte
    {
        /// <summary>
        /// 运行
        /// </summary>
        Run = 0,
        /// <summary>
        /// 未运行
        /// </summary>
        NotRun = 1,

    }


    /// <summary>
    /// 测量值参数限定词 (QPM)
    /// </summary>
    public class QualifyParameterMeasure
    {
        /// <summary>
        ///测量值参数限定词 (QPM)
        /// </summary>
        public byte QPM;
        /// <summary>
        /// 参数类别
        /// </summary>
        public KindParameter KPA
        {
            set
            {
                QPM &= 0xC0;
                QPM |= (byte)value;
            }
            get
            {
                return (KindParameter)(QPM & 0x3F);
            }
        }
        /// <summary>
        /// 当地参数改变
        /// </summary>
        public LocalParameterChange LPC
        {
            set
            {
                QPM &= 0xBF;
                QPM |= (byte)((byte)value << 6); 
            }
            get
            {
                return (LocalParameterChange)((QPM & 0x40) >> 6);
            }
        }
        /// <summary>
        /// 参数在运行
        /// </summary>
        public ParameterRun POP
        {
            set
            {
                QPM &= 0x7F;
                QPM |= (byte)((byte)value << 7);
            }
            get
            {
                return (ParameterRun)((QPM & 0x40) >> 7);
            }
        }

        /// <summary>
        /// 测量值参数限定词
        /// </summary>
        /// <param name="qpm">测量值参数限定词</param>
        public  QualifyParameterMeasure(byte qpm)
        {
            QPM = qpm;
        }
        /// <summary>
        /// 测量值参数限定词
        /// </summary>
        /// <param name="kpa">参数类别</param>
        /// <param name="lpc">当地参数改变</param>
        /// <param name="pop">参数在运行</param>
        public QualifyParameterMeasure(KindParameter kpa, LocalParameterChange lpc, ParameterRun pop)
        {
            QPM = (byte)((byte)kpa | (((byte)lpc) << 6) | ((byte)pop << 7));
        }
    }
}
