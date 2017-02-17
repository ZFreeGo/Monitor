using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.BasicElement
{
    /// <summary>
    /// 节准备就绪限定词
    /// </summary>
    public class SectionReadyQulifier
    {
        // <summary>
        /// 节准备就绪
        /// </summary>
        public const byte ACK = 0;
        /// <summary>
        /// 节未准备就绪
        /// </summary>
        public const byte NonACK = 0x80;

        /// <summary>
        /// 文件准备就绪限定词
        /// </summary>
        public byte SRQ;
        public SectionReadyQulifier(byte srq)
        {
            SRQ = srq;            
        }
    }
}
