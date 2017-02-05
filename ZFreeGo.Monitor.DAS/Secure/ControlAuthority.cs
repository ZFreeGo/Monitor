using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace ZFreeGo.Monitor.AutoStudio.Secure
{
    /// <summary>
    /// 控件权限标注
    /// </summary>
    public class ControlAuthority: INotifyPropertyChanged
    {
        /// <summary>
        /// 元素
        /// </summary>
        private FrameworkElement mElement;
        /// <summary>
        /// 获取空间的Name
        /// </summary>
        public string ElementName
        {
            set
            {
               // OnPropertyChanged(new PropertyChangedEventArgs("ElementName"));
            }
            get
            {
                return mElement.Name;
            }
        }





        /// <summary>
        /// 所需要的最小权限等级
        /// </summary>
        private AuthorityLevel mMinLevel;
   
        /// <summary>
        /// 设置或获取所需要的最小权限等级
        /// </summary>
        public int MinLevel
        {
            set
            {
                mMinLevel = (AuthorityLevel)value;
                OnPropertyChanged(new PropertyChangedEventArgs("MinLevel"));
            }
            get
            {
                return (int)mMinLevel;
            }
        }



        /// <summary>
        /// 获取当前元素的ToString
        /// </summary>
        public string ToStr
        {
            set
            {
                // OnPropertyChanged(new PropertyChangedEventArgs("ElementName"));
            }
            get
            {
                return mElement.ToString();
            }
        }

        /// <summary>
        /// 控件权限
        /// </summary>
        /// <param name="element">控件</param>
        /// <param name="minLevel">最小使用权限</param>
        public ControlAuthority(FrameworkElement element, AuthorityLevel minLevel)
        {
            mElement = element;
            mMinLevel = minLevel;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {

            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
     
    }
}
