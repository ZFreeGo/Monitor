using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr
{
    /// <summary>
    ///写文件数据传输包确认
    /// </summary>
    public class FileWriteThransmitAckPacket
    {
        

        /// <summary>
        /// 文件操作标识
        /// </summary>
        public OperatSign OperationSign
        {
            get
            {
                return (OperatSign)packetData[0];
            }
            private set
            {
                OperationSign = value;
            }
        }
        /// <summary>
        /// 文件标识
        /// </summary>
        public UInt32 FileID
        {
            get
            {
                int offset = 1;
                var num = ConstructionElement.ElementTool.CombinationByte(packetData[offset++],
                    packetData[offset++], packetData[offset++], packetData[offset++]);
                return num;
            }
            set
            {
                int offset = 1;
                packetData[offset++] = ConstructionElement.ElementTool.GetBit7_0(value);
                packetData[offset++] = ConstructionElement.ElementTool.GetBit15_8(value);
                packetData[offset++] = ConstructionElement.ElementTool.GetBit23_16(value);
                packetData[offset++] = ConstructionElement.ElementTool.GetBit31_24(value);
            }
        }
        /// <summary>
        /// 数据段号
        /// </summary>
        public UInt32 FragmentNum
        {
            get
            {
                int offset = 5;
                var num = ConstructionElement.ElementTool.CombinationByte(packetData[offset++],
                    packetData[offset++], packetData[offset++], packetData[offset++]);
                return num;
            }
            set
            {
                int offset = 5;
                packetData[offset++] = ConstructionElement.ElementTool.GetBit7_0(value);
                packetData[offset++] = ConstructionElement.ElementTool.GetBit15_8(value);
                packetData[offset++] = ConstructionElement.ElementTool.GetBit23_16(value);
                packetData[offset++] = ConstructionElement.ElementTool.GetBit31_24(value);
            }
        }

        /// <summary>
        /// 写结果描述
        /// </summary>
        public FileTransmitDescription Result
        {
            get
            {
                return (FileTransmitDescription)packetData[9];
            }
            set
            {
                packetData[9] = (byte)value;
            }
        }

        /// <summary>
        /// 数据包数据
        /// </summary>
        private byte[] packetData;

        /// <summary>
        /// 获取数据包数据
        /// </summary>
        /// <returns>数据包数据数组</returns>
        public byte[] GetPacketData()
        {
            return packetData;
        }
       
        /// <summary>
        /// 写文件传输确认包
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <param name="num">数据段号</param>
        /// <param name="result">传输结果</param>
        public FileWriteThransmitAckPacket(UInt32 id, UInt32 num, FileTransmitDescription result)
        {
            packetData = new byte[10];
            OperationSign = OperatSign.ReadFileDataResponseACK;
            FileID = id;
            FragmentNum = num;
            Result = result;
           
        }

        /// <summary>
        /// 写文件传输确认包
        /// </summary>
        /// <param name="data">数据包</param>
        /// <param name="offset">偏移</param>
        public FileWriteThransmitAckPacket(byte[] data, byte offset)
        {
            packetData = new byte[10];
            Array.Copy(data, offset, packetData, 0, 10);
        }
    }
}
