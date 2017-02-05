using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement
{
    /// <summary>
    /// 应用服务数据单元ApplicationServiceDataUnit
    /// </summary>
    public class ApplicationServiceDataUnit
    {
        private byte typeId;
        /// <summary>
        /// 类型标识TransferReason
        /// </summary>
        public byte TypeId
        {
            set { typeId = value; }
            get { return typeId; }
        }

        /// <summary>
        /// 可变结构限定词
        /// </summary>
        public byte variableStructureQualifier;


        /// <summary>
        /// 可变结构限定词--信息对象数目
        /// </summary>
        public byte InformationObjectCount
        {
            private set { variableStructureQualifier = (byte)(variableStructureQualifier & 0x007F | (value & 0x007F)); }
            get { return (byte)(variableStructureQualifier & 0x007F); }
        }

        /// <summary>
        /// 是否为顺序结构true-序列化 false-非序列化
        /// </summary>
        public bool IsSequence
        {
            private set 
            { 
                if (value)
                {
                    variableStructureQualifier |= 0x0080;
                }
                else
                {
                    variableStructureQualifier = InformationObjectCount ;
                }
            }
            get { return (variableStructureQualifier & 0x0080) == 0x0080; }
        }




        private byte causeOfTransmission1;
        /// <summary>
        /// 传送原因 0bit-7bit 第1字节(共2字节)
        /// </summary>
        public byte CauseOfTransmission1
        {
            set { causeOfTransmission1 = value; }
            get { return causeOfTransmission1; }
        }

        private byte causeOfTransmission2;
        /// <summary>
        /// 传送原因 8bit-16bit 第1字节(共2字节)
        /// </summary>
        public byte CauseOfTransmission2
        {
            set { causeOfTransmission2 = value; }
            get { return causeOfTransmission2; }
        }

        private byte appDataPublicAddress1;
        /// <summary>
        /// 应用服务数据单元公共地址 0bit-7bit 第1字节(共2字节)
        /// </summary>
        public byte AppDataPublicAddress1
        {
            set { appDataPublicAddress1 = value; }
            get { return appDataPublicAddress1; }
        }
        private byte appDataPublicAddress2;
        /// <summary>
        /// 应用服务数据单元公共地址 8bit-15bit 第2字节(共2字节)
        /// </summary>
        public byte AppDataPublicAddress2
        {
            set { appDataPublicAddress2 = value; }
            get { return appDataPublicAddress2; }
        }

      


        /// <summary>
        /// 信息对象存储数组
        /// </summary>
        private byte[] informationObject;
        /// <summary>
        /// 信息对象存储数组
        /// </summary>
        public byte[] InformationObject
        {
            set { informationObject = value; }
            get { return informationObject; }
        }

        public byte[] GetASDUDataArray()
        {
            var data = new byte[Length];
            data[0] = typeId;
            data[1] = variableStructureQualifier;
            data[2] = causeOfTransmission1;
            data[3] = causeOfTransmission2;
            data[4] = appDataPublicAddress1;
            data[5] = AppDataPublicAddress2;

            Array.Copy(InformationObject, 0, data, 6, InformationObject.Length);
            ASDUDataArray = data;
            return data;

        }
        /// <summary>
        /// 原始数组
        /// </summary>
        public byte[] ASDUDataArray;



        /// <summary>
        /// ASDU 长度
        /// </summary>
        public byte Length
        {
            //类型标识符 TI 1 字节
            //可变帧长限定词 VSQ 1 字节
            //传送原因 COT 2 字节
            //ASDU 公共地址 2 字节
            get { return (byte)(1 + 1 + 2 + 2 + GetTotalInformationObjectLenth()); }
        }

        private byte objectCount;
        /// <summary>
        /// 已经添加进入的信息对象数目
        /// </summary>
         public byte ObjectCount
        {
            get { return objectCount; }
            //set { objectCount = value; }
        }

       



        /// <summary>
        /// 添加信息对象
        /// </summary>
        /// <param name="data">数组</param>
        /// <param name="dataLen">数据长度</param>
         public void AddInformationObject(byte[] data, byte dataLen)
         {
             byte realLen = 0;//数据长度 + 地址长度（可选）
             byte offset = 0;
             if (objectCount > InformationObjectCount)
             {
                 throw new Exception("信息对象空间已满");
             }
             if (dataLen > data.Length)
             {
                 throw new Exception("设定长度大于设定长度");
             }
             var typdeLen = GetSingleInformationObjectLenth();

             if (IsSequence)
             {

                 realLen = typdeLen;
                 offset = 3;
             }
             else
             {
                 realLen = (byte)(typdeLen + 3);
                 offset = 0;
             }

             if (realLen != dataLen)
             {
                 throw new Exception("导入数据长度与类型长度不匹配");
             }
             Array.Copy(data, 0, InformationObject, objectCount * realLen + offset, typdeLen + 3);
             objectCount++;


         }
         /// <summary>
         /// 添加信息对象
         /// </summary>
         /// <param name="data">数组</param>
         /// <param name="dataLen">数据长度</param>
         /// <param name="position">添加的位置</param>
         public void AddInformationObject(byte[] data, byte dataLen, byte position)
         {
             byte realLen = 0;//数据长度 + 地址长度（可选）
             byte offset = 0;
             if (objectCount > InformationObjectCount)
             {
                 throw new Exception("信息对象空间已满");
             }
             if (dataLen > data.Length)
             {
                 throw new Exception("设定长度大于设定长度");
             }
             var typdeLen = GetSingleInformationObjectLenth();

             if (IsSequence)
             {

                 realLen = typdeLen;
                 offset = 3;
             }
             else
             {
                 realLen = (byte)(typdeLen + 3);
                 offset = 0;
             }

             if (realLen != dataLen)
             {
                 throw new Exception("导入数据长度与类型长度不匹配");
             }
             Array.Copy(data, 0, InformationObject, position * realLen + offset, realLen);
             objectCount++;

         }

        /// <summary>
        /// 获取信息对象总长度
        /// </summary>
        /// <returns></returns>
        public byte GetTotalInformationObjectLenth()
        {     

             byte totalCount = 0;
             switch ((TypeIdentification)TypeId)
             {

                 case TypeIdentification.C_IC_NA_1:  //总召唤/组召唤
                 case TypeIdentification.C_RP_NA_1:   //复位进程命令
                 case TypeIdentification.M_EI_NA_1:  //初始化结束                  
                 case TypeIdentification.C_CS_NA_1: //时钟同步                    
                 case TypeIdentification.C_SC_NA_1: //单点命令   //遥控信息
                 case TypeIdentification.C_DC_NA_1: //双点命令
                 case TypeIdentification.C_CI_NA_1:  //电能脉冲召唤  
                 case TypeIdentification.C_RR_NA_1: //读当前定值区号
                     {
                         totalCount = (byte)(GetSingleInformationObjectLenth() + 3);//长度加 地址3
                         break;
                     }

                 //遥信信息
                 case TypeIdentification.M_SP_NA_1://单点信息
                 case TypeIdentification.M_DP_NA_1://双点信息
                 case TypeIdentification.M_SP_TB_1://带CP56Time2a时标的单点信息
                 case TypeIdentification.M_DP_TB_1://带CP56Time2a时标的双点信息
                 //遥测信息
                 case TypeIdentification.M_ME_NA_1://测量值，归一化值
                 case TypeIdentification.M_ME_NC_1://测量值，短浮点数
                 case TypeIdentification.M_ME_TD_1://带CP56Time2a时标的测量值，归一化值
                 case TypeIdentification.M_ME_TF_1://带CP56Time2a时标的测量值，短浮点数
                     //文件
                 case TypeIdentification.F_DR_TA_1: //目录{空白或X，只在监视（标准）方向有效}
                 case TypeIdentification.F_FR_NA_1: //文件就绪
                 case TypeIdentification.F_SR_NA_1://节准备就绪
                 case TypeIdentification.F_SC_NA_1: //召唤目录，选择文件，召唤文件，召唤节 
                 case TypeIdentification.F_LS_NA_1: // 最后的节，最后的端，
                 case TypeIdentification.F_AF_NA_1: // 确认文件，确认节
                     
                     //校准
                 case TypeIdentification.P_ME_NC_1://测量值参数，短浮点数
                 case TypeIdentification.P_AC_NA_1://参数激活
                 case TypeIdentification.M_IT_NA_1://累积量

                     {
                         if (IsSequence)
                         {
                             totalCount =
                                 (byte)(GetSingleInformationObjectLenth() * InformationObjectCount + 3);//单个信息对象 * 对象数 + 地址3
                         }
                         else
                         {
                             totalCount =
                                 (byte)((GetSingleInformationObjectLenth() + 3) * InformationObjectCount);//(单个信息对象 + 地址3)* 对象数 
                         }
                         break;
                     }
                 //设定值
                 case TypeIdentification.C_SE_NC_1: //定值命令，短浮点数
                     {
                         if (IsSequence)
                         {
                             totalCount =
                                 (byte)(GetSingleInformationObjectLenth() * InformationObjectCount + 3 + 1);//单个信息对象 * 对象数 + 地址3 + 1(最后一字节命令描述)
                         }
                         else
                         {
                             totalCount =
                                 (byte)((GetSingleInformationObjectLenth() + 3) * InformationObjectCount + 1);//(单个信息对象 + 地址3)* 对象数 + 1(最后一字节命令描述)
                         }
                         break;
                     }
                 default:
                     {
                         throw new Exception("获取信息对象总长度,TypeID不是所识别的");
                     }
             }
             return totalCount;


         


        }

       


        /// <summary>
        /// 获取单个信息对象长度,不含地址
        /// </summary>
        /// <returns></returns>
        public byte GetSingleInformationObjectLenth()
        {
            byte singleCount = 0;
            switch  ((TypeIdentification)TypeId)
            {

 

                case TypeIdentification.C_IC_NA_1:  //总召唤/组召唤
                case TypeIdentification.C_RP_NA_1:   //复位进程命令
                case TypeIdentification.M_EI_NA_1:  //初始化结束  
                case TypeIdentification.C_CI_NA_1: //电能脉冲召唤
                    {
                        singleCount = 1; //限定词1
                        break;
                    }
                case TypeIdentification.C_CS_NA_1: //时钟同步
                    {

                        singleCount = 7; // 时标7
                        break;
                    }
                //测试命令
                case TypeIdentification.C_TS_TA_1://带CP56Time2a时标的测试命令
                    {
                        singleCount = 1 + 7; //TSC 1 + 时标7
                        break;
                    }
                    //遥信信息
                case TypeIdentification.M_SP_NA_1://单点信息
                case TypeIdentification.M_DP_NA_1://双点信息
                case TypeIdentification.C_RC_NA_1://步调节信息
                    {
                        singleCount = 1; //品质描述词(QDS) 1
                        break;
                    }
                case TypeIdentification.M_SP_TB_1://带CP56Time2a时标的单点信息
                case TypeIdentification.M_DP_TB_1://带CP56Time2a时标的双点信息
                    {
                        singleCount = 8; //品质描述词(QDS) 1 + 时标7
                        break;
                    }
                    //遥测信息
                case TypeIdentification.M_ME_NA_1://测量值，归一化值
                    {
                        singleCount = 3; //归一化值 NVA 2  + 品质描述词(QDS) 1 
                        break;
                    }
                case TypeIdentification.M_ME_NC_1://测量值，短浮点数
                    {
                        singleCount = 5; //短浮点数 4  + 品质描述词(QDS) 1 
                        break;
                    }
                case TypeIdentification.M_ME_TD_1://带CP56Time2a时标的测量值，归一化值
                    {
                        singleCount = 10; //归一化值 NVA 2  + 品质描述词(QDS) 1 + 时标7
                        break;
                    }
                case TypeIdentification.M_ME_TF_1://带CP56Time2a时标的测量值，短浮点数
                    {
                        singleCount = 12; //短浮点数 4  + 品质描述词(QDS) 1 + 时标7
                        break;
                    }
                    //遥控信息
                case TypeIdentification.C_SC_NA_1: //单点命令
                case TypeIdentification.C_DC_NA_1: //双点命令
                    {
                        singleCount = 1; //单命令 SCO 1
                        break;
                    }
                    //设定值
                case TypeIdentification.C_SE_TA_1://带CP56Time2a时标的设定值命令，归一化值
                case TypeIdentification.C_SE_TB_1://带CP56Time2a时标的设定值命令，标度化值
                    {
                        singleCount = 2  + 7; //VALUE + S/VALUE + S/E/QL + 时标7
                        break;
                    }
                case TypeIdentification.C_SE_TC_1://带CP56Time2a时标的设定值命令，短浮点数
                    {
                        singleCount = 4 + 1 + 7;// 短浮点数 + 1 + 7
                        break;
                    }
                case TypeIdentification.C_BO_TA_1://带CP56Time2a时标的32比特串
                    {
                        singleCount = 4 + 7;// 比特串4 + 时标7
                        break;
                    }
                    //定值命令
                case TypeIdentification.C_SE_NC_1: //定值命令，短浮点数
                    {
                        singleCount = 4 ;// 短浮点数4 
                        break;
                    }
                //校准
                case TypeIdentification.P_ME_NC_1://测量值参数，短浮点数
                    {
                        singleCount = 4 + 1;//短浮点数4 + 测量值参数限定值1
                        break;
                    }
                case TypeIdentification.P_AC_NA_1://参数激活
                    {
                        singleCount = 1;//参数激活限定词
                        break;
                    }
                case TypeIdentification.M_IT_NA_1://累积量
                    {
                        singleCount = 4 + 1;//累积量
                        break;
                    }

                //文件命令
                case TypeIdentification.F_FR_NA_1: //文件已经准备好
                    {
                        singleCount = 2 + 3 + 1; ////文件名2   + 文件节长度3 + FRQ文件准备就绪限定词
                        break;
                    }
                case TypeIdentification.F_SR_NA_1: //节已经准备好
                    {
                        singleCount = 2 + 1+ 3 + 1;//文件名2  + 节名称1 + 节长度3 + SRQ节准备就绪限定词
                        break;
                    }
                case TypeIdentification.F_SC_NA_1: //召唤目录，选择文件，召唤文件，召唤节 
                    {
                        singleCount = 2 + 1 + 1; //文件名2 + 节名字1 + 选择和调用限定词1
                        break;
                    }
                case TypeIdentification.F_LS_NA_1: // 最后的节，最后的段
                    {
                        singleCount = 2 + 1 + 1 + 1;//文件名2 + 节名字1 + UI8 + UI8
                        singleCount = 0;
                        break;
                    }
                case TypeIdentification.F_AF_NA_1: // 确认文件，确认节
                    {
                        singleCount = 2 + 1 + 1; //文件2 节1 + AFQ
                        break;
                    }
                case TypeIdentification.F_SG_NA_1: //段 自定义
                    {
                        singleCount = 0;
                        break;
                    }
                case TypeIdentification.F_DR_TA_1: //目录{空白或X，只在监视（标准）方向有效}
                    {
                        singleCount = 2 + 3 + 1 + 7; //文件2 + 文件长度3 + SOF 1 + 时标7
                        break;
                    }
                case TypeIdentification.F_SC_NB_1: // 查询日志(QueryLog)
                    {
                        singleCount =  2 + 7 + 7;//文件名2+时标7+时标7
                        break;
                    }
                case TypeIdentification.C_RR_NA_1:
                    {
                        singleCount = 0;
                        break;
                    }
        

                default:
                    {
                        throw new Exception("获取单个信息对象长度,中TypeID不是所识别的");
                    }                 
                   
            }

            return singleCount;
        }

       

        /// <summary>
        /// 针对序列化的SQ = 1，设置首地址地址
        /// </summary>
        /// <returns></returns>
        public bool SetSequenceFirstObjectAddress(UInt32 addrss)
        {
            if (IsSequence)
            {
                informationObject[0] = ElementTool.GetBit7_0(addrss);
                informationObject[1] = ElementTool.GetBit15_8(addrss);
                informationObject[2] = ElementTool.GetBit23_16(addrss);
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 初始化ASDU
        /// </summary>
        /// <param name="typeId">类型ID</param>
        /// <param name="InformationObjectCount">项目数</param>
        /// <param name="IsSequence">是否顺序排列</param>
        /// <param name="causeOfTransmission">传输原因</param>
        /// <param name="AppDataPublicAddress">应用服务数据单元公共地址 </param>
        public ApplicationServiceDataUnit(byte typeId, byte informationObjectCount, bool isSequence, 
            byte causeOfTransmission, UInt16 AppDataPublicAddress)
        {
            this.typeId = typeId;
            InformationObjectCount = informationObjectCount;
            IsSequence = isSequence;
            this.causeOfTransmission1 = causeOfTransmission;
            var length = GetTotalInformationObjectLenth();
            this.informationObject = new byte[length];
            this.appDataPublicAddress1 = (byte)(AppDataPublicAddress & 0x00FF);
            this.appDataPublicAddress2 = (byte)(AppDataPublicAddress >> 8);
            objectCount = 0;
        }
        /// <summary>
        /// 初始化ASDU
        /// </summary>
        /// <param name="typeId">类型ID</param>
        /// <param name="causeOfTransmission">传输原因</param>
        /// <param name="AppDataPublicAddress">应用服务数据单元公共地址 </param>
        /// <param name="objectLen">信息对象长度</param>
        public ApplicationServiceDataUnit(byte typeId,
            byte causeOfTransmission, UInt16 AppDataPublicAddress, byte objectLen)
        {
            this.typeId = typeId;
            InformationObjectCount = 1;
            IsSequence = false;
            this.causeOfTransmission1 = causeOfTransmission;
            this.informationObject = new byte[objectLen];
            this.appDataPublicAddress1 = (byte)(AppDataPublicAddress & 0x00FF);
            this.appDataPublicAddress2 = (byte)(AppDataPublicAddress >> 8);
   
        }
        /// <summary>
        /// 默认初始化,强制进行转换
        /// </summary>
        /// <param name="dataArray">数组长度</param>
        public ApplicationServiceDataUnit(byte[] dataArray)
        {
            if (dataArray.Length < 9)
            {
                throw new Exception("ApplicationServiceDataUnit(byte[] dataArray) dataArray长度不满足要求，需要不小于9");
            }
            ASDUDataArray = dataArray;            
            informationObject = new byte[dataArray.Length -6 ];

            typeId = dataArray[0];
            variableStructureQualifier = dataArray[1];
            causeOfTransmission1 = dataArray[2];
            causeOfTransmission2 = dataArray[3];
            appDataPublicAddress1 = dataArray[4];
            appDataPublicAddress2 = dataArray[5];
            Array.Copy(dataArray, 6, InformationObject, 0, dataArray.Length - 6);

        }

        

    }
}
