using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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

namespace KR
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteConnection m_dbConnection;
        SQLiteCommand command;
        SQLiteDataAdapter da;

        string sql;

        public MainWindow()
        {
            InitializeComponent();

            string db_name = Environment.CurrentDirectory + "\\products.db";

            m_dbConnection = new SQLiteConnection("Data Source=" + db_name + ";Version=3;");
            // открытие соединения с базой данных
            m_dbConnection.Open();

            ListBox.SelectedIndex = 0;
        }

        #region Добавление/удаление товаров, обновление списка товаров
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow wind = new AddWindow();
            wind.Owner = this; //окно MainWindow главное для этого окна
            wind.Show();
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            DelWindow wind = new DelWindow();
            wind.Owner = this; //окно MainWindow главное для этого окна
            wind.Show();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            int i = ListBox.SelectedIndex;
            ListBox.SelectedIndex = 0;
            ListBox.SelectedIndex = i;
        }
        #endregion

        #region Вывод таблиц в DataGrid
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox.SelectedIndex == 0) AllGetData();
            else if (ListBox.SelectedIndex == 1) AirConditionersGetData();
            else if (ListBox.SelectedIndex == 2) DishwashersGetData();
            else if (ListBox.SelectedIndex == 3) WashingMachinesGetData();
            else if (ListBox.SelectedIndex == 4) RefrigeratorsGetData();
        }

        public void AllGetData() //загрузка таблицы со всеми товарами
        {
            string[] head = { "Штрих-код", "Название", "Цена", "Страна", "Гарантия", "Размер" };
            string[] bind = { "barcode", "name", "price", "country", "warranty", "size" };

            NewColums(head, bind, 5);

            SqlData("Select * From MainTable;");
        }

        public void RefrigeratorsGetData() //загрузка таблицы с холодильниками
        {
            string[] head = { "Штрих-код", "Название", "Цена", "Страна", "Гарантия", "Размер", "Объём холод./мороз. камеры", "кол-во полок в холод. камере / не двери / ящиков в мороз. камере", "Энергопотребление" };
            string[] bind = { "barcode", "name", "price", "country", "warranty", "size", "volume", "shelves", "energyPerYear" };

            NewColums(head, bind, 8);

            SqlData("Select * From MainTable CROSS JOIN Refrigerators WHERE Refrigerators.barcode = MainTable.barcode;");
        }

        public void DishwashersGetData() //загрузка таблицы с посудомоечными машинами
        {
            string[] head = { "Штрих-код", "Название", "Цена", "Страна", "Гарантия", "Размер", "Макс. вместимость", "Полки для кружек", "Расход воды за цикл", "Таймер отложенного страта" };
            string[] bind = { "barcode", "name", "price", "country", "warranty", "size", "maxCapacity", "shelvesForMugs", "water", "timer" };

            NewColums(head, bind, 9);

            SqlData("Select * From MainTable CROSS JOIN Dishwashers WHERE Dishwashers.barcode = MainTable.barcode;");
        }

        public void AirConditionersGetData() //загрузка таблицы с кондиционерами
        {
            string[] head = { "Штрих-код", "Название", "Цена", "Страна", "Гарантия", "Размер", "Площадь комнаты", "Таймер включния", "Таймер выключения", "Потребляемая мощность" };
            string[] bind = { "barcode", "name", "price", "country", "warranty", "size", "room", "timerOn", "timerOff", "powerInput" };

            NewColums(head, bind, 9);

            SqlData("Select * From MainTable CROSS JOIN AirConditioners WHERE AirConditioners.barcode = MainTable.barcode;");
        }

        public void WashingMachinesGetData() //загрузка таблицы со стиральными машинами
        {
            string[] head = { "Штрих-код", "Название", "Цена", "Страна", "Гарантия", "Размер", "Макс. загрузка", "Объём барабана", "Максю скорость отжима", "Расход воды", "Диаметр люка" };
            string[] bind = { "barcode", "name", "price", "country", "warranty", "size", "maxLoading", "volume", "maxSpinSpeed", "water", "hatchDiameter" };

            NewColums(head, bind, 10);

            SqlData("Select * From MainTable CROSS JOIN WashingMachines WHERE WashingMachines.barcode = MainTable.barcode;");
        }

        //создаёт новые колонки
        public void NewColums(string[] head, string[] bind, int n)
        {
            int count_col = DG1.Columns.Count;
            if (count_col != 0) //если есть колонки - они удаляются
            for (int i = 0; i < count_col; i++)
            {
                DG1.Columns.Remove(DG1.Columns[0]);
            }

            for (int i = 0; i <= n; i++) //новые колонки
            {
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.Header = head[i];
                textColumn.Binding = new Binding(bind[i]);
                textColumn.IsReadOnly = true;
                DG1.Columns.Add(textColumn);
            }
        }

        public void SqlData(string sql)
        {
            DataSet ds = new DataSet(); //Создаем объект класса DataSet
            da = new SQLiteDataAdapter(sql, m_dbConnection);//Создаем объект класса DataAdapter (тут мы передаем наш запрос и получаем ответ)
            da.Fill(ds);//Заполняем DataSet cодержимым DataAdapter'a
            DG1.DataContext = ds.Tables[0].DefaultView;//Заполняем созданный на форме dataGridView1
        }
        #endregion

        public string cmd
        {
            get
            { return cmd; }
            set
            {
                sql = value;
                Wind_Command(sql);
            }
        }

        private void Wind_Command(string sql)
        {
            command = new SQLiteCommand(sql, m_dbConnection);
            // извлечение запроса
            command.ExecuteNonQuery();
        }
    }
}
