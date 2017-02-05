using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement
{
    /// <summary>
    /// 动作描述
    /// </summary>
    public enum ActionDescrible : byte
    {
        /// <summary>
        /// 选择
        /// </summary>
        Select = 1,
        /// <summary>
        /// 执行
        /// </summary>
        Execute = 0,
    }

    /// <summary>
    /// 设定命令限定词 
    /// </summary>
    /// 
    public class QualifyCommandSet
    {
        /// <summary>
        /// 设定命令限定词
        /// </summary>
        public byte QOS;

        /// <summary>
        /// 设定命令限定词初始化
        /// </summary>
        /// <param name="ads">动作描述 选择/执行</param>
        public QualifyCommandSet(ActionDescrible ads)
        {
            QOS = (byte)((byte)ads << 7);
        }


    }
}
