using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfCustomControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для LogIn.xaml
    /// </summary>
    public partial class LogIn : UserControl
    {
        public static readonly DependencyProperty LoginProperty = DependencyProperty.Register("Login",
                                                                                             typeof(string),
                                                                                             typeof(LogIn),
                                                                                             new PropertyMetadata(string.Empty, LoginChanged));

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password",
                                                                                             typeof(string),
                                                                                             typeof(LogIn),
                                                                                             new PropertyMetadata(string.Empty, PasswordChanged));

        public static readonly DependencyProperty LoginCommandProperty = DependencyProperty.Register("LoginCommand",
                                                                                             typeof(ICommand),
                                                                                             typeof(LogIn));

        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register("CancelCommand",
                                                                                             typeof(ICommand),
                                                                                             typeof(LogIn));

        public LogIn()
        {
            InitializeComponent();
        }

        public string Login
        {
            get { return (string)GetValue(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public ICommand LoginCommand
        {
            get { return (ICommand)GetValue(LoginCommandProperty); }
            set { SetValue(LoginCommandProperty, value); }
        }

        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        private static void LoginChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((LogIn)obj).LoginTextBox.Text = ((LogIn)obj).Login;
        private static void PasswordChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((LogIn)obj).LoginPasswordBox.Password = ((LogIn)obj).Password;
    }
}
