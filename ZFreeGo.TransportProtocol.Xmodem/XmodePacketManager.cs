using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.Xmodem
{
    /// <summary>
    /// Xmode数据包管理器
    /// </summary>
    public class XmodePacketManager
    {
        /// <summary>
        /// 包数据列表
        /// </summary>
        private List<XmodePacket> packetList;

        /// <summary>
        /// 数据包管理器初始化
        /// </summary>
        /// <param name="data">需要打包的数据</param>
        /// <param name="len">数据长度</param>
        /// <param name="startHeader">开始字符</param>
        public XmodePacketManager(byte[] data, int len, XmodeStartHeader startHeader)
        {
            int validDataLen = 128;
            if (XmodeStartHeader.SOH == startHeader)
            {
                validDataLen = 128;
            }
            else if (XmodeStartHeader.STX == startHeader)
            {
                validDataLen = 1024;
            }
            else
            {
                throw new Exception("XmodePacketManager:错误头长度");
            }

           

            if (data.Length < len)
            {
                throw new Exception("XmodePacketManager:数据长度小于所需长度");
            }

            if (len > 255 * validDataLen)
            {
                throw new Exception("XmodePacketManager:数据长度超过一次传输的最大长度");
            }
            packetList = new List<XmodePacket>();

            int fullLen = len / validDataLen;
            int remainLen = len % validDataLen;
            
            for (int i = 0; i < fullLen; i++)
            {
                var array = new byte[validDataLen];
                Array.Copy(data, i * validDataLen, array, 0, validDataLen);
                var packet = new XmodePacket(startHeader, (byte)(i + 1), data, validDataLen);
                packetList.Add(packet);
            }
            if (remainLen != 0)
            {
                var dataArray = new byte[remainLen];
                Array.Copy(data, fullLen * validDataLen, dataArray, 0, remainLen);
                var packet = new XmodePacket(startHeader, (byte)(fullLen + 1), data, remainLen);
                packetList.Add(packet);
            }
        }


        /// <summary>
        /// CRC16计算
        /// </summary>
        /// <param name="ptr">数组</param>
        /// <param name="count">数据长度</param>
        /// <returns>16bit crc计算值</returns>
        public int CalCRC16(byte[] ptr, int count)
        {
            int crc = 0;
            int i = 0;
            int j = 0;
            while (--count >= 0)
            {
                crc = crc ^ (int)ptr[j++] << 8;
                for (i = 0; i < 8; ++i)
                {
                    if ((crc & 0x8000) == 0x8000)
                    {
                        crc = crc << 1 ^ 0x1021;
                    }
                    else
                    {
                        crc = crc << 1;
                    }
                }
            }
            return (crc & 0xFFFF);
        }

        /// <summary>
        /// 计算累加和
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="count">数据长度</param>
        /// <returns>16bit累加和</returns>
        public ushort CalSum16(byte[] data, int count)
        {
            ushort sum = 0;
            for (int i = 0; i < count; i++)
            {
                sum += data[i];
            }
            return sum;
        }
    }
}
