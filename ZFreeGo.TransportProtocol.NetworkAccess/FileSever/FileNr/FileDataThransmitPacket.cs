using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransportProtocol.NetworkAccess.Helper;

namespace ZFreeGo.TransportProtocol.NetworkAccess.FileSever
{
    /// <summary>
    /// 读/写文件数据传输包
    /// </summary>
    public class FileDataThransmitPacket
    {
        /// <summary>
        /// 文件操作标识
        /// </summary>
        public OperatSign OperationSign
        {
            get;
            set;
        }
        /// <summary>
        /// 文件标识
        /// </summary>
        public UInt32 FileID
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置数据段号
        /// </summary>
        public UInt32 FragmentNum
        {
            get;
            set;
        }

        /// <summary>
        ///获取或设置有无后续标志
        /// </summary>
        public FllowingFlag Fllow
        {
            get;
            set;
        }

        /// <summary>
        /// 文件数据
        /// </summary>
        public byte[] FileData
        {
            get;
            set;
        }

        /// <summary>
        /// 单字节模和运算
        /// </summary>
        public byte Check
        {
            get;
            set;
        }
        /// <summary>
        /// 下次可能的数据段号
        /// </summary>
        public UInt32 NextFragmentNum
        {
            get
            {
                return (UInt32)(FragmentNum + FileData.Length);
            }
        }

        /// <summary>
        /// 加测文件数据与校验和是否一致
        /// </summary>
        /// <returns>一致返回true，否则返回false</returns>
        public bool CheckSame()
        {
            if (GetCheck(FileData) == Check)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 报数据
        /// </summary>
        private byte[] packetData;


        /// <summary>
        /// 获取报数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetPacketData()
        {
            try
            {
                int index = 0;
                packetData = new byte[FileData.Length + 11];
                packetData[index++] = (byte)OperationSign;
                packetData[index++] = ElementTool.GetBit7_0(FileID);
                packetData[index++] = ElementTool.GetBit15_8(FileID);
                packetData[index++] = ElementTool.GetBit23_16(FileID);
                packetData[index++] = ElementTool.GetBit31_24(FileID);
                packetData[index++] = ElementTool.GetBit7_0(FragmentNum);
                packetData[index++] = ElementTool.GetBit15_8(FragmentNum);
                packetData[index++] = ElementTool.GetBit23_16(FragmentNum);
                packetData[index++] = ElementTool.GetBit31_24(FragmentNum);
                packetData[index++] = (byte)Fllow;
                Array.Copy(FileData, 0, packetData, index, FileData.Length);
                packetData[packetData.Length - 1] = GetCheck(FileData);

                return packetData;
            
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取模和校验码
        /// </summary>
        /// <param name="data">检验数据</param>
        /// <returns>校验和</returns>
        private byte GetCheck(byte[] data)
        {
            byte sum = 0;
            foreach (var m in data)
            {
                sum += m;
            }
            return sum;
        }



        /// <summary>
        /// 文件数据传输包初始化
        /// </summary>
        /// <param name="data">数据包</param>
        /// <param name="offset">偏移</param>
        /// <param name="len">长度</param>
        public FileDataThransmitPacket(byte[] data, byte offset, byte len)
        {
            try
            {
                packetData = new byte[len];
                Array.Copy(data, offset, packetData, 0, len);

                OperationSign = (OperatSign)data[offset++];
                FileID = ElementTool.CombinationByte(data[offset++], data[offset++],
                    data[offset++], data[offset++]);
                FragmentNum = ElementTool.CombinationByte(data[offset++], data[offset++],
                    data[offset++], data[offset++]);
                Fllow = (FllowingFlag)data[offset++];
                FileData = new byte[len - 11];
                Array.Copy(packetData, 10, FileData, 0, len - 11);

                Check = packetData[packetData.Length - 1];

            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }
        /// <summary>
        /// 文件数据传输包
        /// </summary>
        /// <param name="sign">读写操作标识</param>
        /// <param name="id">文件ID</param>
        /// <param name="num">数据段号</param>
        /// <param name="fllow">后续标志</param>
        /// <param name="fileData">包好文件数据的数组</param>
        /// <param name="offset">数据偏移</param>
        /// <param name="len">数据长度</param>
        public FileDataThransmitPacket(OperatSign sign,UInt32 id,UInt32 num,
            FllowingFlag fllow, byte[] fileData,int offset, int len)
        {
            try
            {
                OperationSign = sign;
                FileID = id;
                FragmentNum = num;
                Fllow = fllow;
                FileData = new byte[len];
                Array.Copy(fileData, offset, FileData, 0, len);
                Check = GetCheck(FileData);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
