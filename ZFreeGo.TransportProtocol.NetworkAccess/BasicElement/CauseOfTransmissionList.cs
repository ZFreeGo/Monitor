using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.BasicElement
{
    //    原因 ＝Cause ∶＝UI6[1..6]<0..63>
    //<0> ∶＝ 未用
    //<1> ∶＝ 周期、循环 per/cyc
    //<2> ∶＝ 背景扫描 back
    //<3> ∶＝ 突发(自发) spont
    //<4> ∶＝ 初始化 init
    //<5> ∶＝ 请求或者被请求 req
    //<6> ∶＝ 激活 act
    //<7> ∶＝ 激活确认 actcon
    //<8> ∶＝ 停止激活 deact
    //<9> ∶＝ 停止激活确认 deactcon
    //<10> ∶＝ 激活终止 actterm
    //<11> ∶＝ 远方命令引起的返送信息 retrem
    //<12> ∶＝ 当地命令引起的返送信息 retloc
    //<13> ∶＝ 文件传输 file
    //<14..19>∶＝ 为配套标准兼容范围保留
    //<20> ∶＝ 响应站召唤 introgen
    //<21> ∶＝ 响应第 1 组召唤 inro1
    //<22> ∶＝ 响应第 2 组召唤 inro2
    //<29> ∶＝ 响应第 9 组召唤 inro9
    //<30> ∶＝ 响应第 10 组召唤 inro10
    //<42..43> ∶＝ 为配套标准兼容范围保留
    //<44> ∶＝ 未知的类型标识
    //<45> ∶＝ 未知的传送原因
    //<46> ∶＝ 未知的应用服务数据单元公共地址
    //<47> ∶＝ 未知的信息对象地址
    //<48..63>∶ ＝ 特殊应用能力保留(专用范围

    /// <summary>
    /// 传送原因列表,定义了传送原因代号
    /// </summary>
    public enum CauseOfTransmissionList : byte
    {
        NULL = 0,

        /// <summary>
        /// 周期,循环 per/cyc
        /// </summary>
        Cycle = 1,

        /// <summary>
        /// 背景扫描 back
        /// </summary>
         BackgroundScanning = 2,

        /// <summary>
        /// 突发（自发）
        /// </summary>
         Spontaneous  = 3,

         /// <summary>
         /// 初始化 init
         /// </summary>
         Initialization = 4,

         /// <summary>
         /// 请求或者被请求 Request
         /// </summary>
         Request = 5,

        /// <summary>
         /// 激活 act
        /// </summary>
         Activation = 6,

         /// <summary>
         /// 激活确认 actcon
         /// </summary>
         ActivationACK = 7,

 
        /// <summary>
        /// 停止激活 deact
        /// </summary>
         Deactivate = 8,

         /// <summary>
         /// 停止激活确认 deactcon
         /// </summary>
         DeactivateACK = 9,

         /// <summary>
         /// 激活终止 ActivateTermination
         /// </summary>
         ActivateTermination = 10,

         /// <summary>
         /// 远方命令引起的返送信息 retrem
         /// </summary>
         FoldbackFromDistanceCommand = 11,


         /// <summary>
         /// 当地命令引起的返送信息 retloc
         /// </summary>
         FoldbackFromLocalCommand = 12,


         /// <summary>
         /// 文件传输 file
         /// </summary>
         FileTransmit = 13,


        //<14..19>∶＝ 为配套标准兼容范围保留

         /// <summary>
         /// 响应站召唤 introgen
         /// </summary>
         ResponseStationInterrogation = 20,

         /// <summary>
         /// 响应站第1组召唤 inro1
         /// </summary>
         ResponseGropInterrogation1th = 21,

         /// <summary>
         /// 响应站第1组召唤 inro1
         /// </summary>
         ResponseGropInterrogation2th = 22,

         /// <summary>
         /// 响应站第1组召唤 inro1
         /// </summary>
         ResponseGropInterrogation9th = 29,

         /// <summary>
         /// 响应站第1组召唤 inro10
         /// </summary>
         ResponseGropInterrogation10th = 30,

        //<42..43> ∶＝ 为配套标准兼容范围保留

         /// <summary>
         /// 未知的类型标识 unknownTypeID
         /// </summary>
         UnknownTypeID = 44,

         /// <summary>
         /// 未知的传送原因 UnknownTypeCaseTransmission
         /// </summary>
         UnknownTypeCaseTransmission = 45,

         /// <summary>
         /// 未知的应用服务数据单元地址 UnknownAppDataPublicAddress
         /// </summary>
         UnknownAppDataPublicAddress = 46,

         /// <summary>
         /// 未知信息对象地址 UnknownInformationObjectAddress
         /// </summary>
         UnknownInformationObjectAddress = 47,

         //<48..63>∶ ＝ 特殊应用能力保留(专用范围)


    }
}
