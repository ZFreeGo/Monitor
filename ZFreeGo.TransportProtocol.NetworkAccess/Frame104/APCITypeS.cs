using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.Frame104
{
    /// <summary>
    /// S 格式（ Numbered Supervisory Function） S 格式的 APDU 只包括 APCI
    /// </summary>
    public class APCITypeS
    {
        /// <summary>
        /// 通用apci数据,S 格式的 APDU 只包括 APCI
        /// </summary>
        private ApplicationProtocalControlnformation apci;
        /// <summary>
        /// 接收序列号
        /// </summary>
        public UInt16 ReceiveSequenceNumber
        {
            get
            {
                return (ushort)(apci.ControlDomain3 >> 1 + (UInt16)apci.ControlDomain4 << 7);//将低7bit与高8bit(7-14)组合
            }
            set
            {
                apci.ControlDomain1 = 0x01;
                apci.ControlDomain2 = 0;
                apci.ControlDomain3 = (byte)((value << 1) & (0x00FF));//获取低7bit，0bit为0
                apci.ControlDomain4 = (byte)(value >> 7);            //获取高8bit(7-14)
            }
        }

        /// <summary>
        /// 获取APCI数据
        /// </summary>
        /// <returns>6字节数组</returns>
        public byte[] GetAPCIDataArray()
        {
            return apci.GetAPCIDataArray();
        }

        /// <summary>
        /// APCI-S监控帧
        /// </summary>
        /// <param name="reciveNum">接收序列号</param>
        public APCITypeS(UInt16 reciveNum)
        {
            apci = new ApplicationProtocalControlnformation(0x68, 4, 0x01, 0, 0,  0);
            ReceiveSequenceNumber = reciveNum;
        }
    }
}
