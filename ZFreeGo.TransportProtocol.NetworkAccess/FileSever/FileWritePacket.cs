using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocols.BasicElement;


namespace ZFreeGo.TransmissionProtocols.FileSever
{
    /// <summary>
    /// 文件服务基于ID=210
    /// </summary>
    public class FileWritePacket : FilePacket
    {

        /// <summary>
        /// 文件服务--写文件激活
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="receiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="writeAcitvityPacket">写文件激活包数据</param>
        public FileWritePacket(UInt16 transmitSeqNum, UInt16 receiveSeqNum,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress,
         FileReadActivityPacket writeAcitvityPacket)
            : base(transmitSeqNum, receiveSeqNum, cot, ASDUPublicAddress, writeAcitvityPacket.GetPacketData())
        {

        }


        /// <summary>
        /// 文件服务--写文件数据传输
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="receiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="transmitDataPacket">传输包数据</param>
        public FileWritePacket(UInt16 transmitSeqNum, UInt16 receiveSeqNum,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress,
         FileDataThransmitPacket transmitDataPacket)
            : base(transmitSeqNum, receiveSeqNum, cot, ASDUPublicAddress, transmitDataPacket.GetPacketData())
        {

        }

    }
}
