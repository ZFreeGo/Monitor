using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr
{
    /// <summary>
    /// 召唤文件目录附加数据包
    /// </summary>
    public class FileDirectoryCalledPacket
    {
        /// <summary>
        /// 文件操作标识
        /// </summary>
        public OperatSign OperationSign;

        /// <summary>
        /// 目录ID
        /// </summary>
        public UInt32 DirectoryID;
        /// <summary>
        /// 目录名长度
        /// </summary>
        public byte DirectoryNameLen;

        /// <summary>
        /// 目录名
        /// </summary>
        public string DirectoryName;

        /// <summary>
        /// 文件召唤标志
        /// </summary>
        public FileCallFlag CallFlag;

        /// <summary>
        /// 搜索起始时间
        /// </summary>
        public CP56Time2a QueryStartingTime;

        /// <summary>
        /// 查询终止时间
        /// </summary>
        public CP56Time2a QueryEndTime;

        /// <summary>
        /// 报数据
        /// </summary>
        private byte[] packetData;
        /// <summary>
        /// 获取报数据
        /// </summary>
        /// <returns>字节数组</returns>
        public byte[] GetPacketData()
        {
            DirectoryNameLen = (byte)DirectoryName.Length;
            if (DirectoryNameLen > 200)
            {
                throw new ArgumentException("字符过长");
            }
            //21+x 
            int len = 21 + DirectoryNameLen;
            packetData = new byte[len];
            packetData[0] = (byte)OperationSign;
            packetData[1] = ElementTool.GetBit7_0(DirectoryID);
            packetData[2] = ElementTool.GetBit15_8(DirectoryID);
            packetData[3] = ElementTool.GetBit23_16(DirectoryID);
            packetData[4] = ElementTool.GetBit31_24(DirectoryID);
            packetData[5] = DirectoryNameLen;
            var name = UnicodeEncoding.ASCII.GetBytes(DirectoryName);
            Array.Copy(name, 0, packetData, 6, name.Length);
            int offset = 6 + name.Length;
            var start = QueryStartingTime.GetDataArray();
            var end = QueryEndTime.GetDataArray();
            Array.Copy(start, 0, packetData, offset, 7);
            offset += 7;
            Array.Copy(end, 0, packetData, offset, 7);
            return packetData;
        }
    }
}
