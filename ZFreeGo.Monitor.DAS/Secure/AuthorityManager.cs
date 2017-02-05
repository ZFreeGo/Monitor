using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace ZFreeGo.Monitor.AutoStudio.Secure
{
    /// <summary>
    /// 权限管理器
    /// </summary>
    public class AuthorityManager
    {
        /// <summary>
        /// 账户管理器
        /// </summary>
        private AccountManager accountManager;


        /// <summary>
        /// 空间权限
        /// </summary>
        private Dictionary<FrameworkElement, ControlAuthority> controlAuthority;



        /// <summary>
        /// 权限初始化
        /// </summary>
        /// <param name="inAccountManager">权限管理器</param>
        public AuthorityManager(AccountManager inAccountManager)
        {
            accountManager = inAccountManager;

            controlAuthority = new Dictionary<FrameworkElement,ControlAuthority>();


        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="element">元素名称</param>
        public void AddControl(FrameworkElement element)
        {
            controlAuthority.Add(element, new ControlAuthority(element, AuthorityLevel.IV));
        }
        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="element">元素名称</param>
        /// <param name="level">最小权限</param>
        public void AddControl(FrameworkElement element, AuthorityLevel level)
        {
            controlAuthority.Add(element, new ControlAuthority(element, level));
        }
        /// <summary>
        /// 设置控件权限
        /// </summary>
        /// <param name="element">控件</param>
        /// <param name="level">使用权限</param>
        public void SetControlAuthority(FrameworkElement element, AuthorityLevel level)
        {
            controlAuthority[element].MinLevel = (int)level;
            
        }

        /// <summary>
        /// 设置控件权限
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="level">使用权限</param>
        public void SetControlAuthority(int index, AuthorityLevel level)
        {
            controlAuthority.ElementAt(index).Value.MinLevel = (int)level;
           
        }

        /// <summary>
        /// 获取控件允许
        /// </summary>
        /// <param name="level">允许水平</param>
        public void GetPermission(AuthorityLevel level)
        {
            foreach(var m in controlAuthority)
            {
                if ((int)level >= m.Value.MinLevel)
                {
                    m.Key.IsEnabled = true;
                }
                else
                {
                    m.Key.IsEnabled = false;
                }
            }
        }


        /// <summary>
        /// 获取time
        /// </summary>
        /// <returns>ObservableCollection集合</returns>
        public ObservableCollection<ControlAuthority> GetItem()
        {
            var collect = new ObservableCollection<ControlAuthority>();
            foreach(var m in controlAuthority)
            {
                collect.Add(m.Value);
            }
            return collect;
        }



    }
}
