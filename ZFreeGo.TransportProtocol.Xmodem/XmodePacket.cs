using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.Xmodem
{
    /// <summary>
    /// Xmode 帧定义
    /// </summary>
    public class XmodePacket
    {
        /// <summary>
        /// 数据包有效数据长度
        /// </summary>
        private readonly int validDataLen;

        /// <summary>
        /// Xmode 包数据
        /// </summary>
        private byte[] packetData;

        /// <summary>
        /// 获取帧开始字符
        /// </summary>
        public XmodeStartHeader StartOfHearder
        {
            get
            {
                return (XmodeStartHeader)packetData[0];
            }
        }

        /// <summary>
        /// 获取包序号
        /// </summary>
        public byte PacketNumber
        {
            get
            {
                return packetData[1];
            }
        }

    

        /// <summary>
        /// 获取数据包中的数据,包含头和尾
        /// </summary>
        public byte[] PacketData
        {
            get
            {
                return  packetData;
            }
        }

        /// <summary>
        /// 获取有效数据部分的副本
        /// </summary>
        public byte[] ValidData
        {
            get
            {
                byte[] data = new byte[validDataLen];
                Array.Copy(packetData, 3, data, 0, validDataLen);
                return data;
            }
        }
        /// <summary>
        /// 获取包长度
        /// </summary>
        public int PacketLen
        {
            get
            {
                return validDataLen + 5;
            }
        }
        /// <summary>
        /// 获取包有效数据长度
        /// </summary>
        public int ValidDataLen
        {
            get
            {
                return validDataLen ;
            }
        }

        /// <summary>
        /// 添加校验
        /// </summary>
        /// <param name="checkResult">校验数据</param>
        public void AddCheck(ushort checkResult)
        {
            byte checkH = (byte)(checkResult >> 8);
            byte checkL = (byte)(checkResult & 0x00FF);
            packetData[PacketLen - 2] = checkH;
            packetData[PacketLen - 1] = checkL;

        }

        /// <summary>
        /// Xmode数据包初始化
        /// </summary>
        /// <param name="startHeader">起始字符,决定Xmode类型</param>
        /// <param name="packetNum">包序号</param>
        /// <param name="validData">有效数据，不足长度将自动填充,超过部分丢弃</param>
        /// <param name="len">有效数据数据长度</param>
        public XmodePacket(XmodeStartHeader startHeader, byte packetNum, byte[] validData, int len)
        {
            try
            {
                if (validData.Length < len)
                {
                    throw new Exception("XmodePacket：有效数据实际长度小于所定义长度");
                }


                if (XmodeStartHeader.SOH == startHeader)
                {
                    validDataLen = 128;
                }
                else
                {
                    validDataLen = 1024;
                }

                packetData = new byte[PacketLen];

                packetData[0] = (byte)startHeader;
                packetData[1] = packetNum;
                packetData[2] = (byte)(packetNum^0xFF); //取反

                if (len >= validDataLen) //大于等于则直接填充有效长度
                {
                    Array.Copy(validData, 0, packetData, 3, validDataLen);
                }
                else
                {
                    //小于部分填充数据包
                    if (len == 0)
                    {
                        throw new Exception("XmodePacket：有效数据长度不能为0");
                    }
                    Array.Copy(validData, 0, packetData, 3, len);

                    for (int i = len ; i < validDataLen; i++)
                    {
                        packetData[3 + i] = (byte)XmodeDefine.EOF;
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
