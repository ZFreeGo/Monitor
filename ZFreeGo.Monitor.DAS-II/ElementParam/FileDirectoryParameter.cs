using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocol.NetworkAccess104.FileNr;

namespace ZFreeGo.Monitor.AutoStudio.ElementParam
{
    /// <summary>
    /// 文件目录参数
    /// </summary>
    public class FileDirectoryParameter 
    {
        /// <summary>
        /// 文件属性
        /// </summary>
        public FileAttribute Attribute;


        public string Name
        {
            get
            {
                return Attribute.Name;
            }
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public UInt32 Size
        {
           get
            {
                return Attribute.Size;
            }
        }

    }
}
