using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement
{

    /// <summary>
    /// 带品质描述词的双点信息
    /// </summary>
    public class DoubleInformationQualifier
    {
        /// <summary>
        /// 带品质描述词的双点信息
        /// </summary>
        public byte DIQ;
        /// <summary>
        /// 双点信息 0-不确定或中间状态 1-确定状态开 2-确定状态合 3-不确定
        /// </summary>
        public byte DPI
        {
            set
            {
                DIQ &= 0xFC;
                DIQ = (byte)((value & 0x03) | DIQ);
               
            }
            get
            {
                return (byte)(DIQ & 0x03);
            }
        }
 
        /// <summary>
        /// 品质描述词
        /// </summary>
        public QualityDescription QDS
        {
            set
            {
                DIQ &= 0x03;
                DIQ |= (byte)(value.QDS & 0xFC);
            }
            get
            {
                return new QualityDescription((byte)(DIQ & 0xFC));
            }
            
        }

        /// <summary>
        /// 初始化带品质描述词的单点信息
        /// </summary>
        /// <param name="DIQ">带品质描述词的单点信息</param>
        public DoubleInformationQualifier(byte diq)
        {
            DIQ = diq;
        }

    }
}
