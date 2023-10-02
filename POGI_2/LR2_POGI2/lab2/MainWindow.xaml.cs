using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteConnection m_dbConnection;
        SQLiteCommand command;
        SQLiteDataAdapter da;

        string sql;

        string ID;
        string FIO;
        string dateOfBirth;
        string physics;
        string math;

        public MainWindow()
        {
            InitializeComponent();

            string db_name = Environment.CurrentDirectory + "\\BD.db";

            m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");
            // открытие соединения с базой данных
            m_dbConnection.Open();

            GetData();
        }

        public void GetData()
        {
            DataSet ds = new DataSet(); //Создаем объект класса DataSet
            sql = "Select * From FioDateOfBirth CROSS JOIN Mark WHERE FioDateOfBirth.ID = Mark.ID;"; //Sql запрос (достать все из таблицы customer)
            da = new SQLiteDataAdapter(sql, m_dbConnection);//Создаем объект класса DataAdapter (тут мы передаем наш запрос и получаем ответ)
            da.Fill(ds);//Заполняем DataSet cодержимым DataAdapter'a
            DG1.DataContext = ds.Tables[0].DefaultView;//Заполняем созданный на форме dataGridView1
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            FIO = addFio.Text;
            dateOfBirth = addDateOfBirth.Text;
            physics = addPhys.Text;
            math = addMath.Text;

            sql = $"INSERT INTO FioDateOfBirth (FIO, DateOfBirth) VALUES ('{FIO}', '{dateOfBirth}');";
            command = new SQLiteCommand(sql, m_dbConnection);
            // извлечение запроса
            command.ExecuteNonQuery();

            sql = $"INSERT INTO Mark (Physics, Math) VALUES ('{physics}', '{math}');";
            command = new SQLiteCommand(sql, m_dbConnection);
            // извлечение запроса
            command.ExecuteNonQuery();

            addDateOfBirth.Clear();
            addFio.Clear();
            addMath.Clear();
            addPhys.Clear();

            GetData();
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            ID = delID.Text;

            sql = $"DELETE FROM FioDateOfBirth WHERE ID = {ID};";
            command = new SQLiteCommand(sql, m_dbConnection);
            // извлечение запроса
            command.ExecuteNonQuery();

            sql = $"DELETE FROM Mark WHERE ID = {ID};";
            command = new SQLiteCommand(sql, m_dbConnection);
            // извлечение запроса
            command.ExecuteNonQuery();

            delID.Clear();

            GetData();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ID = editID.Text;
            FIO = editFio.Text;
            dateOfBirth = editDateOfBirth.Text;
            physics = editPhys.Text;
            math = editMath.Text;

            sql = $"UPDATE FioDateOfBirth SET FIO = '{FIO}', DateOfBirth = '{dateOfBirth}' WHERE id = {ID};";
            command = new SQLiteCommand(sql, m_dbConnection);
            // извлечение запроса
            command.ExecuteNonQuery();

            sql = $"UPDATE Mark SET Physics = {physics}, Math = {math} WHERE id = {ID};";
            command = new SQLiteCommand(sql, m_dbConnection);
            // извлечение запроса
            command.ExecuteNonQuery();

            editDateOfBirth.Clear();
            editFio.Clear();
            editID.Clear();
            editMath.Clear();
            editPhys.Clear();

            GetData();
        }

        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            GetData();
        }

            private void OffButton_Click(object sender, RoutedEventArgs e)
        {
            m_dbConnection.Close();
        }
    }
}
