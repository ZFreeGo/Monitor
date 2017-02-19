using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace ZFreeGo.Monitor.DASModel.Table
{
    /// <summary>
    /// 这是一个参数类
    /// </summary>
    public class SystemParameter : ObservableObject
    {

        private int internalID;
        public int InternalID
        {
            get { return internalID; }
            set
            {
                internalID = value;
                RaisePropertyChanged("InternalID");
            }
        }

        private string paramName;
        public string ParamName
        {
            get { return paramName; }
            set
            {
                paramName = value;
               RaisePropertyChanged("ParamName");
            }
        }


        private string defaultValue;
        public string DefaultValue
        {
            get { return defaultValue; }
            set
            {
                defaultValue = value;
               RaisePropertyChanged("DefaultValue");
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
               RaisePropertyChanged("Description");
            }
        }

        private bool isChanged;
        /// <summary>
        /// 属性值是否被改变，true-初始化后被改变，false-没有
        /// </summary>
        public bool IsChanged
        {
            get { return isChanged; }
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="internalID">内部ID</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="description">描述</param>
        public SystemParameter(int internalID, string paramName, string defaultValue, string description)
        {
            this.internalID = internalID;
            this.paramName = paramName;
            this.defaultValue = defaultValue;
            this.description = description;

            isChanged = false;
        }



        public override string ToString()
        {
            return paramName + " (" + defaultValue + ")";
        }

    }

     
}
