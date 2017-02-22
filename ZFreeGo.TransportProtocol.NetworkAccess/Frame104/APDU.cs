using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.Frame;
using ZFreeGo.TransmissionProtocols.Helper;

namespace ZFreeGo.TransmissionProtocols.Frame104
{

    /// <summary>
    /// 应用规约数据单元（ APDU）
    /// </summary>
    public class APDU
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
        /// 针对遥信信息，以此添加信息对象，单点，双点信息,非序列化
        /// </summary>
        /// <param name="telesignalisationAddress">遥信地址</param>
        /// <param name="qds">带品质描述单点或双点信息</param>
        public bool AddInformationObject(UInt32 telesignalisationAddress, byte qds)
        {

            bool state = false;
            if (ASDU.IsSequence == true)
            {
                return state;
            }
            var data = new byte[3 + 1];
            data[0] = ElementTool.GetBit7_0(telesignalisationAddress);
            data[1] = ElementTool.GetBit15_8(telesignalisationAddress);
            data[2] = ElementTool.GetBit23_16(telesignalisationAddress);
            data[3] = qds; 
            ASDU.AddInformationObject(data, 4);

            return true;
        }

        /// <summary>
        /// 针对遥信信息，以此添加信息对象，单点，双点信息,非序列化
        /// </summary>
        /// <param name="telesignalisationAddress">遥信地址</param>
        /// <param name="qds">带品质描述单点或双点信息</param>
        public bool AddInformationObject(UInt32 telesignalisationAddress,byte qds, CP56Time2a time)
        {

            bool state = false;
            if (ASDU.IsSequence == true)
            {
                return state;
            }
            var data = new byte[3 + 1 + time.GetDataArray().Length];//地址3 + 品质描述1 + 时间戳7
            data[0] = ElementTool.GetBit7_0(telesignalisationAddress);
            data[1] = ElementTool.GetBit15_8(telesignalisationAddress);
            data[2] = ElementTool.GetBit23_16(telesignalisationAddress);
            data[3] = qds;
            Array.Copy(time.GetDataArray(), 0, data, 4, time.GetDataArray().Length);

            ASDU.AddInformationObject(data, (byte)(4 + time.GetDataArray().Length));

            return true;
        }
        /// <summary>
        /// 针对遥信信息，以此添加信息对象，单点，双点信息,序列化
        /// </summary>
        /// <param name="qds">品质描述</param>
        public bool AddInformationObject( byte qds)
        {

            bool state = false;
            if (ASDU.IsSequence == false)
            {
                return state;
            }
            var data = new byte[1];

            data[0] = qds;
            ASDU.AddInformationObject(data, 1);
            return true;
        }
        
       
        /// <summary>
        /// 针对遥信信息，以此添加信息对象，单点，双点信息,序列化
        /// </summary>
        /// <param name="telesignalisationAddress">遥信地址</param>
        /// <param name="qds">品质描述</param>
        public bool AddInformationObject(byte qds, CP56Time2a time)
        {

            bool state = false;
            if (ASDU.IsSequence == false)
            {
                return state;
            }
            var data = new byte[1 + time.GetDataArray().Length];// 品质描述1 + 时间戳7
            data[0] = qds;
            Array.Copy(time.GetDataArray(), 0, data, 1, time.GetDataArray().Length);
            ASDU.AddInformationObject(data, 4);

            return true;
        }

        /// <summary>
        /// 针对遥测信息，,非序列化
        /// </summary>
        /// <param name="Address">遥测地址</param>
        /// <param name="nve">归一化值</param>
        public bool AddInformationObject(UInt32 address,  NormalizationValue nve)
        {

            bool state = false;
            if (ASDU.IsSequence == true)
            {
                return state;
            }
            var data = new byte[3 + nve.GetDataArray().Length];
            data[0] = ElementTool.GetBit7_0(address);
            data[1] = ElementTool.GetBit15_8(address);
            data[2] = ElementTool.GetBit23_16(address);
            Array.Copy(nve.GetDataArray(), 0, data, 3, nve.GetDataArray().Length);
            ASDU.AddInformationObject(data, (byte)data.Length);

            return true;
        }
        /// <summary>
        /// 添加信息对象，非序列化，浮点数+地址+品质描述词
        /// </summary>
        /// <param name="Address">遥测地址</param>
        /// <param name="sf">浮点数</param>
        public bool AddInformationObject(UInt32 address, ShortFloating sf)
        {

            bool state = false;
            if (ASDU.IsSequence == true)
            {
                return state;
            }
            var data = new byte[3 + sf.GetDataArray().Length];
            data[0] = ElementTool.GetBit7_0(address);
            data[1] = ElementTool.GetBit15_8(address);
            data[2] = ElementTool.GetBit23_16(address);
            Array.Copy(sf.GetDataArray(), 0, data, 3, sf.GetDataArray().Length);
            ASDU.AddInformationObject(data, (byte)data.Length);

            return true;
        }
        /// <summary>
        /// 针对遥测信息，,非序列化
        /// </summary>
        /// <param name="Address">遥测地址</param>
        /// <param name="nve">归一化值</param>
        /// <param name="time">时间信标</param>
        public bool AddInformationObject(UInt32 address, NormalizationValue nve, CP56Time2a time)
        {

            bool state = false;
            if (ASDU.IsSequence == true)
            {
                return state;
            }
            var data = new byte[3 + nve.GetDataArray().Length + time.GetDataArray().Length];
            data[0] = ElementTool.GetBit7_0(address);
            data[1] = ElementTool.GetBit15_8(address);
            data[2] = ElementTool.GetBit23_16(address);
            Array.Copy(nve.GetDataArray(), 0, data, 3, nve.GetDataArray().Length);
            Array.Copy(time.GetDataArray(), 0, data, 3 + nve.GetDataArray().Length, time.GetDataArray().Length);
            ASDU.AddInformationObject(data, (byte)data.Length);

            return true;
        }
        /// <summary>
        /// 针对遥测信息，,非序列化
        /// </summary>
        /// <param name="Address">遥测地址</param>
        /// <param name="sf">浮点数</param>
        /// <param name="time">时间信标</param>
        public bool AddInformationObject(UInt32 address, ShortFloating sf, CP56Time2a time)
        {

            bool state = false;
            if (ASDU.IsSequence == true)
            {
                return state;
            }
            var data = new byte[3 + sf.GetDataArray().Length];
            data[0] = ElementTool.GetBit7_0(address);
            data[1] = ElementTool.GetBit15_8(address);
            data[2] = ElementTool.GetBit23_16(address);
            Array.Copy(sf.GetDataArray(), 0, data, 3, sf.GetDataArray().Length);
            Array.Copy(time.GetDataArray(), 0, data, 3 + sf.GetDataArray().Length, time.GetDataArray().Length);
            ASDU.AddInformationObject(data, (byte)data.Length);

            return true;
        }
        /// <summary>
        /// 针对遥测信息，,序列化
        /// </summary>
        /// <param name="nve">归一化值</param>
        public bool AddInformationObject( NormalizationValue nve)
        {
            bool state = false;
            if (ASDU.IsSequence == true)
            {
                return state;
            }
            ASDU.AddInformationObject(nve.GetDataArray(), (byte)nve.GetDataArray().Length);

            return true;
        }
        /// <summary>
        /// 添加序列化的信息对象：短浮点数(byte4)+品质描述词(byte1)
        /// <summary>
        /// <param name="sf">浮点数</param>
        /// <param name="qds">品质描述词</param>
        public bool AddInformationObject( ShortFloating sf, QualityDescription qds)
        {                    
            var data = new byte[5];
            Array.Copy(sf.GetDataArray(), data, 4);
            data[4] = qds.QDS;

            ASDU.AddInformationObject(data, 5);

            return true;
        }
        /// <summary>
        /// 添加序列化的信息对象：短浮点数(byte4)+品质描述词(byte1) 
        /// <summary>
        /// <param name="sf">浮点数</param>
        /// <param name="qds">品质描述词</param>
        /// <param name="position">添加的位置</param>
        public bool AddInformationObject(ShortFloating sf, QualityDescription qds,byte position)
        {
          
            var data = new byte[5];
            Array.Copy(sf.GetDataArray(), data, 4);
            data[4] = qds.QDS;
            ASDU.AddInformationObject(data, 5, position);

            return true;
        }
        
       /// <summary>
       /// 添加序列化的信息对象：双元素
       /// </summary>
       /// <param name="data">字节数据数组</param>
       /// <param name="len">数组长度</param>
       /// <param name="dataB">字节数组B</param>
        /// <param name="dataBlen">数组长度B</param>
       /// <param name="position">位置</param>
       /// <returns></returns>
        public bool AddInformationObject(byte[] data, byte len, byte[] dataB, byte dataBlen, byte position)
        {

            var aray = new byte[len + dataBlen];
            data.CopyTo(aray, 0);

            Array.Copy(dataB, 0, aray, len, dataBlen);

            ASDU.AddInformationObject(aray, (byte)(len + dataBlen), position);

            return true;
        }
        /// <summary>
        /// 添加序列化的信息对象：单元素
        /// </summary>
        /// <param name="data">字节数据数组</param>
        /// <param name="len">数组长度</param>
        /// <param name="position">位置</param>
        /// <returns></returns>
        public bool AddInformationObject(byte[] data, byte len, byte position)
        {

            var aray = new byte[len];
            data.CopyTo(aray, 0);         

            ASDU.AddInformationObject(aray, (byte)(len ), position);

            return true;
        }
        /// <summary>
        /// 针对遥测信息，,序列化
        /// </summary>
        /// <param name="nve">归一化值</param>
        /// <param name="time">时间信标</param>
        public bool AddInformationObject(NormalizationValue nve, CP56Time2a time)
        {

            bool state = false;
            if (ASDU.IsSequence == true)
            {
                return state;
            }
            var data = new byte[nve.GetDataArray().Length + time.GetDataArray().Length];
            Array.Copy(nve.GetDataArray(), 0, data, 0, nve.GetDataArray().Length);
            Array.Copy(time.GetDataArray(), 0 + nve.GetDataArray().Length, data, nve.GetDataArray().Length, time.GetDataArray().Length);

            ASDU.AddInformationObject(data, (byte)data.Length);

            return true;
        }
        /// <summary>
        /// 针对遥测信息，序列化
        /// <summary>
        /// <param name="sf">浮点数</param>
        /// <param name="time">时间信标</param>
        public bool AddInformationObject(ShortFloating sf, CP56Time2a time)
        {
            bool state = false;
            if (ASDU.IsSequence == true)
            {
                return state;
            }
            var data = new byte[sf.GetDataArray().Length + time.GetDataArray().Length];
            Array.Copy(sf.GetDataArray(), 0, data, 0, sf.GetDataArray().Length);
            Array.Copy(time.GetDataArray(), 0 + sf.GetDataArray().Length, data, sf.GetDataArray().Length, time.GetDataArray().Length);

            ASDU.AddInformationObject(data, (byte)data.Length);
            return true;
        }


        /// <summary>
        /// 获信息对象，对象数组列表
        /// </summary>
        /// <returns>对象数组列表</returns>
        public List<byte[]> GetObjectListObject()
        {

            List<byte[]> list = new List<byte[]>();
            int len = ASDU.GetSingleInformationObjectLenth();
            int offset = 0;
            if (ASDU.IsSequence)
            {
                offset = 3;               
            }
            else
            {
                offset = 0;
            }
            
            for (int i = 0; i < ASDU.InformationObjectCount; i++)
            {
                var data = new byte[len];
                Array.Copy(ASDU.InformationObject, i * len + offset, data, 0, len);
                list.Add(data);
            }
            return list;
        }


        /// <summary>
        /// 获取信息对象列表
        /// </summary>
        /// <returns>信息元素 元组中以此为序号，信息， 时标(可选)</returns>
        public List<Tuple<UInt32, object, object>> GetInformationList()
        {
            try
            {

                var list = new List<Tuple<UInt32, object, object>>();
                switch ((TypeIdentification)ASDU.TypeId)
                {
                    //遥信信息
                    case TypeIdentification.M_SP_NA_1://单点信息
                    case TypeIdentification.M_DP_NA_1://双点信息
                        {

                            if (ASDU.IsSequence)
                            {
                               // int len = 1;
                                var addr = ElementTool.CombinationByte(ASDU.InformationObject[0], ASDU.InformationObject[1], ASDU.InformationObject[2]);
                                for (int i = 0; i < ASDU.InformationObjectCount; i++)
                                {

                                    var m =  ASDU.InformationObject[3 + i];
                                    list.Add(new Tuple<UInt32, object, object>((UInt32)(addr + i), m, null));
                                }
                            }
                            else
                            {
                                int len = 4;
                                for (int i = 0; i < ASDU.InformationObjectCount; i++)
                                {
                                    var addr1 = ElementTool.CombinationByte(ASDU.InformationObject[0 + len * i],
                                        ASDU.InformationObject[1 + len * i], ASDU.InformationObject[2  +len * i]);
                                    var m = ASDU.InformationObject[3 + len * i];

                                    list.Add(new Tuple<UInt32, object, object>(addr1, m, null));
                                }
                            }
                            break;
                        }
                   
                    case TypeIdentification.M_SP_TB_1://带CP56Time2a时标的单点信息
                    case TypeIdentification.M_DP_TB_1://带CP56Time2a时标的双点信息
                        {
                            if (ASDU.IsSequence)
                            {
                                int len = 1 + 7;
                                var addr = ElementTool.CombinationByte(ASDU.InformationObject[0], ASDU.InformationObject[1], ASDU.InformationObject[2]);
                                for (int i = 0; i < ASDU.InformationObjectCount; i++)
                                {

                                    var m = ASDU.InformationObject[3 + i * len];
                                    var data = new byte[7];
                                    Array.Copy(ASDU.InformationObject, i * len + 4, data, 0, 7);
                                    var t = new CP56Time2a(data);
                                    list.Add(new Tuple<UInt32, object, object>((UInt32)(addr + i), m, t));
                                }
                            }
                            else
                            {
                                int len = 3 + 1 + 7;
                                for (int i = 0; i < ASDU.InformationObjectCount; i++)
                                {
                                    var addr1 = ElementTool.CombinationByte(ASDU.InformationObject[0 + len * i],
                                        ASDU.InformationObject[1 + 8 * i], ASDU.InformationObject[2 + +len * i]);

                                    var m = ASDU.InformationObject[3 + len * i];
                                    var data = new byte[7];
                                    Array.Copy(ASDU.InformationObject, i * len + 4, data, 0, 7);
                                    var t = new CP56Time2a(data);

                                    list.Add(new Tuple<UInt32, object, object>(addr1, m, t));
                                }
                            }
                            break;
                        }
                    
                    //遥测信息
                    case TypeIdentification.M_ME_NA_1://测量值，归一化值
                        {
                            break;
                        }
                    case TypeIdentification.M_ME_NC_1://测量值，短浮点数
                    case TypeIdentification.M_IT_NA_1: //累积量
                        {
                            if (ASDU.IsSequence)
                            {
                                int len = 4 + 1;
                                var addr = ElementTool.CombinationByte(ASDU.InformationObject[0], ASDU.InformationObject[1], ASDU.InformationObject[2]);
                                for (int i = 0; i < ASDU.InformationObjectCount; i++)
                                {
                                    var data = new byte[5];
                                    Array.Copy(ASDU.InformationObject, i * len + 3, data, 0, 5);
                                    var m = new ShortFloating(data);
                                    var q = new QualityDescription(data[4]);
                                    list.Add(new Tuple<UInt32, object, object>((UInt32)(addr + i), m.Value, q));
                                }
                            }
                            else
                            {
                                int len = 12;//4 + 1 + 7
                                for (int i = 0; i < ASDU.InformationObjectCount; i++)
                                {
                                    var addr1 = ElementTool.CombinationByte(ASDU.InformationObject[0 + len * i],
                                        ASDU.InformationObject[1 + len * i], ASDU.InformationObject[2 + +len * i]);
                                    var data = new byte[4];
                                    Array.Copy(ASDU.InformationObject, i * len + 3, data, 0, 4);
                                    var m = new ShortFloating(data);
                                    list.Add(new Tuple<UInt32, object, object>(addr1, m, null));
                                }
                            }
                            break;
                        }
                    
                    case TypeIdentification.M_ME_TD_1://带CP56Time2a时标的测量值，归一化值
                    case TypeIdentification.M_ME_TF_1://带CP56Time2a时标的测量值，短浮点数
                        {
                            break;
                        }
                   
                
                    default:
                        {
                            return null;
                            //break;
                        }
                }

                return list;
            }
            catch (Exception)
            {
                return null;
            }
        }




 
        /// <summary>
        /// APDU初始化
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="typeID">类型ID</param>
        /// <param name="isquense">是否序列号 true-序列化 false-非序列化</param>
        /// <param name="objectCount">信息对象数目数目</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">公共地址</param>
        public APDU(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum, TypeIdentification typeID, bool isquense, byte objectCount,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, UInt32 objectAddress)
        {
            ASDU = new ApplicationServiceDataUnit((byte)typeID, objectCount, isquense, (byte)cot, ASDUPublicAddress);
            //信息对象地址为0
            ASDU.InformationObject[0] = ElementTool.GetBit7_0(objectAddress);
            ASDU.InformationObject[1] = ElementTool.GetBit15_8(objectAddress);
            ASDU.InformationObject[2] = ElementTool.GetBit23_16(objectAddress);            
            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;
        }

        /// <summary>
        /// APDU初始化,设定值命令
        /// </summary>
        /// <param name="transmitSeqNum">发送序列号</param>
        /// <param name="ReceiveSeqNum">接收序列号</param>
        /// <param name="typeID">类型ID</param>
        /// <param name="isquense">是否序列号 true-序列化 false-非序列化</param>
        /// <param name="objectCount">信息对象数目数目</param>
        /// <param name="cot">传输原因</param>
        /// <param name="ASDUPublicAddress">公共地址</param>
        public APDU(UInt16 transmitSeqNum, UInt16 ReceiveSeqNum, TypeIdentification typeID, bool isquense, byte objectCount,
            CauseOfTransmissionList cot, UInt16 ASDUPublicAddress, UInt32 objectAddress, QualifyCommandSet qos)
        {
            ASDU = new ApplicationServiceDataUnit((byte)typeID, objectCount, isquense, (byte)cot, ASDUPublicAddress);
            //信息对象地址为0
            ASDU.InformationObject[0] = ElementTool.GetBit7_0(objectAddress);
            ASDU.InformationObject[1] = ElementTool.GetBit15_8(objectAddress);
            ASDU.InformationObject[2] = ElementTool.GetBit23_16(objectAddress);
            //设置命令限定词，最后一字节
            ASDU.InformationObject[ASDU.InformationObject.Length - 1] = qos.QOS;
            var apduLen = 4 + ASDU.Length; //控制域长度4 + ASDU长度
            APCI = new APCITypeI((byte)apduLen, transmitSeqNum, ReceiveSeqNum);

            TimeStamp = DateTime.Now;
        }
     
        /// <summary>
        /// 使用字节数组初始化，属于强制性转换初始化
        /// </summary>
        /// <param name="dataArray">字节数组</param>
        public APDU(byte[] dataArray)
        {
            if (dataArray.Length < 15)
            {
               throw new Exception("APDU(byte[] dataArray) 长度不应该小于15");
            }
          
            APCI = new APCITypeI(dataArray);
            var data = new byte[dataArray.Length - 6];
            Array.Copy(dataArray, 6, data, 0, dataArray.Length - 6);
            FrameArray = dataArray;
            ASDU = new ApplicationServiceDataUnit(data);

            TimeStamp = DateTime.Now;
        }

        /// <summary>
        /// 使用APCI与ASDU初始化APDU
        /// </summary>
        /// <param name="apci">APCI</param>
        /// <param name="asdu">ASDU</param>
        public APDU(APCITypeI apci, ApplicationServiceDataUnit asdu)
        {
            APCI = apci;
            ASDU = asdu;
            TimeStamp = DateTime.Now;
        }


        /// <summary>
        /// 获取帧格式字符串
        /// </summary>
        public override string ToString()
        {
            var data = GetAPDUDataArray();
            StringBuilder build = new StringBuilder(data.Length * 3 + 10);
            build.Append("[");
            foreach(var m in data)
            {
                build.AppendFormat("{0:X2} ", m);
            }
            build.Append("]");
            return build.ToString();
        }

       /// <summary>
        /// 对信息进行分割
        /// </summary>
        /// <param name="flag">true--对照分割，false--详细分割</param>
        /// <returns>信息字符串</returns>
        public  string ToString(bool flag)
        {
            StringBuilder strBuild = new StringBuilder(APCI.ToString(flag), 40 + ASDU.Length*3);
            strBuild.Append("  ");
            strBuild.Append(ASDU.ToString(flag));
            return strBuild.ToString();
        }
            

    }
}
