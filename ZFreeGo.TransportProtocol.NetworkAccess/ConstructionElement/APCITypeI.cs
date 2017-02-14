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


        /// <summary>
        /// 转换字节数组显示
        /// </summary>
        /// <returns>信息字符串</returns>
        public override string ToString()
        {
            StringBuilder strBuild = new StringBuilder(45);
            strBuild.AppendFormat("{0:X00}", APDULength);
            strBuild.Append(" ");
            strBuild.AppendFormat("{0:X00}", ControlDomain1);
            strBuild.Append(" ");
            strBuild.AppendFormat("{0:X00}", ControlDomain2);
            strBuild.Append(" ");
            strBuild.AppendFormat("{0:X00}", ControlDomain3);
            strBuild.Append(" ");
            strBuild.AppendFormat("{0:X00}", ControlDomain4);
            strBuild.Append(" ");
            return strBuild.ToString();
           
        }

        /// <summary>
        /// 对信息进行分割
        /// </summary>
        /// <param name="flag">true--对照分割，false--详细分割</param>
        /// <returns>信息字符串</returns>
        public  string ToString(bool flag)
        {
            if (flag)
            {
                StringBuilder strBuild = new StringBuilder(100);
                strBuild.Append("APCITypeI,");
                strBuild.AppendFormat("APDU长度:[{0:X00}]={0:00},",APDULength, APDULength);
                strBuild.AppendFormat("发送序列号:[{0:X00} {1:X00}}={2:00}", ControlDomain2,ControlDomain1, TransmitSequenceNumber);
                strBuild.AppendFormat("接收序列号:[{0:X00} {1:X00}}={2:00}", ControlDomain4, ControlDomain3, ReceiveSequenceNumber);
                  return strBuild.ToString();
            }
            else
            {
                StringBuilder strBuild = new StringBuilder(45);
                strBuild.AppendFormat("APDU长度:{0:00},", APDULength);
                strBuild.AppendFormat("发送序列号:{0:00},", TransmitSequenceNumber);
                strBuild.AppendFormat("发送序列号:{0:00}", ReceiveSequenceNumber);
                return strBuild.ToString();
            }
        }



    }
}
