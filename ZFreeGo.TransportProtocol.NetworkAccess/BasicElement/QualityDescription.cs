using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.BasicElement
{
    /// <summary>
    /// 品质描述词QualityDescription
    /// </summary>
    public class QualityDescription 
    {
        /// <summary>
        /// 1-溢出 0-未溢出
        /// </summary>
        public const  byte OV = 0x01;
         /// <summary>
        /// 1-被闭锁 0-未被闭锁
        /// </summary>
        public const byte BL = 0x10;
        /// <summary>
        /// 1-被取代 0-未被取代
        /// </summary>
        public const byte SB = 0x20;
        /// <summary>
        /// 1-非当前值 0-当前值
        /// </summary>
        public const byte NT = 0x40;

        /// <summary>
        /// 1-无效 0-有效
        /// </summary>
        public const byte IV = 0x80;

        /// <summary>
        /// 初始化品质描述词QDS
        /// </summary>
        /// <param name="qds">品质描述</param>
        public QualityDescription(byte qds)
        {
            QDS = qds;
           
        }

        /// <summary>
        /// 品质描述词
        /// </summary>
        public byte QDS
        {
            get;
            set;
        }
    }
}
