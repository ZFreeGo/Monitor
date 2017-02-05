using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileElement
{
    /// <summary>
    /// 文件认可或节认可限定词 低4bit
    /// </summary>
    public enum AckFileQulifierLow : byte
    {
        /// <summary>
        /// 缺省
        /// </summary>
        Defalut = 0,
        /// <summary>
        /// 文件传输的肯定认可
        /// </summary>
        FileAck = 1,
        /// <summary>
        /// 文件传输的否定认可
        /// </summary>
        FileNoAck = 1,
         /// <summary>
        /// 节传输的肯定认可
        /// </summary>
        SectionAck = 1,
        /// <summary>
        /// 节传输的否定认可
        /// </summary>
        SectionNoAck = 1,
    }

    /// <summary>
    /// 文件认可或节认可限定词 高4bit
    /// </summary>
    public enum AckFileQulifierHight
    {
        /// <summary>
        /// 缺省
        /// </summary>
        Defalut = 0,
        /// <summary>
        /// 无所请求的存储空间
        /// </summary>
        NoRequestSpace = 1,
        /// <summary>
        /// 校验和错误
        /// </summary>
        RequestFile = 2,
        /// <summary>
        ///非所期望的通信服务
        /// </summary>
        StopActivedFile = 3,
        /// <summary>
        /// 非所期望的文件名称
        /// </summary>
        DeleteFile = 4,
        /// <summary>
        /// 非所期望的节名称
        /// </summary>
        SelectSection = 5,
     
    }
    
    /// <summary>
    /// 文件认可或节认可限定词
    /// </summary
    public class AckFileQulifier
    {
        /// <summary>
        /// 选择和召唤限定词 
        /// </summary>
        public byte AFQ;
        
        /// <summary>
        /// 选择和召唤限定词 初始化
        /// </summary>
        /// <param name="scq">限定词</param>
        public AckFileQulifier(byte afq)
        {
            AFQ = afq;
        }
        /// <summary>
        /// 文件认可或节认可限定词
        /// </summary>
        /// <param name="afqh">高4bit限定词</param>
        /// <param name="afql">低4bit限定词</param>
        public AckFileQulifier(AckFileQulifierHight afqh, AckFileQulifierLow afql)
        {
            AFQ= (byte)(((byte)afqh << 4) | ((byte)afql));
        }
    }
}
