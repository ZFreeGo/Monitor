using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement
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
        private TransmissionCotrolFunction transmissionCotrolFunction;

        public TransmissionCotrolFunction TransmissionCotrolFun
        {
            get {return transmissionCotrolFunction;}
        }


        /// <summary>
        /// 获取APCI数据
        /// </summary>
        /// <returns>6字节数组</returns>
        public byte[] GetAPCIDataArray() 
        {
            return apci.GetAPCIDataArray();
        }




        public void SetControlMessage(TransmissionCotrolFunction fun)
        {

            switch (fun)
            {
                case TransmissionCotrolFunction.StartDataTransmission:
                    {

                        apci.ControlDomain1 = (1 << 2) | 0x03;
                        
                        break;
                    }
                case TransmissionCotrolFunction.StopDataTransmission:
                    {
                        apci.ControlDomain1 = (1 << 4) | 0x03;

                        break;
                    }
                case TransmissionCotrolFunction.TestFrame:
                    {
                        apci.ControlDomain1 = (1 << 6) | 0x03;

                       break;
                    }
                case TransmissionCotrolFunction.AcknowledgementStartDataTransmission:
                    {

                        apci.ControlDomain1 = (1 << 3) | 0x03;

                        break;
                    }
                case TransmissionCotrolFunction.AcknowledgementStopDataTransmission:
                    {
                        apci.ControlDomain1 = (1 << 5) | 0x03;

                        break;
                    }
                case TransmissionCotrolFunction.AcknowledgementTestFrame:
                    {
                        apci.ControlDomain1 = (1 << 7) | 0x03; ;

                        break;
                    }
                default :
                    {
                        throw new Exception("TransmissionCotrolFunction功能选择项不正确");
                    }
           
            }
            apci.ControlDomain2 = 0;
            apci.ControlDomain3 = 0;
            apci.ControlDomain4 = 0;
            transmissionCotrolFunction = fun;
            

        }

        public APCITypeU(TransmissionCotrolFunction fun)
        {
            apci = new ApplicationProtocalControlnformation();
            SetControlMessage(fun);
            apci.APDULength = 4; //仅仅只有APCI
           
           
        }



    }
}
