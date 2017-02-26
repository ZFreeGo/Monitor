using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.DASModel.Helper
{
    /// <summary>
    /// 校准帧信息
    /// </summary>
    public class OldCalibrationEventArgs : EventArgs
    {
        /// <summary>
        /// 初始化校准帧信息参数
        /// </summary>
        /// <param name="head">头数据</param>
        /// <param name="frame">帧数据</param>
        /// <param name="property">帧数据</param>
        public OldCalibrationEventArgs(byte[] data, UInt32[] frame, CalibrationAction property)
        {
            DataFrame = frame;
            Data = data;
            Property = property;
        }

        /// <summary>
        /// 完整帧数据
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// 数据帧
        /// </summary>
        public UInt32[] DataFrame;
        /// <summary>
        /// 校准行为
        /// </summary>
        public CalibrationAction Property;
    
    }
}
