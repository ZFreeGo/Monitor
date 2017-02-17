using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocols.TransmissionControl104;

namespace ZFreeGo.TransmissionProtocols.Frame104
{
    /// <summary>
    /// U 格式（ Unnumbered Control Function） U 格式的 APDU 只包括 APCI
    /// </summary>
    public class APCITypeU 
    {
        /// <summary>
        /// 通用apci数据
        /// </summary>
        private ApplicationProtocalControlnformation apci;

        /// <summary>
        /// 传输控制功能
        /// </summary>
        private TransmissionControlFunction TransmissionControlFunction;

        public TransmissionControlFunction TransmissionCotrolFun
        {
            get {return TransmissionControlFunction;}
        }


        /// <summary>
        /// 获取APCI数据
        /// </summary>
        /// <returns>6字节数组</returns>
        public byte[] GetAPCIDataArray() 
        {
            return apci.GetAPCIDataArray();
        }




        public void SetControlMessage(TransmissionControlFunction fun)
        {

            switch (fun)
            {
                case TransmissionControlFunction.StartDataTransmission:
                    {

                        apci.ControlDomain1 = (1 << 2) | 0x03;
                        
                        break;
                    }
                case TransmissionControlFunction.StopDataTransmission:
                    {
                        apci.ControlDomain1 = (1 << 4) | 0x03;

                        break;
                    }
                case TransmissionControlFunction.TestFrame:
                    {
                        apci.ControlDomain1 = (1 << 6) | 0x03;

                       break;
                    }
                case TransmissionControlFunction.AcknowledgementStartDataTransmission:
                    {

                        apci.ControlDomain1 = (1 << 3) | 0x03;

                        break;
                    }
                case TransmissionControlFunction.AcknowledgementStopDataTransmission:
                    {
                        apci.ControlDomain1 = (1 << 5) | 0x03;

                        break;
                    }
                case TransmissionControlFunction.AcknowledgementTestFrame:
                    {
                        apci.ControlDomain1 = (1 << 7) | 0x03; ;

                        break;
                    }
                default :
                    {
                        throw new Exception("TransmissionControlFunction功能选择项不正确");
                    }
           
            }
            apci.ControlDomain2 = 0;
            apci.ControlDomain3 = 0;
            apci.ControlDomain4 = 0;
            TransmissionControlFunction = fun;
            

        }

        public APCITypeU(TransmissionControlFunction fun)
        {
            apci = new ApplicationProtocalControlnformation();
            SetControlMessage(fun);
            apci.APDULength = 4; //仅仅只有APCI
           
           
        }

        /// <summary>
        /// 产生帧信息事件
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var data = GetAPCIDataArray();
            StringBuilder strbuild = new StringBuilder(data.Length * 3 + 10);
            foreach (var m in data)
            {
                strbuild.AppendFormat("{0:X00) ", m);
            }
            return strbuild.ToString();
        }

        /// <summary>
        /// 对信息进行分割
        /// </summary>
        /// <param name="flag">true--对照分割，false--详细分割</param>
        /// <returns>信息字符串</returns>
        public string ToString(bool flag)
        {
            
                StringBuilder strBuild = new StringBuilder(100);
                strBuild.Append("APCITypeU,");
                strBuild.AppendFormat("APDU长度:[{0:X00}]={0:00},", apci.APDULength, apci.APDULength);
                strBuild.AppendFormat("控制功能:[{0:X00} {1:X00} {2:X00} {3:X00}}={4:00}", apci.ControlDomain4, apci.ControlDomain3, apci.ControlDomain2, apci.ControlDomain1,
                    TransmissionCotrolFun.ToString());
                     return strBuild.ToString();
           
        }

    }
}
