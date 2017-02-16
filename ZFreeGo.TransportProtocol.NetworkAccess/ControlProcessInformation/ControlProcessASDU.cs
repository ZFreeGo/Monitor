using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ControlProcessInformation
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


    }
}
