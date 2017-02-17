using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.ReciveCenter
{
    /// <summary>
    /// 检测代码结果代码
    /// </summary>
    public enum CheckCode : int
    {
        /// <summary>
        /// 不到最小长度
        /// </summary>
        MinLength  = 1,
        /// <summary>
        /// 不满足启动字符要求
        /// </summary>
        StartCharacter = 2,
        
        /// <summary>
        /// 完整长度
        /// </summary>
        IntactLength = 3,
        /// <summary>
        /// 完整长度
        /// </summary>
        IntactNoLength = 4,
      
        /// <summary>
        /// S格式
        /// </summary>
        TypeS = 6,

        /// <summary>
        /// 格式与长度不匹配
        /// </summary>
        TypeLenghtError = 7,


        /// <summary>
        /// U格式
        /// </summary>
        TypeU = 20,

        /// <summary>
        /// 未定义的控制命令
        /// </summary>
        TypeUError = 21,



        /// <summary>
        /// 可能为I格式
        /// </summary>
        TypeI = 100, //后续还需要继续判断
        /// <summary>
        /// 未知的TypeID
        /// </summary>
        IDUnknow = 101,
        /// <summary>
        /// 主站系统命令
        /// </summary>
        MasterComand = 102,
        /// <summary>
        /// 遥信命令
        /// </summary>
        TelesignalisationCommand = 103,
        /// <summary>
        /// 遥测信息
        /// </summary>
        TelemeteringCommand = 104,
        /// <summary>
        /// 遥控命令
        /// </summary>
        TelecontrolCommand = 105,

        /// <summary>
        /// 电度获取
        /// </summary>
         ElectricEnergy = 106,
        /// <summary>
        /// 校准信息
        /// </summary>
        Calibration = 107,
        /// <summary>
        /// 保护定值设定
        /// </summary>
        ProtectsetPoint = 108,

        /// <summary>
        /// 文件服务
        /// </summary>
        FileServer = 109,

        /// <summary>
        /// TypeID已经定义但不未使用
        /// </summary>
        IDUnused = 1000,

    }
}
