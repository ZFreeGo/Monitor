using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.BasicElement
{
    /// <summary>
    /// 计数量召唤命令限定词-请求
    /// </summary>
    public enum QCCRequest : byte
    {
        /// <summary>
        /// 没请求计数量（未采用）
        /// </summary>
        NoUsed = 0,
        /// <summary>
        /// 请求计数量第一组
        /// </summary>
        Request1 = 1,
        /// <summary>
        /// 请求计数量第二组
        /// </summary>
        Request2 = 2,
        /// <summary>
        /// 请求计数量第三组
        /// </summary>
        Request3 = 3,
        /// <summary>
        /// 请求计数量第四组
        /// </summary>
        Request4 = 4,
        /// <summary>
        /// 请求计数量第五组
        /// </summary>
        Request5 = 5,
    }
    /// <summary>
    /// 计数量召唤命令限定词(QCC)-冻结
    /// </summary>
    public enum QCCFreeze : byte
    {
        /// <summary>
        ///读（无冻结或复位）
        /// </summary>
        Read = 0,
        /// <summary>
        /// 计数量冻结不带复位（被冻结的值为累加值）
        /// </summary>
        FreezeNoReset = 1,
        /// <summary>
        /// 计数量冻结带复位（被冻结的值为增量信息）
        /// </summary>
        FreezeReset = 2,
        /// <summary>
        /// 计数量复位
        /// </summary>
        CalculateReset = 3,

    }

    /// <summary>
    /// 计量召唤限定词QCC
    /// </summary>
   public class QualifyCalculateCommad
    {
       /// <summary>
       /// 计数量召唤命令限定词
       /// </summary>
       public byte QCC;
       /// <summary>
       /// 请求
       /// </summary>
       public QCCRequest RQT
       {
           set
           {
               QCC &= 0xC0;
               QCC |= (byte)(0x3F & (byte)value);
           }
           get
           {
               return (QCCRequest)(0x3F & QCC);
           }
       }
       /// <summary>
       /// 冻结
       /// </summary>
       public QCCFreeze FRZ
       {
           set
           {
               QCC &= 0x3F;
               QCC |= (byte)((byte)value << 6);
           }
           get
           {
               return (QCCFreeze)(QCC >> 6);
           }
       }
       /// <summary>
       /// 计数量召唤命令限定词初始化
       /// </summary>
       /// <param name="rqt">请求</param>
       /// <param name="frz">冻结</param>
       public QualifyCalculateCommad(QCCRequest rqt, QCCFreeze frz)
       {
           QCC = (byte)((byte)rqt | ((byte)frz << 6));
       }
       /// <summary>
       /// 计数量召唤命令限定词初始化
       /// </summary>
       /// <param name="qcc">字节</param>
       public QualifyCalculateCommad(byte qcc)
       {
           QCC = qcc;
       }
    }
}
