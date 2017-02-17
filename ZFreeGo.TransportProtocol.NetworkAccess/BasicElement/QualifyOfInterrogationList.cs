using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.BasicElement
{
    /// <summary>
    /// 召唤限定词列表 QOI   QualifyOfInterrogationList
    /// </summary>
    public enum QualifyOfInterrogationList : uint
    {






        /// <summary>
        /// 总召唤
        /// </summary>
         GeneralInterrogation = 20,

         /// <summary>
         /// 响应站第1组召唤 inro1
         /// </summary>
         GropInterrogation1th = 21,

         /// <summary>
         /// 响应站第2组召唤 inro1
         /// </summary>
         GropInterrogation2th = 22,

         /// <summary>
         /// 响应站第9组召唤 inro1
         /// </summary>
         GropInterrogation9th = 29,

         /// <summary>
         /// 响应站第10组召唤 inro10
         /// </summary>
         GropInterrogation10th = 30,


    }
}
