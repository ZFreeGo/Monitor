using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocols.Helper;


namespace ZFreeGo.TransmissionProtocols.FileSever
{
    /// <summary>
    /// 读文件激活确认
    /// </summary>
    public class FileReadActivityAckPacket
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
        /// 结果描述符
        /// </summary>
        public FileResultSign ResultSign
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
            private set;
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
            private set;
        }


        /// <summary>
        /// 读数据文件激活确认
        /// </summary>
        /// <param name="data">数据包</param>
        /// <param name="offset">偏移</param>
        /// <param name="len">数据长度</param>
        public  FileReadActivityAckPacket(byte[] data, byte offset, byte len)
        {
            try
            {
                int definelen = data[offset + 2] + 11;
                if (definelen != len)
                {
                    throw new ArgumentException("读文件激活确认,定义长度与实际长度不一致");
                }
                OperationSign = (OperatSign)data[offset++];
                ResultSign = (FileResultSign)data[offset++];
                int strLen = data[offset++];
                Name = UnicodeEncoding.ASCII.GetString(data, offset, strLen);
                offset += NameLen;
                FileID = ElementTool.CombinationByte(data[offset++], data[offset++],
                    data[offset++], data[offset++]);
                Size = ElementTool.CombinationByte(data[offset++], data[offset++],
                    data[offset++], data[offset++]);

            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
