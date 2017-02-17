using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.FileSever
{
    /// <summary>
    /// 读文件激活
    /// </summary>
    public class FileReadActivityPacket
    {
        /// <summary>
        /// 文件操作标识
        /// </summary>
        public OperatSign OperationSign;

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
        public string Name;

        /// <summary>
        /// 包数据
        /// </summary>
        private byte[] packetData;

     
        /// <summary>
        /// 获取报数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetPacketData()
        {
            int len = 2 + NameLen;
            packetData = new byte[len];
            packetData[0] = (byte)OperationSign;
            packetData[1] = (byte)NameLen;
            var nameStr = UnicodeEncoding.ASCII.GetBytes(Name);
            Array.Copy(nameStr, 0, packetData, 2, NameLen);
            return packetData;
        }

        /// <summary>
        /// 读文件激活
        /// </summary>
        /// <param name="name">文件名</param>
        public FileReadActivityPacket(string name)
        {
            OperationSign = OperatSign.ReadFileActivity;
            Name = name;

        }
    }
}
