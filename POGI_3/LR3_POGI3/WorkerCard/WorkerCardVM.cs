using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WorkerCard.Model;
using WpfCustomControlLibrary;

namespace WorkerCard
{
    internal class WorkerCardVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly UsersData usersData;
        private readonly BitmapImage BlankPic = (BitmapImage)Application.Current.Resources["BlankProfilePic"];

        public ICommand StarterMenu_RegisterCommand { get; }
        public ICommand StarterMenu_LogInCommand { get; }

        public ICommand LogIn_LogInCommand { get; }

        public ICommand Register_RegisterCommand { get; }

        public ICommand UserCard_SignOutCommand { get; }
        public ICommand UserCard_UpdateCommand { get; }

        public ICommand UpdateCard_SaveCommand { get; }
        public ICommand UpdateCard_UpdateImage { get; }

        public ICommand BackToUserCardCommand { get; }
        public ICommand BackToStarterMenuCommand { get; }

        public WorkerCardVM()
        {
            usersData = new UsersData();
            employee = new Employee(0);
            _lastEmployee = new Employee(0);

            viewState = ViewState.StarterMenu;

            StarterMenu_RegisterCommand = new RelayCommand(Open_Register);
            StarterMenu_LogInCommand = new RelayCommand(Open_LogIn);
            Register_RegisterCommand = new RelayCommand(Open_UserCard_Register);
            LogIn_LogInCommand = new RelayCommand(Open_UserCard_LogIn);
            UserCard_SignOutCommand = new RelayCommand(Open_StarterMenu);
            UserCard_UpdateCommand = new RelayCommand(Open_UpdateCard);
            UpdateCard_SaveCommand = new RelayCommand(Open_UserCard_UpdateCard);
            UpdateCard_UpdateImage = new RelayCommand(GetImagePath);

            BackToUserCardCommand = new RelayCommand(CancelUpdateCard);
            BackToStarterMenuCommand = new RelayCommand(Open_StarterMenu);
        }

        #region Свойства с информацией пользователя
        private Employee _lastEmployee;
        private Employee _employee;
        private Employee employee
        {
            get => _employee;
            set
            {
                _employee = value;
                OnPropertyChanged(nameof(Nickname));
                OnPropertyChanged(nameof(RealName));
                OnPropertyChanged(nameof(Department));
                OnPropertyChanged(nameof(Position));
                OnPropertyChanged(nameof(Image));
            }
        }

        private string _nickname;
        public string Nickname
        {
            get => employee.Nickname;
            set
            {
                employee.Nickname = value;
                OnPropertyChanged();
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        private string _realName;
        public string RealName
        {
            get => employee.RealName;
            set
            {
                employee.RealName = value;
                OnPropertyChanged();
            }
        }
        private string _department;
        public string Department
        {
            get => employee.Department;
            set
            {
                employee.Department = value;
                OnPropertyChanged();
            }
        }
        private string _position;
        public string Position
        {
            get => employee.Position;
            set
            {
                employee.Position = value;
                OnPropertyChanged();
            }
        }
        private BitmapImage _image;
        public BitmapImage Image
        {
            get
            {
                if (imagePath != null && !File.Exists(imagePath))
                {
                    MessageBox.Show($"Файл {imagePath} не найден.");
                    employee.ImagePath = string.Empty;
                    _image = BlankPic;
                    return _image;
                }
                return _image;
            }
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }
        private string imagePath
        {
            get => employee.ImagePath;
            set
            {
                if (!File.Exists(value))
                {
                    MessageBox.Show($"Файл {value} не найден.");
                    employee.ImagePath = string.Empty;
                    Image = BlankPic;
                    return;
                }
                employee.ImagePath = value;
                var temp = new BitmapImage();
                temp.BeginInit();
                temp.UriSource = new Uri(value, UriKind.Absolute);
                temp.CacheOption = BitmapCacheOption.OnLoad;
                temp.EndInit();
                temp.Freeze();
                Image = temp;
            }
        }
        #endregion

        #region Свойства видимости элементов из WpfCustomControlLibrary
        private ViewState _viewState;
        private ViewState viewState
        {
            get => _viewState;
            set
            {
                _viewState = value;
                OnPropertyChanged(nameof(StarterMenu_Visibility));
                OnPropertyChanged(nameof(LogIn_Visibility));
                OnPropertyChanged(nameof(Register_Visibility));
                OnPropertyChanged(nameof(UserCard_Visibility));
                OnPropertyChanged(nameof(UpdateCard_Visibility));
            }
        }
        public Visibility StarterMenu_Visibility => GetVisibilityFromViewState(ViewState.StarterMenu);
        public Visibility LogIn_Visibility => GetVisibilityFromViewState(ViewState.LogIn);
        public Visibility Register_Visibility => GetVisibilityFromViewState(ViewState.Register);
        public Visibility UserCard_Visibility => GetVisibilityFromViewState(ViewState.UserCard);
        public Visibility UpdateCard_Visibility => GetVisibilityFromViewState(ViewState.UpdateCard);

        private Visibility GetVisibilityFromViewState(ViewState state) => viewState == state ? Visibility.Visible : Visibility.Collapsed;
        #endregion

        #region Методы обработки команд
        /// <summary>
        /// Открывает элемент регистрации
        /// </summary>
        /// <param name="parameter"></param>
        private void Open_Register(object parameter)
        {
            _lastEmployee = new Employee(employee);
            employee = new Employee(0);
            viewState = ViewState.Register;
        }
        /// <summary>
        /// Открывает элемент входа
        /// </summary>
        /// <param name="parameter"></param>
        private void Open_LogIn(object parameter)
        {
            _lastEmployee = new Employee(employee);
            employee = new Employee(0);
            viewState = ViewState.LogIn;
        }
        /// <summary>
        /// Открывает элемент начального меню
        /// </summary>
        /// <param name="parameter"></param>
        private void Open_StarterMenu(object parameter)
        {
            employee = new Employee(0);
            _lastEmployee = new Employee(0);
            viewState = ViewState.StarterMenu;
        }
        /// <summary>
        /// Открывает элемент обновления информации
        /// </summary>
        /// <param name="parameter"></param>
        private void Open_UpdateCard(object parameter)
        {
            _lastEmployee = new Employee(employee);
            viewState = ViewState.UpdateCard;
        }
        /// <summary>
        /// Закрывает элемент регистрации
        /// </summary>
        /// <param name="parameter"></param>
        private void CancelUpdateCard(object parameter)
        {
            employee = new Employee(_lastEmployee);
            _lastEmployee = new Employee(0);
            viewState = ViewState.UserCard;
        }
        /// <summary>
        /// Открывает элемент карточки пользователя после регистрации
        /// </summary>
        /// <param name="parameter"></param>
        private void Open_UserCard_Register(object parameter)
        {
            if (CheckRegister())
            {
                Open_UserCard(usersData.CreateEmployee(Nickname, Password));
                MessageBox.Show("Пользователь успешно зарегистрирован");
            }
        }
        /// <summary>
        /// Открывает элемент карточки пользователя после входа
        /// </summary>
        /// <param name="parameter"></param>
        private void Open_UserCard_LogIn(object parameter)
        {
            Open_UserCard(usersData.CreateEmployee(Nickname, Password));
        }
        /// <summary>
        /// Открывает элемент карточки пользователя после изменения данных
        /// </summary>
        /// <param name="parameter"></param>
        private void Open_UserCard_UpdateCard(object parameter)
        {
            UpdateUserInfo();
            viewState = ViewState.UserCard;
        }

        /// <summary>
        /// Открывает элемент карточки пользователя
        /// </summary>
        /// <param name="emp"></param>
        private void Open_UserCard(Employee emp)
        {
            employee = emp;
            viewState = ViewState.UserCard;
            Password = string.Empty;
        }
        /// <summary>
        /// Проверяет возможность регистрации с введёнными данными
        /// </summary>
        /// <returns></returns>
        private bool CheckRegister()
        {
            if (!DataValidator.IsLoginActuallyUsing(Nickname))
            {
                if (!DataValidator.IsPasswordMeetsRequirements(Password))
                {
                    MessageBox.Show("Пароль не соответствует требованиям.\nВ пароле могут использоваться строчные и заглавные латитнские буквы, цифры и подчеркивания.\nМинимальная длина 8 символов.");
                    return false;
                }
                return true;
            }
            else
            {
                MessageBox.Show("Пользователь с таким логином уже существует");
                return false;
            }
        }
        /// <summary>
        /// Обновляет всю информацию пользователя
        /// </summary>
        private void UpdateUserInfo()
        {
            employee = UsersData.UpdateEmployeeLogin(employee, Nickname);
            employee = UsersData.UpdateEmployeeRealName(employee, RealName);
            employee = UsersData.UpdateEmployeeDepartment(employee, Department);
            employee = UsersData.UpdateEmployeePosition(employee, Position);
            employee = UsersData.UpdateEmployeeImagePath(employee, imagePath);
        }
        /// <summary>
        /// Получает путь к фото через диалоговввое окно
        /// </summary>
        /// <param name="parameter"></param>
        private void GetImagePath(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                if (fileInfo.Length <= (2 * 1024 * 1024))  // Проверка длины файла (не больше 2МБ)
                {
                    imagePath = openFileDialog.FileName;
                }
                else
                {
                    MessageBox.Show("Размер файла превышает 2Мб, выберите файл меньшего размера.");
                }
            }
        }
        #endregion

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum ViewState
    {
        StarterMenu,
        LogIn,
        Register,
        UserCard,
        UpdateCard
    }
}
