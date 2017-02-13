using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileSever
{
    /// <summary>
    /// 文件包管理器,用于拆分或组装文件。
    /// </summary>
    public class FilePacketManager
    {

        private FileAttribute mAttribute;
        /// <summary>
        /// 获取文件属性
        /// </summary>
        public FileAttribute Attribute
        {
            get
            {
                return mAttribute;
            }
        }

        /// <summary>
        /// 文件数据传输包集合
        /// </summary>
        private List<FileDataThransmitPacket> listPacket;


        /// <summary>
        /// 获取文件数据集合
        /// </summary>
        public List<FileDataThransmitPacket> PacketCollect
        {
            get
            {
                return listPacket;
            }
        }


        /// <summary>
        /// 获取当前包数据 
        /// </summary>
        public FileDataThransmitPacket CurrentPacket
        {
            get
            {
               if (listPacket.Count != 0)
               {
                   return listPacket[listPacket.Count - 1];
               }
               else
               {
                   return null;
               }
            }
        }

        /// <summary>
        /// 文件数据完整标志
        /// </summary>
        private bool fullFlag;
        public bool FullFlag
        {
            get
            {
                return fullFlag;
            }
        }

        /// <summary>
        /// 数据超过size范围标志
        /// </summary>
        private bool overFlag;

        /// <summary>
        /// 获取数据是否溢出标志
        /// </summary>
        public bool OverFlag
        {
            get
            {
                return overFlag;
            }
        }

            


        /// <summary>
        /// 加入的字节数量
        /// </summary>
        private UInt32 addCount;


        /// <summary>
        /// 接收大小
        /// </summary>
        public UInt32 ReciveSize
        {
            get
            {
                return addCount;
            }
        }


      

        /// <summary>
        /// 文件包初始化
        /// </summary>
        /// <param name="attribute">文件属性</param>
        public FilePacketManager(FileAttribute attribute)
        {
            listPacket = new List<FileDataThransmitPacket>();
            mAttribute = attribute;
            fullFlag = false;
            addCount = 0;
        }

        /// <summary>
        /// 文件包初始化--每次文件传输最大按200字节计算
        /// </summary>
        /// <param name="attribute">文件属性</param>
        /// <param name="fileData">文件数据</param>
        public FilePacketManager(FileAttribute attribute, byte[] fileData)
            : this(attribute)
        {       
            try
            {
                if(fileData.Length == 0)
                {
                    throw new ArgumentException("打包数据长度不能为0");
                }
                int max = 200;
                int count = fileData.Length / max;
                int remain = fileData.Length % max;
                for (int i =0; i< count; i++)
                {
                    UInt32 frage = (UInt32)(i * max);
                    var packet = new FileDataThransmitPacket(OperatSign.WriteFileThransmit, mAttribute.ID,
                        frage, FllowingFlag.Exist, fileData, (int)frage, max);
                    listPacket.Add(packet);
                    addCount = (UInt32)(addCount + max);
                }
                if (remain != 0)
                {
                     UInt32 frage = (UInt32)(count * max);
                    var packet = new FileDataThransmitPacket(OperatSign.WriteFileThransmit, mAttribute.ID,
                        frage, FllowingFlag.Nothing, fileData, (int)frage, remain);
                    addCount = (UInt32)(addCount + remain);
                }
                else
                {
                    CurrentPacket.Fllow = FllowingFlag.Nothing;
                }
                fullFlag = true;

            }
            catch(Exception ex)
            {
                throw ex;
            }        

        }
        /// <summary>
        /// 添加数据包到列表
        /// </summary>
        /// <param name="packet">数据包</param>
        private void AddPacketToList(FileDataThransmitPacket packet)
        {
            listPacket.Add(packet);
            addCount = (UInt32)(addCount + packet.FileData.Length);
            if (addCount > mAttribute.Size)
            {
                overFlag = true;
            }
            if (packet.Fllow == FllowingFlag.Nothing)
            {
                fullFlag = true;
            }

        }


        /// <summary>
        /// 添加数据包--用于接收数据
        /// </summary>
        /// <param name="packet">校验后的数据包</param>
        /// <returns>FileTransmitDescription 代码</returns>
        public FileTransmitDescription AddPacketData(FileDataThransmitPacket packet)
        {
            try
            {
                if (fullFlag)
                {
                    return FileTransmitDescription.UnknowError;
                }

                if (!packet.CheckSame())
                {
                    return FileTransmitDescription.CheckError;
                }


                if (mAttribute.ID != packet.FileID)//检测ID是否一致
                {
                    return FileTransmitDescription.DifferentID;
                }

                if (CurrentPacket != null)
                {
                    if (CurrentPacket.NextFragmentNum != packet.FragmentNum)//检测段号是否连续
                    {
                        return FileTransmitDescription.DifferentFileLen;
                    }
                    if (CurrentPacket.Fllow != FllowingFlag.Exist) //检测上次是否已满
                    {
                        return FileTransmitDescription.UnknowError;
                    }
                    else
                    {
                        AddPacketToList(packet);

                        return FileTransmitDescription.Success;
                    }
                    
                }
                else
                {
                    AddPacketToList(packet);
                    return FileTransmitDescription.Success;
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }




    }

 
}
