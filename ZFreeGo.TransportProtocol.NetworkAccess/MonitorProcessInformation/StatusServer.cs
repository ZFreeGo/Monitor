using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.BasicElement;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement;
using ZFreeGo.TransportProtocol.NetworkAccess.Helper;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess.MonitorProcessInformation
{
    /// <summary>
    /// 状态更新服务 遥信，SOE，时间记录 
    /// </summary>
    public class StatusServer : ReciveServer<ApplicationServiceDataUnit>
    {
        /// <summary>
        /// 单 双点信息
        /// </summary>
        public event EventHandler<StatusEventArgs<List<Tuple<UInt32, byte>>>>   StatusUpdateEvent;

        /// <summary>
        /// 带时标的单点或双点信息
        /// </summary>
        public event EventHandler<StatusEventArgs<List<Tuple<UInt32, byte, CP56Time2a>>>> SOEStatusEvent;


        /// <summary>
        /// 状态服务
        /// </summary>
        public StatusServer()
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
                if (mReciveQuene.Count > 0)
                {
                    GetComprehensiveMessage(mReciveQuene.Dequeue());
                }
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取综合信息，包含帧解释与帧数据提取
        /// </summary>
        /// <param name="ASDU">ASDU</param>
        private void GetComprehensiveMessage(ApplicationServiceDataUnit asdu)
        {
            try
            {


                switch ((TypeIdentification)asdu.TypeId)
                {
                    //遥信信息
                    case TypeIdentification.M_SP_NA_1://单点信息
                    case TypeIdentification.M_DP_NA_1://双点信息
                        {

                            var message = GetMessage(asdu);
                            SendSingleDoubleEvent(message);
                            break;
                        }
                     //SOE
                    case TypeIdentification.M_SP_TB_1://带CP56Time2a时标的单点信息
                    case TypeIdentification.M_DP_TB_1://带CP56Time2a时标的双点信息
                        {
                            var message = GetMessageWithTime(asdu);
                            SendSingleDoubleEvent(message);
                            break;
                        }
                        case TypeIdentification.M_FT_NA_1: //故障值信息
                        {

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


        /// <summary>
        /// 获取故障信息
        /// </summary>
        /// <param name="aSDU">ASDU</param>
        private void GetMessageMalfunction(ApplicationServiceDataUnit aSDU)
        {
            var list = new List<Tuple<UInt32, byte, CP56Time2a>>();

            int len = 7 + 1 + 2;
            int  count = aSDU.InformationObject[0];
            byte typID =  aSDU.InformationObject[1];
            int offset = 2;

            for (int i = 0; i < count; i++)
            {
                var addr1 = ElementTool.CombinationByte(aSDU.InformationObject[len * i + offset++],
                    aSDU.InformationObject[8 * i + offset++]);

                var m = aSDU.InformationObject[len * i + offset++];
                var data = new byte[7];
                Array.Copy(aSDU.InformationObject, i * len + offset++, data, 0, 7);
                var t = new CP56Time2a(data);
                list.Add(new Tuple<UInt32, byte, CP56Time2a>(addr1, m, t));
            }
            int start = offset + count * 10;
            count = aSDU.InformationObject[start];
            typID = aSDU.InformationObject[start + 1];
            offset = 2;
            //
            //
            //待完善
        }


        /// <summary>
        /// 发送单双点信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="apdu">列表</param>
        private void SendSingleDoubleEvent(List<Tuple<uint,byte>> message)
        {
            if (StatusUpdateEvent != null)
            {
                StatusUpdateEvent(this, new StatusEventArgs<List<Tuple<uint,byte>>>(message));
            }
        }
        /// <summary>
        /// 发送带时标的单双点信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="apdu">列表</param>
        private void SendSingleDoubleEvent(List<Tuple<uint, byte, CP56Time2a>> message)
        {
            if (StatusUpdateEvent != null)
            {
                SOEStatusEvent(this, new StatusEventArgs<List<Tuple<uint, byte, CP56Time2a>>>(message));
            }
        }
    }
}
