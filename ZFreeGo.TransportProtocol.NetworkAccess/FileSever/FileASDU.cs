using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.Frame;
using ZFreeGo.TransmissionProtocols.Frame104;


namespace ZFreeGo.TransmissionProtocols.FileSever
{
    /// <summary>
    /// 文件服务ASDU
    /// </summary>
    public class FileASDU : ApplicationServiceDataUnit
    {           
        /// <summary>
        /// 文件服务
        /// </summary>       
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="packet">包数据</param>
        public FileASDU(CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, byte[] packet)
            :  base((byte)TypeIdentification.F_FR_NA_1_NR, 
                    (byte)cot, ASDUPublicAddress, packet.Length + 3)
        {
            try
            {               
                InformationObject[0] = 0;
                InformationObject[1] = 0;
                InformationObject[1] = 2;//文件传输
                Array.Copy(packet, 0, InformationObject, 3, packet.Length);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 文件服务--文件目录召唤
        /// </summary>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="callPacket">召唤包数据</param>
        public  FileASDU(CauseOfTransmissionList cot, UInt16 ASDUPublicAddress,
         FileDirectoryCalledPacket callPacket)
            : this( cot, ASDUPublicAddress, callPacket.GetPacketData())
        {

        }

        /// <summary>
        /// 文件服务--读文件
        /// </summary>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="activityPacket">文件读激活包</param>
        public  FileASDU(CauseOfTransmissionList cot, UInt16 ASDUPublicAddress,
         FileReadActivityPacket activityPacket)
            : this(cot, ASDUPublicAddress, activityPacket.GetPacketData())
        {

        }

        /// <summary>
        /// 文件服务--读文件传输确认
        /// </summary>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="readAckPacket">读文件传输确认包</param>
        public FileASDU( CauseOfTransmissionList cot, UInt16 ASDUPublicAddress,
         FileReadThransmitAckPacket readAckPacket)
            : this( cot, ASDUPublicAddress, readAckPacket.GetPacketData())
        {

        }

        // /// <summary>
        ///// 文件服务--写文件激活
        ///// </summary>
        ///// <param name="cot">传输原因</param>
        ///// <param name="ASDUPublicAddress">ASDU公共地址</param>
        ///// <param name="writeAcitvityPacket">写文件激活包数据</param>
        //public FileASDU( CauseOfTransmissionList cot, UInt16 ASDUPublicAddress,
        // FileReadActivityPacket writeAcitvityPacket)
        //    : this(cot, ASDUPublicAddress, writeAcitvityPacket.GetPacketData())
        //{

        //}


        /// <summary>
        /// 文件服务--写文件数据传输
        /// </summary>  
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="transmitDataPacket">传输包数据</param>
        public FileASDU(CauseOfTransmissionList cot, UInt16 ASDUPublicAddress,
         FileDataThransmitPacket transmitDataPacket)
            :this( cot, ASDUPublicAddress, transmitDataPacket.GetPacketData())
        {

        }
       
    }
}
