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

namespace LAB2_4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 1952; i < 2052; i++)
            {
                year.Items.Add(i); 
            }
            monthadd();
        }

        private void monthadd()
        {
            month.Items.Add("1");
            month.Items.Add("2");
            month.Items.Add("3");
            month.Items.Add("4");
            month.Items.Add("5");
            month.Items.Add("6");
            month.Items.Add("7");
            month.Items.Add("8");
            month.Items.Add("9");
            month.Items.Add("10");
            month.Items.Add("11");
            month.Items.Add("12");
        }

        private void month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int d = 0;
            if ((month.SelectedIndex == 0) || (month.SelectedIndex == 2) || (month.SelectedIndex == 4) || (month.SelectedIndex == 6) || (month.SelectedIndex == 7) || (month.SelectedIndex == 9) || (month.SelectedIndex == 10))
            {
                d = 31;
            }
            if ((month.SelectedIndex == 3) || (month.SelectedIndex == 5) || (month.SelectedIndex == 7) || (month.SelectedIndex == 10))
            {
                d = 30;
            }
            int g = year.SelectedIndex;
            if ((month.SelectedIndex == 1) && (g % 4 == 0)) d = 28;
            else if ((month.SelectedIndex == 1) && (g % 4 == 0)) d = 29;

            for (int i = 1; i < d + 1; i++)
            {
                day.Items.Add(i);
            }
        }

        private void day_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int Nyear = DateTime.Now.Year;
            int Nmonth = DateTime.Now.Month;
            int Nday = DateTime.Now.Day;
            int y = Math.Abs(Nyear - Convert.ToInt32(year.SelectedItem));
            int m = Math.Abs(Nmonth - Convert.ToInt32(month.SelectedItem));
            int d = Math.Abs(Nday - Convert.ToInt32(day.SelectedItem));
            difyear.Text = Convert.ToString(y);
            difmonth.Text = Convert.ToString(m);
            difday.Text = Convert.ToString(d);
        }
    }
}
