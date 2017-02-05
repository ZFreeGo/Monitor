using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement
{

    /// <summary>
    /// 带品质描述词的单点信息
    /// </summary>
    public class SingleInformationQualifier
    {
      
        /// <summary>
        /// 带品质描述词的单点信息
        /// </summary>
        public byte SIQ;
          /// <summary>
        /// 单点信息 0-开 1-合
        /// </summary>
        public byte SPI
        {
            set
            {
                if ((value & 0x01) == 0x01)
                {
                    SIQ |= 0x01;
                }
                else
                {
                    SIQ &= 0xFE;
                }
            }
            get
            {
                return (byte)(SIQ & 0x01);
            }
        }
 
        /// <summary>
        /// 品质描述词
        /// </summary>
        public QualityDescription QDS
        {
            set
            {
                SIQ &= 0x01;
                SIQ |= (byte)(value.QDS & 0xFE);
            }
            get
            {
                return new QualityDescription((byte)(SIQ & 0xFE));
            }
            
        }

        /// <summary>
        /// 初始化带品质描述词的单点信息
        /// </summary>
        /// <param name="siq">带品质描述词的单点信息</param>
        public SingleInformationQualifier(byte siq)
        {
            SIQ = siq;
        }

    }
}
