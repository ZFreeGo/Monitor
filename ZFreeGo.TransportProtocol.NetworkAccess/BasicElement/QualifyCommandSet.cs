using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.BasicElement
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
        /// 获取命令限定词
        /// </summary>
        public ActionDescrible Describle
        {
            get
            {
                if ((QOS & 0x80) == 0x80)
                {
                    return ActionDescrible.Select;
                }
                else
                {
                    return ActionDescrible.Execute;
                }

            }
        }
        /// <summary>
        /// 设定命令限定词初始化
        /// </summary>
        /// <param name="ads">动作描述 选择/执行</param>
        public QualifyCommandSet(ActionDescrible ads)
        {
            QOS = (byte)((byte)ads << 7);
        }
        /// <summary>
        /// 设定命令初始化
        /// </summary>
        /// <param name="data">字节</param>
        public QualifyCommandSet(byte data)
        {
            QOS = data;
        }


    }
}
