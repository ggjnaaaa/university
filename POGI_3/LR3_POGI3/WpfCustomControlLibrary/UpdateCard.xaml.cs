using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfCustomControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для UpdateCard.xaml
    /// </summary>
    public partial class UpdateCard : UserControl
    {
        public static readonly DependencyProperty NicknameProperty = DependencyProperty.Register("Nickname",
                                                                                             typeof(string),
                                                                                             typeof(UpdateCard),
                                                                                             new PropertyMetadata(string.Empty, NicknameChanged));

        public static readonly DependencyProperty RealNameProperty = DependencyProperty.Register("RealName",
                                                                                             typeof(string),
                                                                                             typeof(UpdateCard),
                                                                                             new PropertyMetadata(string.Empty, RealNameChanged));

        public static readonly DependencyProperty DepartmentProperty = DependencyProperty.Register("Department",
                                                                                             typeof(string),
                                                                                             typeof(UpdateCard),
                                                                                             new PropertyMetadata(string.Empty, DepartmentChanged));

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position",
                                                                                             typeof(string),
                                                                                             typeof(UpdateCard),
                                                                                             new PropertyMetadata(string.Empty, PositionChanged));

        public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register("Image",
                                                                                             typeof(BitmapImage),
                                                                                             typeof(UpdateCard),
                                                                                             new PropertyMetadata(null, ImageChanged));

        public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register("SaveCommand",
                                                                                             typeof(ICommand),
                                                                                             typeof(UpdateCard));

        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register("CancelCommand",
                                                                                             typeof(ICommand),
                                                                                             typeof(UpdateCard));

        public static readonly DependencyProperty ImageCommandProperty = DependencyProperty.Register("ImageCommand",
                                                                                             typeof(ICommand),
                                                                                             typeof(UpdateCard));

        public UpdateCard()
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

        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }
        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }
        public ICommand ImageCommand
        {
            get { return (ICommand)GetValue(ImageCommandProperty); }
            set { SetValue(ImageCommandProperty, value); }
        }

        private void EmployeeImage_Click(object sender, RoutedEventArgs e) => ImageCommand?.Execute(null);

        private static void NicknameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((UpdateCard)obj).NicknameTextBox.Text = ((UpdateCard)obj).Nickname;
        private void NicknameChanged(object sender, RoutedEventArgs e) => Nickname = NicknameTextBox.Text;

        private static void RealNameChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((UpdateCard)obj).RealNameTextBox.Text = ((UpdateCard)obj).RealName;
        private void RealNameChanged(object sender, RoutedEventArgs e) => RealName = RealNameTextBox.Text;

        private static void DepartmentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((UpdateCard)obj).DepartmentTextBox.Text = ((UpdateCard)obj).Department;
        private void DepartmentChanged(object sender, RoutedEventArgs e) => Department = DepartmentTextBox.Text;

        private static void PositionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) => ((UpdateCard)obj).PositionTextBox.Text = ((UpdateCard)obj).Position;
        private void PositionChanged(object sender, RoutedEventArgs e) => Position = PositionTextBox.Text;

        private static void ImageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((UpdateCard)obj).EmployeeImage.Source = ((UpdateCard)obj).Image;
        }
    }
}
