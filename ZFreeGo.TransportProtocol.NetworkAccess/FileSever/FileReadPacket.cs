﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.FileElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileSever
{
    /// <summary>
    /// 文件服务基于ID=210
    /// </summary>
    public class FileReadPacket : FilePacket
    {
              


        /// <summary>
        /// 文件服务--文件目录召唤
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="receiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="callPacket">召唤包数据</param>
        public FileReadPacket(UInt16 transmitSeqNum, UInt16 receiveSeqNum,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress,
         FileDirectoryCalledPacket callPacket)
            : base(transmitSeqNum, receiveSeqNum, cot, ASDUPublicAddress, callPacket.GetPacketData())
        {

        }

        /// <summary>
        /// 文件服务--文件目录召唤
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="receiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="activityPacket">文件读激活包</param>
        public FileReadPacket(UInt16 transmitSeqNum, UInt16 receiveSeqNum,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress,
         FileReadActivityPacket activityPacket)
            : base(transmitSeqNum, receiveSeqNum, cot, ASDUPublicAddress, activityPacket.GetPacketData())
        {

        }

        /// <summary>
        /// 文件服务--读文件传输确认
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="receiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="readAckPacket">读文件传输确认包</param>
        public FileReadPacket(UInt16 transmitSeqNum, UInt16 receiveSeqNum,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress,
         FileReadThransmitAckPacket readAckPacket)
            : base(transmitSeqNum, receiveSeqNum, cot, ASDUPublicAddress, readAckPacket.GetPacketData())
        {

        }

        ///// <summary>
        ///// 文件服务--读文件传输确认
        ///// </summary>
        ///// <param name="transmitSeqNum">发送序列号</param>
        ///// <param name="receiveSeqNum">接收序列号</param>
        ///// <param name="cot">传输原因</param>
        ///// <param name="ASDUPublicAddress">ASDU公共地址</param>
        ///// <param name="readAckPacket">读文件传输确认包</param>
        //public FileReadPacket(UInt16 transmitSeqNum, UInt16 receiveSeqNum,
        //    CauseOfTransmissionList cot, UInt16 ASDUPublicAddress,
        // FileReadThransmitAckPacket readAckPacket)
        //    : base(transmitSeqNum, receiveSeqNum, cot, ASDUPublicAddress, readAckPacket.GetPacketData())
        //{

        //}

        
    }
}
