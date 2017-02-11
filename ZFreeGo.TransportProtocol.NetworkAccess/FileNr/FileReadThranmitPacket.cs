using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr
{
    /// <summary>
    /// 读文件数据传输包
    /// </summary>
    public class FileReadThranmitPacket
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
        /// 文件标识
        /// </summary>
        public UInt32 FileID
        {
            get;
            private set;
        }
        /// <summary>
        /// 数据段号
        /// </summary>
        public UInt32 FragmentNum
        {
            get;
            private set;
        }

        /// <summary>
        /// 有无后续标志
        /// </summary>
        public FllowingFlag Fllow
        {
            get;
            private set;
        }

        /// <summary>
        /// 文件数据
        /// </summary>
        public byte[] FileData
        {
            get;
            private set;
        }

        /// <summary>
        /// 单字节模和运算
        /// </summary>
        public byte Check
        {
            get;
            private set;
        }

        /// <summary>
        /// 读文件数据传输包
        /// </summary>
        /// <param name="data">数据包</param>
        /// <param name="offset">偏移</param>
        /// <param name="len">长度</param>
        public FileReadThranmitPacket(byte[] data, byte offset, byte len)
        {
            OperationSign = (OperatSign)data[offset++];
            FileID = ElementTool.CombinationByte(data[offset++], data[offset++],
                data[offset++], data[offset++]);
            FragmentNum = ElementTool.CombinationByte(data[offset++], data[offset++],
                data[offset++], data[offset++]);
            Fllow = (FllowingFlag)data[offset++];
            FileData = new byte[len - 11];
            Array.Copy(data, offset, FileData, offset, len - 11);
            Check = data[offset + len - 11];
        }

    }
}
