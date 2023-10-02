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

namespace LAB2
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void plus_Click(object sender, RoutedEventArgs e)
        {
            double a = Double.Parse(enterA.Text);
            double b = Double.Parse(enterB.Text);
            string res = Convert.ToString(a + b);
            outputRez.Content = res;
        }

        private void minus_Click(object sender, RoutedEventArgs e)
        {
            double a = Double.Parse(enterA.Text);
            double b = Double.Parse(enterB.Text);
            string res = Convert.ToString(a - b);
            outputRez.Content = res;
        }

        private void umn_Click(object sender, RoutedEventArgs e)
        {
            double a = Double.Parse(enterA.Text);
            double b = Double.Parse(enterB.Text);
            string res = Convert.ToString(a * b);
            outputRez.Content = res;
        }

        private void div_Click(object sender, RoutedEventArgs e)
        {
            double a = Double.Parse(enterA.Text);
            double b = Double.Parse(enterB.Text);
            string res = Convert.ToString(a / b);
            outputRez.Content = res;
        }
    }
}