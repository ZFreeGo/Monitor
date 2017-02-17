using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.BasicElement
{
    /// <summary>
    /// 文件状态SOF
    /// </summary>
    public  class StatusOfFile
    {
        /// <summary>
        /// 文件的状态
        /// </summary>
        public byte SOF;

        /// <summary>
        /// 文件状态
        /// </summary>
        public byte Status;
        /// <summary>
        /// 最后还有目录文件 0-后面还有目录文件 1-最后目录文件 
        /// </summary>
        public byte LFD
        {
            set
            {
                if ((value & 0x01) == 0x01)
                {
                    SOF |= 0x20;
                }
                else
                {
                    SOF &= 0xDF;
                }                             
            }
            get
            {
                if ((SOF & 0x20) == 0x20)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }  
            }
        }
        /// <summary>
        /// 文件名或者子目录，0-定义文件名，1-定义子目录
        /// </summary>
        public byte FOR
        {
            set
            {
                if ((value & 0x01) == 0x01)
                {
                    SOF |= 0x40;
                }
                else
                {
                    SOF &= 0xBF;
                }
            }
            get
            {
                if ((SOF & 0x40) == 0x40)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 文件等待传输，0-文件等待传输，1-文件传输已激活
        /// </summary>
        public byte FA
        {
            set
            {
                if ((value & 0x01) == 0x01)
                {
                    SOF |= 0x80;
                }
                else
                {
                    SOF &= 0x7F;
                }
            }
            get
            {
                if ((SOF & 0x80) == 0x80)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 文件状态
        /// </summary>
        /// <param name="sof">文件状态</param>
       public StatusOfFile(byte sof)
        {
            this.SOF = sof;
        }
       /// <summary>
       /// 文件状态
       /// </summary>
       /// <param name="fa">文件等待传输，0-文件等待传输，1-文件传输已激活</param>
       /// <param name="fo">文件名或者子目录，0-定义文件名，1-定义子目录</param>
       /// <param name="lfd">最后还有目录文件 0-后面还有目录文件 1-最后目录文件 </param>
       /// <param name="def">缺省</param>
       public StatusOfFile(byte fa, byte fo, byte lfd, byte def)
       {
           SOF = (byte)(((byte)fa << 7) | ((byte)fo << 6) | ((byte)lfd << 5) | (def & 0x1F));
       }
    }
}
