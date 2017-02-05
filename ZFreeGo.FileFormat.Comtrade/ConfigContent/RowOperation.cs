using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFreeGo.FileOperation.Comtrade.ConfigContent
{
    /// <summary>
    /// 行内容基本操作
    /// </summary>
    public class RowOperation
    {
        /// <summary>
        /// 按标准要求GBT22386 生成行字符串信息
        /// </summary>
        /// <returns>行字符串</returns>
        public virtual  string RowToString()
        {
            throw new Exception("没有实现 MakeRowString");
        }

        /// <summary>
        /// 将行字符串转化为对应行信息。
        /// </summary>
        /// <param name="str">行字符串</param>
        /// <returns>false--转换成功 true--失败</returns>
        public virtual  bool StringToRow(string str)
        {
            throw new Exception("没有实现StringToRow");           
        }

       
    }
}
