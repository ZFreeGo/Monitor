using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ApplicationMessage
{
    /// <summary>
    /// 应用规约数据单元（ APDU）,针对MasterCommand
    /// </summary>
    public class MasterCommand 
    {
        /// <summary>
        /// 通用APCI数据
        /// </summary>
        public APCITypeI APCI;

        /// <summary>
        /// 通用ASDU数据
        /// </summary>
        public ApplicationServiceDataUnit ASDU;


        /// <summary>
        /// 建立时间戳
        /// </summary>
        public DateTime TimeStamp
        {
            get;
            set;
        }
       
        /// <summary>
        /// APDU数组
        /// </summary>
        public byte[] FrameArray;


        /// <summary>
        /// 获取APDU数据
        /// </summary>
        /// <returns>6字节数组</returns>
        public byte[] GetAPDUDataArray()
        {
            var data = new byte[APCI.APDULength + 2];
            Array.Copy(APCI.GetAPCIDataArray(), data, APCI.Length);
            Array.Copy(ASDU.GetASDUDataArray(), 0, data, APCI.Length, ASDU.Length);
            FrameArray = data;
            return data;


        }



        /// <summary>
        /// MasterCommand APDU初始化，用于主站系统 召唤命令
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="asduPublicAddress">ASDU公共地址</param>
        /// <param name="qoi">召唤限定词</param>
        public MasterCommand(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum, TypeIdentification typeID,
            CauseOfTransmissionList cot, UInt16 asduPublicAddress,  QualifyOfInterrogationList qoi)
        {
            ASDU = new ApplicationServiceDataUnit( (byte)typeID, (byte)1, false, (byte)cot, asduPublicAddress);
            //信息对象地址为0
            ASDU.InformationObject[0] = 0;
            ASDU.InformationObject[1] = 0;
            ASDU.InformationObject[2] = 0;
            //召唤限定词
            ASDU.InformationObject[3] = (byte)qoi;            

            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;
           
        }

        /// <summary>
        /// MasterCommand APDU初始化，用于时钟同步
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="asduPublicAddress">ASDU公共地址</param>
        /// <param name="time">时间标识</param>
        public MasterCommand(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum, TypeIdentification typeID,
            CauseOfTransmissionList cot, UInt16 asduPublicAddress, CP56Time2a time)
        {
            ASDU = new ApplicationServiceDataUnit((byte)typeID, (byte)1, false, (byte)cot, asduPublicAddress);
            //信息对象地址为0
            ASDU.InformationObject[0] = 0;
            ASDU.InformationObject[1] = 0;
            ASDU.InformationObject[2] = 0;

            Array.Copy(time.GetDataArray(), 0, ASDU.InformationObject, 3, 7);

            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;

        }

        /// <summary>
        /// MasterCommand APDU初始化，复位进程命令
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="asduPublicAddress">ASDU公共地址</param>
        /// <param name="qrp">复位进程命令限定词 QRP</param>
        public MasterCommand(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum, TypeIdentification typeID,
            CauseOfTransmissionList cot, UInt16 asduPublicAddress,  QualifyResetProgressList qrp )
        {
            ASDU = new ApplicationServiceDataUnit((byte)typeID, (byte)1, false, (byte)cot, asduPublicAddress);
            //信息对象地址为0
            ASDU.InformationObject[0] = 0;
            ASDU.InformationObject[1] = 0;
            ASDU.InformationObject[2] = 0;    
            ASDU.InformationObject[3] = (byte)qrp;

            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;

        }
        /// <summary>
        /// MasterCommand APDU初始化结束命令
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="asduPublicAddress">ASDU公共地址</param>
        /// <param name="objectAddress">信息对象地址</param>
        /// <param name="coi">初始化原因coi</param>
        public MasterCommand(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum, TypeIdentification typeID,
            CauseOfTransmissionList cot, UInt16 asduPublicAddress, UInt32 objectAddress, CauseOfInitialization coi)
        {
            ASDU = new ApplicationServiceDataUnit((byte)typeID, (byte)1, false, (byte)cot, asduPublicAddress);
            //信息对象地址为0
            ASDU.InformationObject[0] = ElementTool.GetBit7_0(objectAddress);
            ASDU.InformationObject[1] = ElementTool.GetBit15_8(objectAddress);
            ASDU.InformationObject[2] = ElementTool.GetBit23_16(objectAddress);
            ASDU.InformationObject[3] = (byte)coi;

            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;

        }
        /// <summary>
        /// MasterCommand 遥控单/双命令
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="asduPublicAddress">ASDU公共地址</param>
        /// <param name="objectAddress">信息对象地址</param>
        /// <param name="sco">单命令 SCO</param>
        public MasterCommand(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum, TypeIdentification typeID,
            CauseOfTransmissionList cot, UInt16 asduPublicAddress, UInt32 objectAddress, byte co)
        {
            ASDU = new ApplicationServiceDataUnit((byte)typeID, (byte)1, false, (byte)cot, asduPublicAddress);
            //信息对象地址为0
            ASDU.InformationObject[0] = ElementTool.GetBit7_0(objectAddress);
            ASDU.InformationObject[1] = ElementTool.GetBit15_8(objectAddress);
            ASDU.InformationObject[2] = ElementTool.GetBit23_16(objectAddress);
            ASDU.InformationObject[3] = co;

            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;

        }
        /// <summary>
        /// MasterCommand 命令 不含信息体
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="asduPublicAddress">ASDU公共地址</param>
        /// <param name="objectAddress">信息对象地址</param>
        public MasterCommand(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum, TypeIdentification typeID,
            CauseOfTransmissionList cot, UInt16 asduPublicAddress)
        {
            ASDU = new ApplicationServiceDataUnit((byte)typeID, (byte)1, false, (byte)cot, asduPublicAddress);
            //信息对象地址为0
            ASDU.InformationObject[0] = 0;
            ASDU.InformationObject[1] = 0;
            ASDU.InformationObject[2] = 0;
            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;

        }
        /// <summary>
        /// MasterCommand初始化，将字节数组强制转化为MasterCommand
        /// </summary>
        /// <param name="dataArray"></param>
        public MasterCommand(byte[] dataArray)
        {
            if (dataArray.Length < 15)
            {
                throw new Exception("MasterCommand APDU(byte[] dataArray) 长度不应该小于15");
            }
            TimeStamp = DateTime.Now;
            APCI = new APCITypeI(dataArray);
            var data = new byte[dataArray.Length - 6];
            Array.Copy(dataArray, 6, data, 0, dataArray.Length - 6);
            FrameArray = dataArray;
            ASDU = new ApplicationServiceDataUnit(data);

            TimeStamp = DateTime.Now;
        }


    }
}
