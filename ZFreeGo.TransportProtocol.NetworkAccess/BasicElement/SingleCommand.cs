using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ZFreeGo.TransportProtocol.NetworkAccess.BasicElement
{
    /// <summary>
    /// 单命令SCO
    /// </summary>
    public class SingleCommand 
    {
        /// <summary>
        /// 单命令
        /// </summary>
        public byte SCO;

        /// <summary>
        ///1-选择 / 0-执行
        /// </summary>
        public byte SE
        {
            set
            {
                if ((value & 0x01) == 1)
                {
                    SCO |= 0x80;
                }
                else
                {
                    SCO &= 0x7F;
                }
            }
            get
            {
                if ((SCO & 0x80) == 0x80)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 命令属性
        /// </summary>
        public QUtype QU
        {
            set
            {
                SCO &= 0x8E;
                SCO |= (byte)((byte)SCO << 3);
            }
            get
            {
                return (QUtype)((SCO & 0x7C) >> 3);
            }
        }

        /// <summary>
        /// 单命令状态 1-合 0-开
        /// </summary>
        public byte SCS
        {
            set
            {
                if ((value & 0x01) == 1)
                {
                    SCO |= 0x01;
                }
                else
                {
                    SCO &= 0xFE;
                }
            }
            get
            {
                return (byte)(SCO & 0x01);
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sco">单命令字节</param>
        public SingleCommand(byte sco)
        {
            SCO = sco;
        }
        /// <summary>
        /// 单命令初始化
        /// </summary>
        /// <param name="se">1-选择 / 0-执行</param>
        /// <param name="qu">命令属性</param>
        /// <param name="scs">1-合 0-开</param>
        public SingleCommand(byte se, QUtype qu, byte scs)
        {
            SE = se;
            QU = qu;
            SCS = scs;
        }
    }

    
}
