using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransportProtocol.NetworkAccess.Frame;
using ZFreeGo.TransportProtocol.NetworkAccess.Frame104;
using ZFreeGo.TransportProtocol.NetworkAccess.Helper;


namespace ZFreeGo.TransportProtocol.NetworkAccess.BasicElement
{
    /// <summary>
    /// 文件传输服务，基于ID号为120的服务
    /// </summary>
    public class FileTransmit
    {
        // <summary>
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
        /// 发送序列号
        /// </summary>
        public UInt16 TransmitSequenceNumber
        {
            get
            {
                return APCI.TransmitSequenceNumber;
            }
            set
            {
                APCI.TransmitSequenceNumber = value;
            }
        }

        /// <summary>
        /// 接收序列号
        /// </summary>
        public UInt16 ReceiveSequenceNumber
        {
            get
            {
                return APCI.ReceiveSequenceNumber;
            }
            set
            {
                APCI.ReceiveSequenceNumber = value;
            }
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
        /// 文件准备就绪
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="objectAddress">对象地址</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="fileLen">文件长度</param>
        /// <param name="frq">文件准备就绪限定词</param>
        public FileTransmit(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum, 
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, UInt32 objectAddress,
            UInt16 fileName, UInt32 fileLen, FileReadyQulifier frq)
        {
            ASDU = new ApplicationServiceDataUnit((byte)TypeIdentification.F_FR_NA_1, 1, false, (byte)cot, ASDUPublicAddress);

            ASDU.InformationObject[0] = ElementTool.GetBit7_0(objectAddress);
            ASDU.InformationObject[1] = ElementTool.GetBit15_8(objectAddress);
            ASDU.InformationObject[2] = ElementTool.GetBit23_16(objectAddress);
            ASDU.InformationObject[3] = ElementTool.GetBit7_0(fileName);
            ASDU.InformationObject[4] = ElementTool.GetBit15_8(fileName);
            ASDU.InformationObject[5] = ElementTool.GetBit7_0(fileLen);
            ASDU.InformationObject[6] = ElementTool.GetBit15_8(fileLen);
            ASDU.InformationObject[7] = ElementTool.GetBit23_16(fileLen);
            ASDU.InformationObject[8] = frq.FRQ;

            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;

        }
        /// <summary>
        /// 节准备就绪
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="objectAddress">对象地址</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="sectionName">节名称</param>
        /// <param name="sectionLen">节长度</param>
        /// <param name="srq">节准备就绪限定词</param>
        public FileTransmit(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, UInt32 objectAddress,
            UInt16 fileName, byte sectionName, UInt32 sectionLen, SectionReadyQulifier srq)
        {
            ASDU = new ApplicationServiceDataUnit((byte)TypeIdentification.F_SR_NA_1, 1, false, (byte)cot, ASDUPublicAddress);

            ASDU.InformationObject[0] = ElementTool.GetBit7_0(objectAddress);
            ASDU.InformationObject[1] = ElementTool.GetBit15_8(objectAddress);
            ASDU.InformationObject[2] = ElementTool.GetBit23_16(objectAddress);
            ASDU.InformationObject[3] = ElementTool.GetBit7_0(fileName);
            ASDU.InformationObject[4] = ElementTool.GetBit15_8(fileName);
            ASDU.InformationObject[5] = sectionName;
            ASDU.InformationObject[6] = ElementTool.GetBit7_0(sectionLen);
            ASDU.InformationObject[7] = ElementTool.GetBit15_8(sectionLen);
            ASDU.InformationObject[8] = ElementTool.GetBit23_16(sectionLen);
            ASDU.InformationObject[9] = srq.SRQ;

            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;

        }
        /// <summary>
        /// 召唤目录，选择文件，召唤文件，召唤节
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="objectAddress">对象地址</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="sectionName">节名称</param>    
        /// <param name="scq">选择和调用限定词</param>
        public FileTransmit(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, UInt32 objectAddress,
            UInt16 fileName, byte sectionName, SelectCallQulifier scq)
        {
            ASDU = new ApplicationServiceDataUnit((byte)TypeIdentification.F_SC_NA_1, 1, false, (byte)cot, ASDUPublicAddress);

            ASDU.InformationObject[0] = ElementTool.GetBit7_0(objectAddress);
            ASDU.InformationObject[1] = ElementTool.GetBit15_8(objectAddress);
            ASDU.InformationObject[2] = ElementTool.GetBit23_16(objectAddress);
            ASDU.InformationObject[3] = ElementTool.GetBit7_0(fileName);
            ASDU.InformationObject[4] = ElementTool.GetBit15_8(fileName);
            ASDU.InformationObject[5] = sectionName;
            ASDU.InformationObject[6] = scq.SCQ;

            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;

        }
        /// <summary>
        /// 最后的节，最后的段
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="objectAddress">对象地址</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="lsq">最后的节和段的限定词LSQ</param>    
        /// <param name="chs">校验和</param>
        public FileTransmit(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, UInt32 objectAddress,
            UInt16 fileName, LastSectionQulifier lsq, byte chs)
        {
            ASDU = new ApplicationServiceDataUnit((byte)TypeIdentification.F_LS_NA_1, 1, false, (byte)cot, ASDUPublicAddress);

            ASDU.InformationObject[0] = ElementTool.GetBit7_0(objectAddress);
            ASDU.InformationObject[1] = ElementTool.GetBit15_8(objectAddress);
            ASDU.InformationObject[2] = ElementTool.GetBit23_16(objectAddress);
            ASDU.InformationObject[3] = ElementTool.GetBit7_0(fileName);
            ASDU.InformationObject[4] = ElementTool.GetBit15_8(fileName);
            ASDU.InformationObject[5] = (byte)lsq;
            ASDU.InformationObject[6] = chs;

            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;

        }
        /// <summary>
        /// 确认文件，确认节
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="objectAddress">对象地址</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="sectionName">节名称</param>    
        /// <param name="afq">节名字</param>
        public FileTransmit(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, UInt32 objectAddress,
            UInt16 fileName, byte sectionName, AckFileQulifier afq)
        {
            ASDU = new ApplicationServiceDataUnit((byte)TypeIdentification.F_AF_NA_1, 1, false, (byte)cot, ASDUPublicAddress);

            ASDU.InformationObject[0] = ElementTool.GetBit7_0(objectAddress);
            ASDU.InformationObject[1] = ElementTool.GetBit15_8(objectAddress);
            ASDU.InformationObject[2] = ElementTool.GetBit23_16(objectAddress);
            ASDU.InformationObject[3] = ElementTool.GetBit7_0(fileName);
            ASDU.InformationObject[4] = ElementTool.GetBit15_8(fileName);
            ASDU.InformationObject[5] = sectionName;
            ASDU.InformationObject[6] = afq.AFQ;

            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;

        }

        /// <summary>
        /// 段
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="objectAddress">对象地址</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="sectionName">节名称</param>    
        /// <param name="sectionLen">段的长度</param>
        /// <param name="dataArray">段数据数组</param>
        public FileTransmit(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, UInt32 objectAddress,
            UInt16 fileName, byte sectionName, byte sectionLen, byte[] dataArray)
        {
            ASDU = new ApplicationServiceDataUnit((byte)TypeIdentification.F_SG_NA_1, (byte)cot, ASDUPublicAddress,
                 (byte)(sectionLen + 2 + 1 + 1 + 3));

            ASDU.InformationObject[0] = ElementTool.GetBit7_0(objectAddress);
            ASDU.InformationObject[1] = ElementTool.GetBit15_8(objectAddress);
            ASDU.InformationObject[2] = ElementTool.GetBit23_16(objectAddress);
            ASDU.InformationObject[3] = ElementTool.GetBit7_0(fileName);
            ASDU.InformationObject[4] = ElementTool.GetBit15_8(fileName);
            ASDU.InformationObject[5] = sectionName;
            ASDU.InformationObject[6] = sectionLen;

            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;

        }





        /// <summary>
        /// 目录{空白或X，只在监视（标准）方向有效}
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">ASDU公共地址</param>
        /// <param name="objectAddress">对象地址</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="sectionName">节名称</param>    
        /// <param name="afq">节名字</param>
        public FileTransmit(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum, byte informationObjectCount,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, UInt32 objectAddress,
            UInt16 fileName, byte sectionName, AckFileQulifier afq)
        {
            ASDU = new ApplicationServiceDataUnit((byte)TypeIdentification.F_DR_TA_1, informationObjectCount, true, (byte)cot, ASDUPublicAddress);

            ASDU.InformationObject[0] = ElementTool.GetBit7_0(objectAddress);
            ASDU.InformationObject[1] = ElementTool.GetBit15_8(objectAddress);
            ASDU.InformationObject[2] = ElementTool.GetBit23_16(objectAddress);
            
          
            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;

           
        }


    /// <summary>
    /// 查询日志文件
    /// </summary>
    /// <param name="transmitSeqNum">发送序列号</param>
    /// <param name="ReceiveSeqNum">接收序列号</param>
    /// <param name="cot">传输原因</param>
    /// <param name="ASDUPublicAddress">ASDU公共地址</param>
    /// <param name="objectAddress">对象地址</param>
    /// <param name="fileName">文件名称</param>
    /// <param name="startTime">开始时间</param>
    /// <param name="endTime">结束时间</param>
        public FileTransmit(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum, 
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, UInt32 objectAddress,
            UInt16 fileName, CP56Time2a startTime, CP56Time2a endTime)
        {
            ASDU = new ApplicationServiceDataUnit((byte)TypeIdentification.F_SC_NB_1, 1, false, (byte)cot, ASDUPublicAddress);

            ASDU.InformationObject[0] = ElementTool.GetBit7_0(objectAddress);
            ASDU.InformationObject[1] = ElementTool.GetBit15_8(objectAddress);
            ASDU.InformationObject[2] = ElementTool.GetBit23_16(objectAddress);
            ASDU.InformationObject[3] = ElementTool.GetBit7_0(fileName);
            ASDU.InformationObject[4] = ElementTool.GetBit15_8(fileName);
            Array.Copy(startTime.GetDataArray(), 0, ASDU.InformationObject, 5, 7);
            Array.Copy(endTime.GetDataArray(), 0, ASDU.InformationObject, 12, 7);

            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;

        }

        /// <summary>
        /// 添加信息对象--针对目录
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="fileLen">文件长度</param>
        /// <param name="sof">文件状态</param>
        /// <param name="time">时间</param>
        public bool AddInformationObject(UInt16  fileName, UInt32 fileLen, StatusOfFile sof, CP56Time2a time)
        {
            bool state = false;
            if (ASDU.IsSequence == true)
            {
                return state;
            }
            var data = new byte[2 + 3 + 1 + 7];
            data[0] = ElementTool.GetBit7_0(fileName);
            data[1] = ElementTool.GetBit15_8(fileName);
            data[2] = ElementTool.GetBit7_0(fileLen);
            data[3] = ElementTool.GetBit15_8(fileLen);
            data[4] = ElementTool.GetBit23_16(fileLen);
            data[5] = sof.SOF;
            Array.Copy(time.GetDataArray(), 0 , data, 6, 7);

            ASDU.AddInformationObject(data, (byte)data.Length);

            return true;
        }


    }


}
