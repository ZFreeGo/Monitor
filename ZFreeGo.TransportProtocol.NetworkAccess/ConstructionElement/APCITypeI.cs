using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement
{
    /// <summary>
    /// I 格式（ Information Transmit Format） ,I 格式的 APDU 至少必须包含一个 ASDU
    /// </summary>
    public class APCITypeI :  ApplicationProtocalControlnformation
    {
        /// <summary>
        /// 发送序列号
        /// </summary>
        public UInt16 TransmitSequenceNumber
        {
            get
            {
                return (ushort)((ControlDomain1 >> 1) + ((UInt16)ControlDomain2 << 7));//将低7bit与高8bit(7-14)组合
            }
            set
            {
                ControlDomain1 = (byte)(((UInt16)value << 1) & (0x00FF)); //获取低7bit，0bit为0
                ControlDomain2 = (byte)(value >> 7);              //获取高8bit(7-14)
            }
        }

        /// <summary>
        /// 接收序列号
        /// </summary>
        public UInt16 ReceiveSequenceNumber
        {
            get
            {
                return (ushort)((ControlDomain3 >> 1) + ((UInt16)ControlDomain4 << 7));//将低7bit与高8bit(7-14)组合
            }
            set
            {
                ControlDomain3 = (byte)(((UInt16)value << 1) & (0x00FF));//获取低7bit，0bit为0
                ControlDomain4 = (byte)(value >> 7);            //获取高8bit(7-14)
            }
        }



        /// <summary>
        /// APCI默认初始化
        /// </summary>
        public APCITypeI(byte[] dataArray)
            : base(dataArray) 
        {
            
        }
        /// <summary>
        /// 初始化APCITypeI
        /// </summary>
        /// <param name="apduLength">APDU长度</param>
        /// <param name="transmitSequenceNumber">发送序列号</param>
        /// <param name="receiveSequenceNumber">接收序列号</param>
        public APCITypeI(byte apduLength, UInt16 transmitSequenceNumber, UInt16 receiveSequenceNumber)
            : base(0x68, apduLength, 0, 0, 0, 0)
        {
            TransmitSequenceNumber = transmitSequenceNumber;
            ReceiveSequenceNumber = receiveSequenceNumber;

        }




    }
}
