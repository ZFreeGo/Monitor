using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.Frame;


namespace ZFreeGo.TransmissionProtocols.ControlSystemCommand
{

    /// <summary>
    /// 控制方向的系统命令ASDU，召唤，时钟，电能脉冲，复位进程
    /// </summary>
    public class ControlCommandASDU : ApplicationServiceDataUnit
    {
      /// <summary>
        /// ControlCommandASDU初始化，用于主站系统 召唤命令
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="cot">传输原因</param>
        /// <param name="asduPublicAddress">ASDU公共地址</param>
        /// <param name="qoi">召唤限定词</param>
        public ControlCommandASDU( TypeIdentification typeID, CauseOfTransmissionList cot, UInt16 asduPublicAddress,
            QualifyOfInterrogationList qoi) : base((byte)typeID, (byte)1, false, (byte)cot, asduPublicAddress)
        {           
            //信息对象地址为0
            InformationObject[0] = 0;
            InformationObject[1] = 0;
            InformationObject[2] = 0;
            //召唤限定词
            InformationObject[3] = (byte)qoi;           
        }

        /// <summary>
        /// MasterCommand APDU初始化，用于时钟同步
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="cot">传输原因</param>
        /// <param name="time">时间标识</param>
        public ControlCommandASDU(TypeIdentification typeID, CauseOfTransmissionList cot, UInt16 asduPublicAddress, CP56Time2a time)
            : base((byte)typeID, (byte)1, false, (byte)cot, asduPublicAddress)
        {
            InformationObject[0] = 0;
            InformationObject[1] = 0;
            InformationObject[2] = 0;
            Array.Copy(time.GetDataArray(), 0, InformationObject, 3, 7);
        }

        /// <summary>
        ///ControlCommandASDU初始化，用于电能脉冲
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="cot">传输原因</param>
        /// <param name="asduPublicAddress">ASDU公共地址</param>
        /// <param name="sco">单命令 SCO</param>
        public ControlCommandASDU(TypeIdentification typeID,
            CauseOfTransmissionList cot, UInt16 asduPublicAddress, QualifyCalculateCommad qcc)
            : base((byte)typeID, (byte)1, false, (byte)cot, asduPublicAddress)
        {
            
            //信息对象地址为0
            InformationObject[0] = 0;
            InformationObject[1] = 0;
            InformationObject[2] = 0;
            InformationObject[3] = qcc.QCC;
        }



        /// <summary>
        /// MasterCommand APDU初始化，复位进程命令
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="cot">传输原因</param>
        /// <param name="asduPublicAddress">ASDU公共地址</param>
        /// <param name="qrp">复位进程命令限定词 QRP</param>
        public ControlCommandASDU(TypeIdentification typeID,
            CauseOfTransmissionList cot, UInt16 asduPublicAddress,  QualifyResetProgressList qrp)
            : base((byte)typeID, (byte)1, false, (byte)cot, asduPublicAddress)
        {

            //信息对象地址为0
            InformationObject[0] = 0;
            InformationObject[1] = 0;
            InformationObject[2] = 0;
            InformationObject[3] = (byte)qrp;
        }



    }
}
