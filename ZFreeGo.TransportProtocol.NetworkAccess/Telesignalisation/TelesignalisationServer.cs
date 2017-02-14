using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;
using ZFreeGo.TransportProtocol.NetworkAccess.Helper;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess
{
    /// <summary>
    /// 遥信服务---一个文件使用一个服务
    /// </summary>
    public class TelesignalisationServer : ReciveServer<APDU>
    {

        /// <summary>
        /// 遥信服务
        /// </summary>
        /// <param name="overTime">重复次数</param>
        /// <param name="maRepeat">最大重复次数</param>
        public TelesignalisationServer(int overTime, int maxRepeat) :base(overTime, maxRepeat)
        {
            
        }


        /// <summary>
        /// 检测接收数据
        /// </summary>
        /// <returns>true--检测通过，false--检测失败</returns>
        public override bool CheckData()
        {
            try
            {
                GetInformationList(mReciveQuene.Dequeue().ASDU);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 提取信息对象列表元素
        /// </summary>
        public void GetInformationList(ApplicationServiceDataUnit ASDU)
        {
            try
            {

               
                switch ((TypeIdentification)ASDU.TypeId)
                {
                    //遥信信息
                    case TypeIdentification.M_SP_NA_1://单点信息
                    case TypeIdentification.M_DP_NA_1://双点信息
                        {
                            var message = GetMessage(ASDU);
                            break;
                        }

                    case TypeIdentification.M_SP_TB_1://带CP56Time2a时标的单点信息
                    case TypeIdentification.M_DP_TB_1://带CP56Time2a时标的双点信息
                        {
                            var message = GetMessageWithTime(ASDU);
                            break;
                        }
                     
                    default:
                        {
                            break;
                        }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取单点或双点信息
        /// </summary>
        /// <param name="ASDU">ASDU</param>
        /// <returns>含有地址和遥信信息</returns>
        private List<Tuple<UInt32, byte>> GetMessage(ApplicationServiceDataUnit ASDU)
        {
            var list = new List<Tuple<UInt32, byte>>();
            if (ASDU.IsSequence)
            {
                // int len = 1;
                var addr = ElementTool.CombinationByte(ASDU.InformationObject[0], ASDU.InformationObject[1], ASDU.InformationObject[2]);
                for (int i = 0; i < ASDU.InformationObjectCount; i++)
                {
                    var m = ASDU.InformationObject[3 + i];

                    list.Add(new Tuple<UInt32, byte>((UInt32)(addr + i), m));
                }
            }
            else
            {
                int len = 4;
                for (int i = 0; i < ASDU.InformationObjectCount; i++)
                {
                    var addr1 = ElementTool.CombinationByte(ASDU.InformationObject[0 + len * i],
                        ASDU.InformationObject[1 + len * i], ASDU.InformationObject[2 + len * i]);
                    var m = ASDU.InformationObject[3 + len * i];
                    list.Add(new Tuple<UInt32, byte>(addr1, m));
                }
            }
            return list;
        }
        /// <summary>
        /// 获取单点或双点信息，含有时标
        /// </summary>
        /// <param name="ASDU">ASDU</param>
        /// <returns>含有地址，遥信信息，时标</returns>
        private List<Tuple<UInt32, byte, CP56Time2a>> GetMessageWithTime(ApplicationServiceDataUnit ASDU)
        {
            var list = new List<Tuple<UInt32, byte, CP56Time2a>>();
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
                    list.Add(new Tuple<UInt32, byte, CP56Time2a>((UInt32)(addr + i), m, t));
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
                    list.Add(new Tuple<UInt32, byte, CP56Time2a>(addr1, m, t));
                }
            }
            return list;
        }


    }
}
