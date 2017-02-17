using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.BasicElement
{
    /// <summary>
    /// 选择和召唤限定词 低4bit
    /// </summary>
    public enum SelectCallQulifierLow : byte
    {
        /// <summary>
        /// 缺省
        /// </summary>
        Defalut = 0,
        /// <summary>
        /// 选择文件
        /// </summary>
        SelectFile = 1,
        /// <summary>
        /// 请求文件
        /// </summary>
        RequestFile = 2,
        /// <summary>
        /// 停止激活文件
        /// </summary>
        StopActivedFile = 3,
        /// <summary>
        /// 删除文件
        /// </summary>
        DeleteFile = 4,
        /// <summary>
        /// 选择节
        /// </summary>
        SelectSection = 5,
        /// <summary>
        /// 请求节
        /// </summary>
        RequestSection = 6,
        /// <summary>
        /// 停止激活节
        /// </summary>
        StopActivedSection = 7,

    }

    /// <summary>
    /// 选择和召唤限定词 高4bit
    /// </summary>
    public enum SelectCallQulifierHight
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
    /// 选择和召唤限定词 
    /// </summary
    public class SelectCallQulifier
    {
        /// <summary>
        /// 选择和召唤限定词 
        /// </summary>
        public byte SCQ;
        
        /// <summary>
        /// 选择和召唤限定词 初始化
        /// </summary>
        /// <param name="scq">限定词</param>
        public SelectCallQulifier(byte scq)
        {
            SCQ = scq;
        }
        /// <summary>
        /// 选择和召唤限定词 
        /// </summary>
        /// <param name="afqh">高4bit限定词</param>
        /// <param name="afql">低4bit限定词</param>
        public SelectCallQulifier(SelectCallQulifierHight scqh, SelectCallQulifierLow scql)
        {
            SCQ = (byte)(((byte)scqh << 4) | ((byte)scql));
        }
    }
}
