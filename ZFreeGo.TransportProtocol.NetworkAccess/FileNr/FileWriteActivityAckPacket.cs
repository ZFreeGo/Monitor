using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr
{
    /// <summary>
    /// 写文件激活确认数据包
    /// </summary>
    class FileWriteActivityAckPacket
    {
        /// <summary>
        /// 文件操作标识
        /// </summary>
        public OperatSign OperationSign
        {
            get;
            private set;
        }

        /// <summary>
        /// 写文件激活结果
        /// </summary>
        public WriteFileResultDescription Result
        {
            get;
            private set;
        }
    
        /// <summary>
        /// 获取文件长度
        /// </summary>
        public byte NameLen
        {
            get
            {
                return (byte)Name.Length;
            }
        }

      
    

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 文件标志
        /// </summary>
        public UInt32 FileID
        {
            get;
            private set;
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public UInt32 Size
        {
            get;
            set;
        }

        /// <summary>
        /// 报数据
        /// </summary>
        private byte[] pakcketData;


        /// <summary>
        /// 写文件数据包
        /// </summary>
        /// <returns>文件数据包</returns>
        public byte[] GetPakcketData()
        {
            int len = NameLen + 11;     
            pakcketData = new byte[len];
            int index = 0;
            pakcketData[index++] = (byte)OperationSign;
            pakcketData[index++] = (byte)Result;
            pakcketData[index++] = NameLen;
            var nameStr = UnicodeEncoding.ASCII.GetBytes(Name);
            Array.Copy(nameStr, 0, pakcketData, index, NameLen);
            index += NameLen;
            pakcketData[index++] = ElementTool.GetBit7_0(FileID);
            pakcketData[index++] = ElementTool.GetBit15_8(FileID);
            pakcketData[index++] = ElementTool.GetBit23_16(FileID);
            pakcketData[index++] = ElementTool.GetBit31_24(FileID);
            pakcketData[index++] = ElementTool.GetBit7_0(Size);
            pakcketData[index++] = ElementTool.GetBit15_8(Size);
            pakcketData[index++] = ElementTool.GetBit23_16(Size);
            pakcketData[index++] = ElementTool.GetBit31_24(Size);
            return pakcketData;
        }

        /// <summary>
        /// 写文件激活确认数据包
        /// </summary>
        /// <param name="data">数据包</param>
        /// <param name="offset">偏移</param>
        /// <param name="len">数据长度</param>
        public  FileWriteActivityAckPacket(byte[] data, byte offset, byte len)
        {
            try
            {
                int definelen = data[offset + 2] + 11;
                if (definelen != len)
                {
                    throw new ArgumentException("写文件激活,定义长度与实际长度不一致");
                }
                pakcketData = new byte[len];
                Array.Copy(data, offset, pakcketData,0, len);
                OperationSign = (OperatSign)data[offset++]; 
                Result = (WriteFileResultDescription)data[offset++];
                int strLen = data[offset++];
                Name = UnicodeEncoding.ASCII.GetString(data, offset, strLen);
                offset += NameLen;
                FileID = ElementTool.CombinationByte(data[offset++], data[offset++],
                    data[offset++], data[offset++]);
                Size = ElementTool.CombinationByte(data[offset++], data[offset++],
                    data[offset++], data[offset++]);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

   
    }
}
