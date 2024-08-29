using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfCustomControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для UserCard.xaml
    /// </summary>
    public partial class UserCard : UserControl
    {
        public static readonly DependencyProperty NicknameProperty = DependencyProperty.Register("Nickname",
                                                                                     typeof(string),
                                                                                     typeof(UserCard),
                                                                                     new PropertyMetadata(string.Empty, NicknameChanged));

        public static readonly DependencyProperty RealNameProperty = DependencyProperty.Register("RealName",
                                                                                             typeof(string),
                                                                                             typeof(UserCard),
                                                                                             new PropertyMetadata(string.Empty, RealNameChanged));

        public static readonly DependencyProperty DepartmentProperty = DependencyProperty.Register("Department",
                                                                                             typeof(string),
                                                                                             typeof(UserCard),
                                                                                             new PropertyMetadata(string.Empty, DepartmentChanged));

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position",
                                                                                             typeof(string),
                                                                                             typeof(UserCard),
                                                                                             new PropertyMetadata(string.Empty, PositionChanged));

        public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register("Image",
                                                                                             typeof(BitmapImage),
                                                                                             typeof(UserCard),
                                                                                             new PropertyMetadata(null, ImagePathChanged));

        public static readonly DependencyProperty UpdateCommandProperty = DependencyProperty.Register("UpdateCommand",
                                                                                             typeof(ICommand),
                                                                                             typeof(UserCard));

        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register("CancelCommand",
                                                                                             typeof(ICommand),
                                                                                             typeof(UserCard));

        public UserCard()
        {
            InitializeComponent();
        }

        public string Nickname
        {
            get { return (string)GetValue(NicknameProperty); }
            set { SetValue(NicknameProperty, value); }
        }
        public string RealName
        {
            get { return (string)GetValue(RealNameProperty); }
            set { SetValue(RealNameProperty, value); }
        }
        public string Department
        {
            get { return (string)GetValue(DepartmentProperty); }
            set { SetValue(DepartmentProperty, value); }
        }
        public string Position
        {
            get { return (string)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        public BitmapImage Image
        {
            get { return (BitmapImage)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        public ICommand UpdateCommand
        {
            get { return (ICommand)GetValue(UpdateCommandProperty); }
            set { SetValue(UpdateCommandProperty, value); }
        }
        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        private static void NicknameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((UserCard)obj).NicknameTextBox.Text = ((UserCard)obj).Nickname;
        private static void RealNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((UserCard)obj).RealNameTextBox.Text = ((UserCard)obj).RealName;
        private static void DepartmentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((UserCard)obj).DepartmentTextBox.Text = ((UserCard)obj).Department;
        private static void PositionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((UserCard)obj).PositionTextBox.Text = ((UserCard)obj).Position;
        private static void ImagePathChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((UserCard)obj).EmployeeImage.Source = ((UserCard)obj).Image;
        }
    }
}
