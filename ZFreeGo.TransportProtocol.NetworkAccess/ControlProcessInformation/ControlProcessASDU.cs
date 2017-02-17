using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransportProtocol.NetworkAccess.BasicElement;
using ZFreeGo.TransportProtocol.NetworkAccess.Frame;
using ZFreeGo.TransportProtocol.NetworkAccess.Helper;

namespace ZFreeGo.TransportProtocol.NetworkAccess.ControlProcessInformation
{

    /// <summary>
    /// 控制方向的系过程命令， 单点,双点命令
    /// </summary>
    public class ControlProcessASDU: ApplicationServiceDataUnit
    {
        /// <summary>
        /// 控制信息过程命令
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="cot">传输原因</param>
        /// <param name="asduPublicAddress">ASDU公共地址</param>
        /// <param name="objectAddress">信息对象地址</param>
        /// <param name="co">命令 CO</param>
        public ControlProcessASDU( TypeIdentification typeID,  CauseOfTransmissionList cot, UInt16 asduPublicAddress, UInt32 objectAddress, byte co)
            : base((byte)typeID, (byte)1, false, (byte)cot, asduPublicAddress)
        {
            //信息对象地址为0
            InformationObject[0] = ElementTool.GetBit7_0(objectAddress);
            InformationObject[1] = ElementTool.GetBit15_8(objectAddress);
            InformationObject[2] = ElementTool.GetBit23_16(objectAddress);
            InformationObject[3] = co;

        }


        /// <summary>
        /// 控制信息过程命令，双命令
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="cot">传输原因</param>
        /// <param name="asduPublicAddress">ASDU公共地址</param>
        /// <param name="objectAddress">信息对象地址</param>
        /// <param name="dco">双命令</param>
         public ControlProcessASDU( TypeIdentification typeID,  CauseOfTransmissionList cot,
             UInt16 asduPublicAddress, UInt32 objectAddress, DoubleCommand dco)
            : this(typeID, cot, asduPublicAddress, objectAddress, dco.DCO)
        {

        }


         /// <summary>
         /// 控制信息过程命令，单命令
         /// </summary>
         /// <param name="typeID">类型ID</param>
         /// <param name="cot">传输原因</param>
         /// <param name="asduPublicAddress">ASDU公共地址</param>
         /// <param name="objectAddress">信息对象地址</param>
         /// <param name="sco">单命令</param>
         public ControlProcessASDU(TypeIdentification typeID, CauseOfTransmissionList cot,
              UInt16 asduPublicAddress, UInt32 objectAddress, SingleCommand sco)
             : this(typeID, cot, asduPublicAddress, objectAddress, sco.SCO)
         {

         }


         /// <summary>
         /// 控制信息过程命令,设定值命令
         /// </summary>
         /// <param name="typeID">类型ID</param>
         /// <param name="isquense">是否序列号 true-序列化 false-非序列化</param>
         /// <param name="objectCount">信息对象数目数目</param>
         /// <param name="cot">传输原因</param>
         /// <param name="ASDUPublicAddress">公共地址</param>
         /// <param name="qos">设定命令限定词</param>
         /// <param name="listFloat">设定值列表</param>
         public ControlProcessASDU(TypeIdentification typeID, bool isquense, 
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress,  QualifyCommandSet qos, 
             List<Tuple<UInt32,ShortFloating>> listFloat)
             : base((byte)typeID, (byte)listFloat.Count, isquense, (byte)cot, ASDUPublicAddress)
        {            
       
            if (isquense)
            {
                UInt32 addr = listFloat[0].Item1;
                InformationObject[0] = ElementTool.GetBit7_0(addr);
                InformationObject[1] = ElementTool.GetBit15_8(addr);
                InformationObject[2] = ElementTool.GetBit23_16(addr);
                int index = 0;
                foreach(var m in listFloat)
                {
                    Array.Copy(m.Item2.GetDataArray(), 0, InformationObject, 3 + 4 * index, 4);
                    index++;
                }
            }
            else
            {                
                int index = 0;
                foreach (var m in listFloat)
                {
                    UInt32 addr = listFloat[0].Item1;
                    InformationObject[0 + index * 7] = ElementTool.GetBit7_0(addr);
                    InformationObject[1 + index * 7] = ElementTool.GetBit15_8(addr);
                    InformationObject[2 + index * 7] = ElementTool.GetBit23_16(addr);
                    Array.Copy(m.Item2.GetDataArray(), 0, InformationObject, 3 + 7 * index, 4);
                    index++;
                }
            }
             //设置命令限定词，最后一字节
           InformationObject[InformationObject.Length - 1] = qos.QOS;           
        }

    }
}
