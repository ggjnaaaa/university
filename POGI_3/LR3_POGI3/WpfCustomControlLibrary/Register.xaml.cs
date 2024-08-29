using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfCustomControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
        public static readonly DependencyProperty LoginProperty = DependencyProperty.Register("Login",
                                                                                     typeof(string),
                                                                                     typeof(Register),
                                                                                     new PropertyMetadata(string.Empty, LoginChanged));

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password",
                                                                                             typeof(string),
                                                                                             typeof(Register),
                                                                                             new PropertyMetadata(string.Empty, PasswordChanged));

        public static readonly DependencyProperty RegisterCommandProperty = DependencyProperty.Register("RegisterCommand",
                                                                                             typeof(ICommand),
                                                                                             typeof(Register));

        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register("CancelCommand",
                                                                                             typeof(ICommand),
                                                                                             typeof(Register));

        public Register()
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

        public ICommand RegisterCommand
        {
            get { return (ICommand)GetValue(RegisterCommandProperty); }
            set { SetValue(RegisterCommandProperty, value); }
        }

        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        private static void LoginChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((Register)obj).UsernameTextBox.Text = ((Register)obj).Login;
        private static void PasswordChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((Register)obj).PasswordBox.Password = ((Register)obj).Password;

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают. Попробуйте еще раз.");
                return;
            }

            Password = password;
            RegisterCommand?.Execute(null);
        }
    }
}
