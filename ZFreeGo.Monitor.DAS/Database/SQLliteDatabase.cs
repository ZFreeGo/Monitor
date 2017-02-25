using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using ZFreeGo.Monitor.AutoStudio.Secure;
using System.Collections.ObjectModel;

namespace ZFreeGo.Monitor.AutoStudio.Database
{
    /// <summary>
    /// SQLliteDatabase数据库
    /// </summary>
    public class SQLliteDatabase
    {
        string datasource = @"Data\Database\Das.db";

        /// <summary>
        /// 创建数据库
        /// </summary>
        public void CreatDatabase()
        {
            //  System.Data.SQLite.SQLiteConnection.CreateFile(datasource);
        }

        /// <summary>
        /// 创建
        /// </summary>
        public void connectDatabase()
        {
            
         
            //连接
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection();
            System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
            connstr.DataSource = datasource;
            //connstr.Password = "admin";//设置密码，SQLite ADO.NET实现了数据库密码保护
            conn.ConnectionString = connstr.ToString();
            conn.Open();

            //创建表格
            //System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
            //string sql = "CREATE TABLE control_authority(ElementName varchar(32), MinLevel int,ToStr varchar(32))";
            //cmd.CommandText = sql;
            //cmd.Connection = conn;
            //cmd.ExecuteNonQuery();

            //插入数据
            //sql = "INSERT INTO test VALUES('a','b')";
            //cmd.CommandText = sql;
            //cmd.ExecuteNonQuery();

           
        }

        /// <summary>
        /// 将数据列表插入表格
        /// </summary>
        /// <param name="collect">权限集合</param>
        public void InsertControlAuthorityTale(ObservableCollection<ControlAuthority> collect)
        {
            try
            {
                using (var conn = new System.Data.SQLite.SQLiteConnection())
                {
                    var connstr = new SQLiteConnectionStringBuilder();
                    connstr.DataSource = datasource;
                    //connstr.Password = "admin";//设置密码，SQLite ADO.NET实现了数据库密码保护
                    conn.ConnectionString = connstr.ToString();
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {            
                       
                        cmd.Connection = conn;

                        //删除表格中索引的数据
                        //string sql = "delete from control_authority";
                        //cmd.CommandText = sql;
                        //cmd.ExecuteNonQuery();
                        string sql = "";
                        foreach (var m in collect)
                        {

                            //sql = "INSERT INTO test VALUES('a','b')";
                            sql = string.Format("INSERT INTO  control_authority VALUES(\'{0}\',{1},\'{2}\')", m.ElementName, m.MinLevel, m.ToStr);

                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    conn.Close();

                }



            }
            catch(Exception ex)
            {
                throw ex;
            }


        }
       /// <summary>
       /// 读取权限列表
       /// </summary>
       /// <returns>权限列表的name与权限</returns>
        public  List<Tuple<string, int >>  ReadAuthorityTable()
        {
            try
            {
                using (var conn = new System.Data.SQLite.SQLiteConnection())
                {
                    var connstr = new SQLiteConnectionStringBuilder();
                    connstr.DataSource = datasource;
                    //connstr.Password = "admin";//设置密码，SQLite ADO.NET实现了数据库密码保护
                    conn.ConnectionString = connstr.ToString();
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {


                        var collect = new List<Tuple<string, int>>();

                        cmd.Connection = conn;

                        string sql = "SELECT * from control_authority";
                        cmd.CommandText = sql;
                        System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();
                        while(reader.Read())
                        {
                            collect.Add(new Tuple<string, int>(reader.GetString(0), reader.GetInt32(1)));
                        }



                        conn.Close();
                        return collect;                      
                    }
                    

                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        /// <summary>
        /// 删除表格中的一些项目
        /// </summary>
        /// <param name="list">删除列表</param>
        public void DeleteAuthorityTableItem(List<string> list)
        {
            try
            {
                if (list.Count > 0)
                {

                    using (var conn = new System.Data.SQLite.SQLiteConnection())
                    {
                        var connstr = new SQLiteConnectionStringBuilder();
                        connstr.DataSource = datasource;
                        //connstr.Password = "admin";//设置密码，SQLite ADO.NET实现了数据库密码保护
                        conn.ConnectionString = connstr.ToString();
                        conn.Open();
                        using (SQLiteCommand cmd = new SQLiteCommand())
                        {


                            var collect = new List<Tuple<string, int>>();

                            cmd.Connection = conn;

                            StringBuilder sqlBuilder = new StringBuilder();
                            sqlBuilder.Append(@"SELECT * from control_authority where ");
                            int cn = 0;
                            foreach (var m in list)
                            {
                                sqlBuilder.Append("ElementName=");
                                sqlBuilder.Append(m);
                                if(++cn != list.Count)
                                {
                                    sqlBuilder.Append(" or ");
                                }
                                
                            }

                            cmd.CommandText = sqlBuilder.ToString();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }


                    }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新表格中的一些项目
        /// </summary>
        /// <param name="collect">权限集合</param>
        /// <returns>更新计数</returns>
        public int UpdateControlAuthorityTale(ObservableCollection<ControlAuthority> collect)
        {
            try
            {
                int cn = 0;
                using (var conn = new System.Data.SQLite.SQLiteConnection())
                {
                    var connstr = new SQLiteConnectionStringBuilder();
                    connstr.DataSource = datasource;
                    //connstr.Password = "admin";//设置密码，SQLite ADO.NET实现了数据库密码保护
                    conn.ConnectionString = connstr.ToString();
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = conn;
                        string sql = "";
                        foreach (var m in collect)
                        {
                            if (m.UpdateData)
                            {
                                sql = string.Format("update  control_authority set MinLevel = {0} where ElementName = '{1}' ", m.MinLevel, m.ElementName);
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                m.UpdateData = false;
                                cn++;
                            }
                        }
                    }
                    conn.Close();
                    return cn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
            
        

       
    }
}
