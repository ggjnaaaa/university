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
using System.Windows.Shapes;

namespace KR
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        int barcod;
        string nam;
        string pric;
        string countr;
        string warrant;
        string siz;

        private string mainCommand
        {
            get
            {
                barcod = Convert.ToInt32(barcode.Text);
                nam = name.Text.ToString();
                pric = price.Text.ToString();
                countr = country.Text.ToString();
                warrant = warranty.Text.ToString();
                siz = size.Text.ToString();
                string str = $"INSERT INTO MainTable (barcode, name, price, country, warranty, size) VALUES ('{barcod}', '{nam}', '{pric}', '{countr}', '{warrant}', '{siz}');";

                return str;
            }
            set{}
        }

        public AddWindow()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _IsEnabled(true);
        }

        void _IsEnabled(bool enabled)
        {
            dopInfo.IsEnabled= enabled;
            barcode.IsEnabled= enabled;
            name.IsEnabled= enabled;
            price.IsEnabled= enabled;
            country.IsEnabled= enabled;
            warranty.IsEnabled= enabled;
            size.IsEnabled= enabled;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox.SelectedIndex == 0) AirConditioners_comm();
            else if (ListBox.SelectedIndex == 1) Dishwashers_comm();
            else if (ListBox.SelectedIndex == 2) WashingMachines_comm();
            else if (ListBox.SelectedIndex == 3) Refrigerators_comm();

            ListBox.SelectedIndex = -1;
            _IsEnabled(false);
        }

        public void Refrigerators_comm()
        {
            barcod = Convert.ToInt32(barcode.Text);
            string[] n = Convert.ToString(dopInfo.Text).Split(',');
            string str = $"INSERT INTO Refrigerators (barcode, volume, shelves, energyPerYear) VALUES ('{barcod}', '{n[0]}', '{n[1]}', '{n[2]}');";

            SendCommand(str);
            Cleaner();
        }

        public void AirConditioners_comm()
        {
            barcod = Convert.ToInt32(barcode.Text);
            string[] n = Convert.ToString(dopInfo.Text).Split(',');
            string str = $"INSERT INTO AirConditioners (barcode, room, timerOn, timerOff, powerInput) VALUES ('{barcod}', '{n[0]}', '{n[1]}', '{n[2]}', '{n[3]}');";

            SendCommand(str);
            Cleaner();
        }

        public void WashingMachines_comm()
        {
            barcod = Convert.ToInt32(barcode.Text);
            string[] n = Convert.ToString(dopInfo.Text).Split(',');
            string str = $"INSERT INTO WashingMachines (barcode, maxLoading, volume, maxSpinSpeed, water, hatchDiameter) VALUES ('{barcod}', '{n[0]}', '{n[1]}', '{n[2]}', '{n[3]}', '{n[4]}');";

            SendCommand(str);
            Cleaner();
        }

        public void Dishwashers_comm()
        {
            barcod = Convert.ToInt32(barcode.Text);
            string[] n = Convert.ToString(dopInfo.Text).Split(',');
            string str = $"INSERT INTO Dishwashers (barcode, maxCapacity, shelvesForMugs, water, timer) VALUES ('{barcod}', '{n[0]}', '{n[1]}', '{n[2]}', '{n[3]}');";

            SendCommand(str);
            Cleaner();
        }

        void SendCommand(string str)
        {
            (Application.Current.MainWindow as MainWindow).cmd = mainCommand;
            (Application.Current.MainWindow as MainWindow).cmd = str;
        }

        void Cleaner()
        {
            barcode.Clear();
            name.Clear();
            price.Clear();
            country.Clear();
            warranty.Clear();
            size.Clear();
            dopInfo.Clear();
        }
    }
}
