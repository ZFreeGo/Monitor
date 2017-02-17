using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ZFreeGo.TransportProtocol.NetworkAccess.BasicElement
{
    /// <summary>
    /// 选择执行选项 
    /// </summary>
    public enum SelectExecuteOption : byte
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
    /// 双命令状态
    /// </summary>
    public enum DCOState : byte
    {
        /// <summary>
        /// 开启
        /// </summary>
        On = 2,
        /// <summary>
        /// 关闭
        /// </summary>
        Off = 1,
        
        
    }

    /// <summary>
    /// 双命令DCO
    /// </summary>
    public class DoubleCommand
    {
        /// <summary>
        /// 双命令
        /// </summary>
        public byte DCO;

        /// <summary>
        ///1-选择 / 0-执行
        /// </summary>
        public SelectExecuteOption SE
        {
            set
            {
                if (((byte)value & 0x01) == 1)
                {
                    DCO |= 0x80;
                }
                else
                {
                    DCO &= 0x7F;
                }
            }
            get
            {
                if ((DCO & 0x80) == 0x80)
                {
                    return SelectExecuteOption.Select;
                }
                else
                {
                    return SelectExecuteOption.Execute;
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
                DCO &= 0x8E;
                DCO |= (byte)((byte)DCO << 3);
            }
            get
            {
                return (QUtype)((DCO & 0x7C) >> 3);
            }
        }

        /// <summary>
        /// 双命令状态 2-启动 1-断开
        /// </summary>
        public DCOState DCS
        {
            set
            {
                DCO &= 0xFC;
                if (((byte)value & 0x03) == 0x01)
                {                  
                    DCO |= 0x01;
                }
                else if (((byte)value & 0x03) == 0x02)
                {                    
                    DCO |= 0x02;
                }
              
            }
            get
            {
                return (DCOState)(DCO & 0x03);
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dco">双命令字节</param>
        public DoubleCommand(byte dco)
        {
            DCO = dco;
        }
        /// <summary>
        /// 双命令初始化
        /// </summary>
        /// <param name="se">1-选择 / 0-执行</param>
        /// <param name="qu">命令属性</param>
        /// <param name="scs">2-合 1-开</param>
        public DoubleCommand(SelectExecuteOption se, QUtype qu, DCOState dcs)
        {
            SE = se;
            QU = qu;
            DCS = dcs;
        }
    }

    
}
