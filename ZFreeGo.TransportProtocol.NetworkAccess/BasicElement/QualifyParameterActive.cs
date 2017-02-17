using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.BasicElement
{
    /// <summary>
    /// 参数激活限定词
    /// </summary>
    public enum QualifyParameterActive : byte
    {
        /// <summary>
        /// 未用
        /// </summary>
        NotUsed = 0,
        /// <summary>
        /// 激活/停止激活这之前装载的参数（信息对象地址=0）^8
        /// </summary>
        LoadParameter = 1,
        /// <summary>
        /// 激活/停止激活所寻址信息对象的参数
        /// </summary>
        AddressingObjec = 2,
        /// <summary>
        /// 激活/停止激活所寻址的持续循环或周期传输的信息对象参数
        /// </summary>
        CycleObject =3,

    }
}
