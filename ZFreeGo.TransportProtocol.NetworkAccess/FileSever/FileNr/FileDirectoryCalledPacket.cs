using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.Helper;


namespace ZFreeGo.TransmissionProtocols.FileSever
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
        public byte DirectoryNameLen
        {
            get 
            {
                return (byte)DirectoryName.Length;
            }
       
        }

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

            if (DirectoryName.Length > 200)
            {
                throw new ArgumentException("字符过长");
            }
            //21+x 
            int len = 21 + DirectoryNameLen;
            int index = 0;
            packetData = new byte[len];
            packetData[index++] = (byte)OperationSign;
            packetData[index++] = ElementTool.GetBit7_0(DirectoryID);
            packetData[index++] = ElementTool.GetBit15_8(DirectoryID);
            packetData[index++] = ElementTool.GetBit23_16(DirectoryID);
            packetData[index++] = ElementTool.GetBit31_24(DirectoryID);
            packetData[index++] = DirectoryNameLen;
            var name = UnicodeEncoding.ASCII.GetBytes(DirectoryName);
            Array.Copy(name, 0, packetData, index, name.Length);
            int offset = index + name.Length;
            var start = QueryStartingTime.GetDataArray();
            var end = QueryEndTime.GetDataArray();
            Array.Copy(start, 0, packetData, offset, 7);
            offset += 7;
            Array.Copy(end, 0, packetData, offset, 7);
            return packetData;
        }

        /// <summary>
        /// 文件目录初始化
        /// </summary>
        /// <param name="id">目录ID</param>
        /// <param name="name">目录名称</param>
        /// <param name="flag">召唤标志</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        public FileDirectoryCalledPacket(UInt32 id, string name, FileCallFlag flag,
            CP56Time2a startTime,CP56Time2a endTime)
        {
            OperationSign = OperatSign.ReadDirectory;
            DirectoryID = id;
            DirectoryName = name;
            CallFlag = flag;
            QueryStartingTime = startTime;
            QueryEndTime = endTime;
        }
    }
}
