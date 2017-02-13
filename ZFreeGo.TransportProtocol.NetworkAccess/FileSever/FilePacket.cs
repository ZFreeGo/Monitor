using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileSever
{
    /// <summary>
    /// 文件服务
    /// </summary>
    public class FilePacket
    {
         // <summary>
        /// 通用APCI数据
        /// </summary>
        public APCITypeI APCI;

        /// <summary>
        /// 通用ASDU数据
        /// </summary>
        public ApplicationServiceDataUnit ASDU;


        /// <summary>
        /// 建立时间戳
        /// </summary>
        public DateTime TimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// 发送序列号
        /// </summary>
        public UInt16 TransmitSequenceNumber
        {
            get
            {
                return APCI.TransmitSequenceNumber;
            }
            set
            {
                APCI.TransmitSequenceNumber = value;
            }
        }

        /// <summary>
        /// 接收序列号
        /// </summary>
        public UInt16 ReceiveSequenceNumber
        {
            get
            {
                return APCI.ReceiveSequenceNumber;
            }
            set
            {
                APCI.ReceiveSequenceNumber = value;
            }
        }



        /// <summary>
        /// 获取APDU数组
        /// </summary>
        public byte[] FrameArray;


        public byte[] packetData; 
        /// <summary>
        /// 获取包数据
        /// </summary>
        public byte[] PacketData
        {
            get
            {
                return packetData;
            }
            private set
            {
                packetData = value;
            }
        }

        /// <summary>
        /// 获取APDU数据
        /// </summary>
        /// <returns>6字节数组</returns>
        public byte[] GetAPDUDataArray()
        {
            var data = new byte[APCI.APDULength + 2];
            Array.Copy(APCI.GetAPCIDataArray(), data, APCI.Length);
            Array.Copy(ASDU.GetASDUDataArray(), 0, data, APCI.Length, ASDU.Length);
            FrameArray = data;
            return data;
        }
        /// <summary>
        /// 文件服务
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="receiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="packet">包数据</param>
        public FilePacket(UInt16 transmitSeqNum, UInt16 receiveSeqNum,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, 
         byte[] packet)
        {
            try
            {               
                ASDU = new ApplicationServiceDataUnit((byte)TypeIdentification.F_FR_NA_1_NR, 
                    (byte)cot, ASDUPublicAddress, packet.Length + 3);

                ASDU.InformationObject[0] = 0;
                ASDU.InformationObject[1] = 0;
                ASDU.InformationObject[1] = 2;//文件传输
                Array.Copy(packet, 0, ASDU.InformationObject, 3, packet.Length);
               
                var apduLen = 4 + (packet.Length + 3 + 6); //控制域长度4 + ASDU长度
                APCI = new APCITypeI((byte)apduLen, transmitSeqNum, receiveSeqNum);
                PacketData = packet;
                TimeStamp = DateTime.Now;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 使用数组进行文件数据包初始化
        /// </summary>
        /// <param name="dataArray">数据数组</param>
        public FilePacket(byte[] dataArray)
        {
            try
            {
                TimeStamp = DateTime.Now;

                if (dataArray.Length < 15)
                {
                    throw new Exception("FilePacket APDU(byte[] dataArray) 长度不应该小于15");
                }
                TimeStamp = DateTime.Now;
                APCI = new APCITypeI(dataArray);
                var data = new byte[dataArray.Length - 6];
                Array.Copy(dataArray, 6, data, 0, dataArray.Length - 6);
                FrameArray = dataArray;
                ASDU = new ApplicationServiceDataUnit(data);
                PacketData = new byte[ASDU.InformationObject.Length - 3];
                Array.Copy(ASDU.InformationObject, 3, PacketData, 0, PacketData.Length);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
