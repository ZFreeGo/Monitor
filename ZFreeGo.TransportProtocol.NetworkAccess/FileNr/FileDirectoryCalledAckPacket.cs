using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr
{
    /// <summary>
    /// 目录召唤确认
    /// </summary>
    public class FileDirectoryCalledAckPacket
    {
        /// <summary>
        /// 文件操作标识
        /// </summary>
        public OperatSign OperationSign;

        /// <summary>
        /// 结果描述符
        /// </summary>
        public FileResultSign ResultSign;

        /// <summary>
        /// 目录ID
        /// </summary>
        public UInt32 DirectoryID
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
        /// 本帧文件数量
        /// </summary>
        public byte Count
        {
            get;
            private set;
        }

        /// <summary>
        /// 文件属性列表
        /// </summary>
        public List<FileAttribute> FileAttributeList
        {
            get;
            private set;
        }


        private byte[] pakcetData;

        /// <summary>
        /// 数据包转换为属性值
        /// </summary>
        /// <param name="data">数据包</param>
        /// <param name="offset">便宜</param>
        /// <param name="len">数据长度</param>
        public FileDirectoryCalledAckPacket(byte[] data, byte offset, byte len)
        {
            try
            {
                FileAttributeList = new List<FileAttribute>();
                PacketDataToElement(data, offset, len);
                pakcetData = new byte[len];
                Array.Copy(data, offset, pakcetData, 0, len);
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 数据包转换为属性值
        /// </summary>
        /// <param name="data">数据包</param>
        /// <param name="offset">便宜</param>
        /// <param name="len">数据长度</param>
        public  void PacketDataToElement(byte[] data, byte offset, byte len)
        {
            OperationSign = (OperatSign)data[offset++];
            ResultSign  = (FileResultSign)data[offset++];
            DirectoryID =  ElementTool.CombinationByte(data[offset++],
                data[offset++],data[offset++],data[offset++]);

            Fllow = (FllowingFlag)data[offset++];
            Count = data[offset++];
            if (Count > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    byte  attributeLen = (byte)(data[offset] + 13);
                    var attribute = new FileAttribute(data, offset, attributeLen);
                    FileAttributeList.Add(attribute);
                    offset += attributeLen;
                }
            }
        }



    }
}
