using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZFreeGo.FileOperation.Comtrade.ConfigContent;
using System.Data.SQLite;

namespace ZFreeGo.Monitor.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        DataSet ds;
        ObservableCollection<Product> products;
        public MainWindow()
        {
            InitializeComponent();
           // DataSet ds = new DataSet();
           // ds.ReadXml("store.xml");
          //  MessageBox.Show("sd");
           // gridProducts.ItemsSource = GetProducts();



           // Test();
        }
        public void Test()
        {
            List<Tuple<FrameworkElement, int>> ds = new System.Collections.Generic.List<Tuple<FrameworkElement, int>>();
            ds.Add(new Tuple<FrameworkElement, int>(Set, 1));
            MessageBox.Show(ds[0].Item1.ToString());
           
        }
        public ICollection<Product> GetProducts()
        {
            try
            {
                ds = new DataSet();
                ds.ReadXmlSchema("storeTest.xsd");
                ds.ReadXml("store.xml");
                ds.WriteXml("store2.xml");
                products = new ObservableCollection<Product>();
                foreach (DataRow productRow in ds.Tables["Products"].Rows)
                {

                    products.Add(new Product((string)productRow["ModelNumber"],
                        (string)productRow["ModelName"], (decimal)productRow["UnitCost"],
                        (string)productRow["Description"], (int)productRow["CategoryID"],
                        (string)productRow["CategoryName"], (string)productRow["ProductImage"]));
                    
                }
                return products;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ReadXML");
                return null;
            }
        }

        private void gridProducts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            
            
string str =e.EditAction.ToString() + "\n"
             + e.Column.Header.ToString() + "\n"
             + e.Row.Item.ToString() + "\n"
             + e.EditingElement.ToString() + "\n"
             + gridProducts.SelectedIndex.ToString() + "\n"
             + gridProducts.ToString() + "\n"
             + gridProducts.SelectedValue.ToString();
           // e.Row.C
            
            MessageBox.Show(str, e.ToString());
        }
        decimal x = 0;
        private void Set_Click(object sender, RoutedEventArgs e)
        {
            products[0].UnitCost = x++; 
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(products[0].UnitCost.ToString());
            UpdateTable();
        }
        private void UpdateTable()
        {
            ds.Tables["Products"].Rows.Clear();
            foreach (var m in products)
            {
                DataRow row;
                row = ds.Tables["Products"].NewRow();
                row["ModelNumber"] = m.ModelNumber;
                row["ModelName"] = m.ModelName;
                row["UnitCost"] = m.UnitCost;
                row["Description"] =  m.Description; 
                row["CategoryID"] = m.CategoryID;
                row["CategoryName"] = m.CategoryName; 
                row["ProductImage"] = m.ProductImagePath;

                ds.Tables["Products"].Rows.Add(row);
                ds.WriteXml("add.xml");
            }
                

        }

        private void SqliteTest(object sender, RoutedEventArgs e)
        {
            string datasource = "test.db";
            System.Data.SQLite.SQLiteConnection.CreateFile(datasource);
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection();
            System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
            connstr.DataSource = datasource;
            connstr.CacheSize = 1024;
          //  connstr.Password = "1234";//设置密码，SQLite ADO.NET实现了数据库密码保护
            conn.ConnectionString = connstr.ToString();
            conn.Open();

            System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand();
            string sql = "CREATE TABLE test(username varchar(20),password varchar(20))";
            cmd.CommandText = sql;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();

            sql = "INSERT INTO test VALUES('a','b')";
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            sql = "SELECT * FROM test";
            cmd.CommandText = sql;
            System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            while (reader.Read())
            {
                sb.Append("username:").Append(reader.GetString(0)).Append("\n")
                .Append("password:").Append(reader.GetString(1));
            }
            MessageBox.Show(sb.ToString());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
        }
    }
}
