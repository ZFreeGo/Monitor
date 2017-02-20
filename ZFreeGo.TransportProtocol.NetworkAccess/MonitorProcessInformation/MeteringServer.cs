using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.Frame;
using ZFreeGo.TransmissionProtocols.Helper;

namespace ZFreeGo.TransmissionProtocols.MonitorProcessInformation
{
     public class MeteringServer : ReciveServer<ApplicationServiceDataUnit>
    {
        /// <summary>
        /// 遥测测量信息
        /// </summary>
        public event EventHandler<StatusEventArgs<List<Tuple<UInt32, float, QualityDescription>>>> TelemeteringEvent;


         /// <summary>
         /// 测量数据接收服务初始化
         /// </summary>
        public MeteringServer()
            : base("MeteringServer-" + DateTime.Now.ToLongTimeString() + "-")
        {
            StartServer();
        }

        /// <summary>
        /// 检测接收数据
        /// </summary>
        /// <returns>true--检测通过, false--检测失败</returns>
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
        /// <param name="ASDU">ASDU</param
        private void GetComprehensiveMessage(ApplicationServiceDataUnit asdu)
        {           
            switch ((TypeIdentification)asdu.TypeId)
            {
                case TypeIdentification.M_ME_NA_1://测量值，归一化值
                {
                    break;
                }
                case TypeIdentification.M_ME_NC_1://测量值，短浮点数
                {

                    var m = GetMessage(asdu);
                    SendTelemeteringEvent(m, (TypeIdentification)asdu.TypeId);
                    break;
                }
                case TypeIdentification.M_ME_TD_1://带CP56Time2a时标的测量值，归一化值
                case TypeIdentification.M_ME_TF_1://带CP56Time2a时标的测量值，短浮点数
                {
                    break;
                }
                default:
                    {
                        break;
                    }
            }
        }

         /// <summary>
         /// 提取短浮点数
         /// </summary>
         /// <param name="asdu">ASDU</param>
         /// <returns>包含地址,浮点数,品质描述的元组列表</returns>
        private List<Tuple<UInt32, float, QualityDescription>> GetMessage(ApplicationServiceDataUnit asdu)
        {
            var list = new List<Tuple<UInt32, float, QualityDescription>>();

            if (asdu.IsSequence)
            {
                int len = 4 + 1;
                var addr = ElementTool.CombinationByte(asdu.InformationObject[0], asdu.InformationObject[1], asdu.InformationObject[2]);
                for (int i = 0; i < asdu.InformationObjectCount; i++)
                {
                    var data = new byte[5];
                    Array.Copy(asdu.InformationObject, i * len + 3, data, 0, 5);
                    var m = new ShortFloating(data);
                    var q = new QualityDescription(data[4]);
                    list.Add(new Tuple<UInt32, float, QualityDescription>((UInt32)(addr + i), m.Value, q));
                }
            }
            else
            {
                int len = 8;//3+4+1
                for (int i = 0; i < asdu.InformationObjectCount; i++)
                {
                    var addr1 = ElementTool.CombinationByte(asdu.InformationObject[0 + len * i],
                        asdu.InformationObject[1 + len * i], asdu.InformationObject[2 + +len * i]);
                    var data = new byte[4];
                    Array.Copy(asdu.InformationObject, i * len + 3, data, 0, 5);
                    var m = new ShortFloating(data);
                    var q = new QualityDescription(data[4]);
                    list.Add(new Tuple<UInt32, float, QualityDescription>((UInt32)addr1, m.Value, q));
                }
            }
            return list;


        }

         /// <summary>
         /// 发送
         /// </summary>
         /// <param name="m">发送浮点数遥测信息</param>
        private void SendTelemeteringEvent(List<Tuple<UInt32, float, QualityDescription>> m, TypeIdentification id)
         {
             if (TelemeteringEvent != null)
             {
                 TelemeteringEvent(this, new StatusEventArgs<List<Tuple<uint,float,QualityDescription>>>(m, id));
             }
         }
       



    }
}
