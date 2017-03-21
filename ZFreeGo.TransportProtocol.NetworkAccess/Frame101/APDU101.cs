using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocols.Frame;

namespace ZFreeGo.TransmissionProtocols.Frame101
{
    public  class APDU101
    {
        /// <summary>
        /// 通用APCI数据
        /// </summary>
        //public APCITypeI APCI;

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

        public byte[] GetAPDUDataArray()
        {
            return null;
        }
        /// <summary>
        /// 使用字节数组初始化，属于强制性转换初始化
        /// </summary>
        /// <param name="dataArray">字节数组</param>
        public APDU101(byte[] dataArray)
        {
            //if (dataArray.Length < 15)
            //{
            //    throw new Exception("APDU(byte[] dataArray) 长度不应该小于15");
            //}

            //APCI = new APCITypeI(dataArray);
            //var data = new byte[dataArray.Length - 6];
            //Array.Copy(dataArray, 6, data, 0, dataArray.Length - 6);
            //FrameArray = dataArray;
            //ASDU = new ApplicationServiceDataUnit(data);

            //TimeStamp = DateTime.Now;
        }

             /// <summary>
        /// 使用APCI与ASDU初始化APDU
        /// </summary>
        /// <param name="apci">APCI</param>
        /// <param name="asdu">ASDU</param>
        //public APDU(APCITypeI apci, ApplicationServiceDataUnit asdu)
        //{
        //    APCI = apci;
        //    ASDU = asdu;
        //    TimeStamp = DateTime.Now;
        //}


    }
}
