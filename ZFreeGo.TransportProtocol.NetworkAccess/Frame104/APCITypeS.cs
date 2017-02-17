using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.Frame104
{
    /// <summary>
    /// S 格式（ Numbered Supervisory Function） S 格式的 APDU 只包括 APCI
    /// </summary>
    public class APCITypeS
    {
        /// <summary>
        /// 通用apci数据,S 格式的 APDU 只包括 APCI
        /// </summary>
        private ApplicationProtocalControlnformation apci;
        /// <summary>
        /// 接收序列号
        /// </summary>
        public UInt16 ReceiveSequenceNumber
        {
            get
            {
                return (ushort)(apci.ControlDomain3 >> 1 + (UInt16)apci.ControlDomain4 << 7);//将低7bit与高8bit(7-14)组合
            }
            set
            {
                apci.ControlDomain1 = 0x01;
                apci.ControlDomain2 = 0;
                apci.ControlDomain3 = (byte)((value << 1) & (0x00FF));//获取低7bit，0bit为0
                apci.ControlDomain4 = (byte)(value >> 7);            //获取高8bit(7-14)
            }
        }

        /// <summary>
        /// 获取APCI数据
        /// </summary>
        /// <returns>6字节数组</returns>
        public byte[] GetAPCIDataArray()
        {
            return apci.GetAPCIDataArray();
        }

        /// <summary>
        /// APCI-S监控帧
        /// </summary>
        /// <param name="reciveNum">接收序列号</param>
        public APCITypeS(UInt16 reciveNum)
        {
            apci = new ApplicationProtocalControlnformation(0x68, 4, 0x01, 0, 0,  0);
            ReceiveSequenceNumber = reciveNum;
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
            strBuild.Append("APCITypeS,");
            strBuild.AppendFormat("APDU长度:[{0:X00}]={0:00},", apci.APDULength, apci.APDULength);
            strBuild.AppendFormat("接收序列号:[{0:X00} {1:X00} {2:X00} {3:X00}}={4:00}", apci.ControlDomain4, apci.ControlDomain3, apci.ControlDomain2, apci.ControlDomain1,
               ReceiveSequenceNumber);
            return strBuild.ToString();

        }
    }
}
