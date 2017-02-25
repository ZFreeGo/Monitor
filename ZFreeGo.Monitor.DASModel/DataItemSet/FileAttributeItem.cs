using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.FileSever;

namespace ZFreeGo.Monitor.DASModel.DataItemSet
{
    /// <summary>
    /// 文件属性列表
    /// </summary>
    public class FileAttributeItem : ObservableObject
    {

        FileAttribute _fileAttribute;

        /// <summary>
        /// 获取文件名称长度
        /// </summary>
        public byte NameLen
        {
            get
            {
                return (byte)Name.Length;
            }
        }


        /// <summary>
        /// 文件名
        /// </summary>
        public string Name
        {
            set
            {
                _fileAttribute.Name = value;
                RaisePropertyChanged("Name");
            }
            get
            {
               return _fileAttribute.Name;
            } 
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public UInt32 Size
        {

            set
            {
                _fileAttribute.Size = value;
                RaisePropertyChanged("Size");
            }
            get
            {
                return _fileAttribute.Size;
            } 
        }

        /// <summary>
        /// 文件属性
        /// </summary>
        public byte Property
        {

            set
            {
                _fileAttribute.Property = value;
                RaisePropertyChanged("Property");
            }
            get
            {
                return _fileAttribute.Property;
            }
        }



        /// <summary>
        /// 文件时间
        /// </summary>
        public DateTime Time
        {
            set
            {
                _fileAttribute.Time = new CP56Time2a(value);
                RaisePropertyChanged("Time");
            }
            get
            {
                return _fileAttribute.Time.Time;
            }
        }

     

        /// <summary>
        /// 文件ID
        /// </summary>
        public UInt32 ID
        {
            set
            {
                _fileAttribute.ID = value;
                RaisePropertyChanged("ID");
            }
            get
            {
                return _fileAttribute.ID;
            }
        }


        /// <summary>
        /// 设置文件属性
        /// </summary>
        /// <param name="fileAttribute">文件属性</param>
        public FileAttributeItem(FileAttribute fileAttribute)
        {
            _fileAttribute = fileAttribute;
        }

    }
}
