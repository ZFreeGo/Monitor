using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr
{
    /// <summary>
    ///读文件数据传输包确认
    /// </summary>
    public class FileReadThransmitAckPacket
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
        /// 有无后续标志
        /// </summary>
        public FllowingFlag Fllow
        {
            get
            {
                return (FllowingFlag)packetData[9];
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
        /// 文件传输包确认
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <param name="num">数据段号</param>
        /// <param name="flag">有无后续标志</param>
        public FileReadThransmitAckPacket(UInt32 id, UInt32 num,FllowingFlag flag)
        {
            packetData = new byte[10];
            OperationSign = OperatSign.ReadFileDataResponseACK;
            FileID = id;
            FragmentNum = num;
            Fllow = flag;
           
        }
    }
}
