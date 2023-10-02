using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для DelWindow.xaml
    /// </summary>
    public partial class DelWindow : Window
    {

        public DelWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (del.Text.Length > 0)
            {
                string l = "";
                if (category.SelectedIndex == 0) l = "Refrigerators";
                else if (category.SelectedIndex == 1) l = "AirConditioners";
                else if (category.SelectedIndex == 2) l = "WashingMachines";
                else if (category.SelectedIndex == 3) l = "Dishwashers";

                string txt = del.Text;

                (Application.Current.MainWindow as MainWindow).cmd = $"DELETE FROM MainTable WHERE barcode = {txt};";

                (Application.Current.MainWindow as MainWindow).cmd = $"DELETE FROM {l} WHERE barcode = {txt};";

                del.Clear();
                category.SelectedIndex = -1;
                del.IsReadOnly = true;
            }
        }

        private void category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            del.IsReadOnly = false;
        }
    }
}
