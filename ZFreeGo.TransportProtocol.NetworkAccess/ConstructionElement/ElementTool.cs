using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement
{
    public class ElementTool
    {
        public static byte GetBit7_0(UInt32 data)
        {
            return (byte)(data & 0x00000FF);
        }
        public  static byte GetBit15_8(UInt32 data)
        {
            return (byte)((data>>8) & 0x000FF);
        }
        public  static byte GetBit23_16(UInt32 data)
        {
            return (byte)((data >> 16) & 0x000FF);
        }
        public static byte GetBit31_24(UInt32 data)
        {
            return (byte)((data >> 24) & 0x000FF);
        }
        /// <summary>
        /// 将三个字节组合成32bit
        /// </summary>
        /// <param name="byte1">低位第一字节</param>
        /// <param name="byte2">低位第二字节</param>
        /// <param name="byte3">低位第三字节</param>
        /// <returns></returns>
        public static UInt32 CombinationByte(byte byte1, byte byte2, byte byte3)
        {
            return (UInt32)(byte1 + ((UInt32)byte2 << 8) + ((UInt32)byte3 << 16));
        }
        /// <summary>
        /// 将四个字节组合成32bit
        /// </summary>
        /// <param name="byte1">低位第1字节</param>
        /// <param name="byte2">低位第2字节</param>
        /// <param name="byte3">低位第3字节</param>
        /// <param name="byte4">低位第4字节</param>
        /// <returns></returns>
        public static UInt32 CombinationByte(byte byte1, byte byte2, byte byte3, byte byte4)
        {
            return (UInt32)(byte1 + ((UInt32)byte2 << 8) + ((UInt32)byte3 << 16) + ((UInt32)byte3 << 24));
        }
        /// <summary>
        /// 将两个个字节组合成16bit
        /// </summary>
        /// <param name="byte1">低位第一字节</param>
        /// <param name="byte2">低位第二字节</param>
        /// <returns></returns>
        public static UInt16 CombinationByte(byte byte1, byte byte2 )
        {
            return (UInt16)(byte1 + ((UInt16)byte2 << 8));
        }
    
    }
}
