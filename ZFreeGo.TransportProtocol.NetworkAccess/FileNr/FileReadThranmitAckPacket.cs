using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr
{
    /// <summary>
    ///读文件数据传输包确认
    /// </summary>
    public class FileReadThranmitAckPacket
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
        /// 数据段号
        /// </summary>
        public UInt32 FragmentNum
        {
            get;
            set;
        }

        /// <summary>
        /// 有无后续标志
        /// </summary>
        public FllowingFlag Fllow
        {
            get;
            set;
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

        }
       
        /// <summary>
        /// 文件传输包确认
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <param name="num">数据段号</param>
        /// <param name="flag">有无后续标志</param>
        public FileReadThranmitAckPacket(UInt32 id, UInt32 num,FllowingFlag flag)
        {
            OperationSign = OperatSign.ReadFileDataResponseACK;
            FileID = id;
            FragmentNum = num;
            Fllow = flag;
        }
    }
}
