using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using ZFreeGo.Monitor.AutoStudio.Database;

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
        private Dictionary<string , ControlAuthority> controlAuthority;

        private List<Tuple<string, int>> mDataBaseList;
        private List<string> mDeleteList;
        private ObservableCollection<ControlAuthority> mAddList;
       
        /// <summary>
        /// 权限初始化
        /// </summary>
        /// <param name="inAccountManager">权限管理器</param>
        public AuthorityManager(AccountManager inAccountManager)
        {
            accountManager = inAccountManager;

            controlAuthority = new Dictionary<string ,ControlAuthority>();


        }

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="element">元素名称</param>
        public void AddControl(FrameworkElement element)
        {
            controlAuthority.Add(element.Name, new ControlAuthority(element, AuthorityLevel.IV));
        }
        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="element">元素名称</param>
        /// <param name="level">最小权限</param>
        public void AddControl(FrameworkElement element, AuthorityLevel level)
        {
            controlAuthority.Add(element.Name, new ControlAuthority(element, level));
        }
        /// <summary>
        /// 设置控件权限
        /// </summary>
        /// <param name="element">控件</param>
        /// <param name="level">使用权限</param>
        public void SetControlAuthority(FrameworkElement element, AuthorityLevel level)
        {
            controlAuthority[element.Name].MinLevel = (int)level;
            
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
        /// 设置控件权限
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <param name="level">使用权限</param>
        ///<returns>成功返回true否则返回false</returns>
        public bool DataBaseSetControlAuthority(string name, AuthorityLevel level)
        {
            if (controlAuthority[name] != null)
            {
                controlAuthority[name].MinLevel = (int)level;
                controlAuthority[name].DataBaseExist = true;

                return true;
            }
            else
            {
                return false;
            }

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
                    //如果已经定义为false，就不需要再置为true
                    if (!m.Value.Element.IsEnabled)
                    {
                        m.Value.Element.IsEnabled = true;
                    }
                   
                }
                else
                {
                    m.Value.Element.IsEnabled = false;
                }
            }
        }


        /// <summary>
        /// 获取item
        /// </summary>
        /// <returns>ObservableCollection集合</returns>
        public ObservableCollection<ControlAuthority> GetItem()
        {
            var collect = new ObservableCollection<ControlAuthority>();
            foreach(var m in controlAuthority)
            {
                m.Value.UpdateData = false;
                collect.Add(m.Value);
            }
            return collect;
        }
        /// <summary>
        /// 获取数据库中不存在的条目
        /// </summary>
        /// <returns>获取item</returns>
        public ObservableCollection<ControlAuthority> GetDatabaseNotExistItem()
        {
            var collect = new ObservableCollection<ControlAuthority>();
            foreach (var m in controlAuthority)
            {
                if(!m.Value.DataBaseExist)
                {
                    collect.Add(m.Value);
                }
                
            }
            return collect;
        }

        /// <summary>
        /// 载入权限数据
        /// </summary>
        public void LoadAuthorityData()
        {
            try
            {
                var database = new SQLliteDatabase();
               
                mDataBaseList = database.ReadAuthorityTable();
                mDeleteList = this.CheckAuthoritySame(mDataBaseList);
                mAddList = this.GetDatabaseNotExistItem();
                if ((mAddList.Count != 0) || (mDeleteList.Count != 0))
                {
                    var str = string.Format("数据库中需要增加控件权限项目{0}条，删除{1}条。请首选设置权限。", mAddList.Count, mDeleteList.Count);
                    MessageBox.Show(str, "权限检测问题");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "载入权限数据");
            }
            
        }


        /// <summary>
        /// 更新条目
        /// </summary>
        /// <param name="items">更新的条目</param>
        /// <returns>更新数量</returns>
        public int  UpdateAuthorityDatabase(ObservableCollection<ControlAuthority> items)
        {

            var database = new SQLliteDatabase();
            if (mDeleteList.Count != 0)
            {
                database.DeleteAuthorityTableItem(mDeleteList);
            }
            if (mAddList.Count != 0)
            {
                database.InsertControlAuthorityTale(mAddList);
            }
            
            var cn = database.UpdateControlAuthorityTale(items);
            return cn;
        }

        /// <summary>
        /// 检测权限一致性
        /// </summary>
        private List<string> CheckAuthoritySame(List<Tuple<string, int>> dataBaseList)
        {


            var nullStr = new List<string>();
            foreach (var m in dataBaseList)
            {
                if (!DataBaseSetControlAuthority(m.Item1, (AuthorityLevel)m.Item2))
                {
                    nullStr.Add(m.Item1);
                    //MessageBox.Show("界面中有不存的数据库控件:" + m.Item1, "数据不匹配");

                }
            }
            return nullStr;
        }

    }
}
