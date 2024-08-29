using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfCustomControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для StarterMenu.xaml
    /// </summary>
    public partial class StarterMenu : UserControl
    {
        public static readonly DependencyProperty RegisterCommandProperty = DependencyProperty.Register("RegisterCommand",
                                                                                             typeof(ICommand),
                                                                                             typeof(StarterMenu));

        public static readonly DependencyProperty LogInCommandProperty = DependencyProperty.Register("LogInCommand",
                                                                                             typeof(ICommand),
                                                                                             typeof(StarterMenu));

        public StarterMenu()
        {
            InitializeComponent();
        }

        public ICommand RegisterCommand
        {
            get { return (ICommand)GetValue(RegisterCommandProperty); }
            set { SetValue(RegisterCommandProperty, value); }
        }

        public ICommand LogInCommand
        {
            get { return (ICommand)GetValue(LogInCommandProperty); }
            set { SetValue(LogInCommandProperty, value); }
        }
    }
}
