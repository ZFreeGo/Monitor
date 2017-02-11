using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr
{
    
    /// <summary>
    /// 文件属性
    /// </summary>
    public class FileAttribute
    {

           /// <summary>
        /// 获取文件名称长度
        /// </summary>
        public byte NameLen
        {
            get
            {
                return (byte)Name.Length;
            }
        }


        /// <summary>
        /// 文件名
        /// </summary>
        public string Name;

        /// <summary>
        /// 文件大小
        /// </summary>
        public UInt32 Size;

        /// <summary>
        /// 文件属性
        /// </summary>
        public byte Property;



        /// <summary>
        /// 文件时间
        /// </summary>
        public CP56Time2a Time;

        /// <summary>
        /// 属性数据包
        /// </summary>
        private byte[] attributeData;


        /// <summary>
        /// 获取数据数组
        /// </summary>
        /// <returns>数组</returns>
        public byte[] GetArrayData()
        {
            var data = new byte[NameLen + 13];
            data[0] = NameLen;
            var nameByte = UnicodeEncoding.ASCII.GetBytes(Name);
            Array.Copy(nameByte, 0, data, 1, NameLen);
            data[NameLen + 1] = ElementTool.GetBit7_0(Size);
            data[NameLen + 2] = ElementTool.GetBit15_8(Size);
            data[NameLen + 3] = ElementTool.GetBit23_16(Size);
            data[NameLen + 4] = ElementTool.GetBit31_24(Size);
            data[NameLen + 5] = Property;
            Array.Copy(Time.GetDataArray(), 0, data, NameLen + 6, 7);
            attributeData = data;
            return data;

        }

        /// <summary>
        /// 设置文件属性
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="size">文件大小</param>
        /// <param name="time">时间</param>
        public FileAttribute(string name, UInt32 size, CP56Time2a time)
        {
            SetFileAttribute(name, size, time);
        }
        /// <summary>
        /// 由数据包初始化数据
        /// </summary>
        /// <param name="packetData">数据包数据</param>
        public FileAttribute(byte[] packetData, int offset, int len)
        {
            SetFileAttribute(packetData, offset, len);
        }
        /// <summary>
        /// 设置文件属性
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="size">文件大小</param>
        /// <param name="time">时间</param>
        public void SetFileAttribute(string name, UInt32 size, CP56Time2a time)
        {
            Name = name;
            Size = size;
            Time = time;
            Property = 0;
        }


        /// <summary>
        /// 由数据包设置数据包
        /// </summary>
        /// <param name="packetData">数据包数据</param>
        public void SetFileAttribute(byte[] packetData, int offset, int len)
        {           
            byte defineLen = (byte)(packetData[offset] + 13) ;
            if (defineLen !=   len)
            {
                throw new ArgumentException("文件属性,定义长度与实际长度不匹配");
            }

            attributeData = new byte[len];
            Array.Copy(packetData, offset, attributeData, 0, len);
            AttributeDataToElement();

  
        }

        /// <summary>
        /// 将数据局数组转换成对应元素属性
        /// </summary>
        private void AttributeDataToElement()
        {
           
            Name = UnicodeEncoding.ASCII.GetString(attributeData, 1, attributeData[0]);
            Size = ElementTool.CombinationByte(attributeData[NameLen + 1],
                attributeData[NameLen + 2], attributeData[NameLen + 3],
                attributeData[NameLen + 4]);
            Property = attributeData[NameLen + 5];
            Time = new CP56Time2a(attributeData, NameLen + 6);
                
        }

    }
}
