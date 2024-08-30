using KR.Commands;
using KR.Data;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace KR.ViewModels
{
    /// <summary>
    /// Вьюмодель окна для добавления мема
    /// </summary>
    internal class MemeAddVM : VMBase
    {
        public delegate void MemeAddedEventHandler(Meme newMeme);
        public event MemeAddedEventHandler MemeAdded;

        #region Свойства
        public string Name { get; set; }
        public string Category { get; set; }
        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MemeImage));
            }
        }

        public ICommand AddCommand { get; }
        public ICommand UpdateImageCommand { get; }

        public BitmapImage MemeImage
        {
            get => File.Exists(FilePath) ? GetBitmapImageFromPath(FilePath) : BlankPic;
            set => OnPropertyChanged();
        }
        #endregion

        public MemeAddVM()
        {
            AddCommand = new RelayCommand(AddMeme);
            UpdateImageCommand = new RelayCommand(UpdateMemeImage);
        }

        #region Обработка нажатий
        /// <summary>
        /// Нажатие на "Добавить мем"
        /// </summary>
        /// <param name="parameter"></param>
        private void AddMeme(object parameter)
        {
            if (Name == null || Name.Replace(" ", "") == string.Empty)
            {
                MessageBox.Show("Введите имя");
                return;
            }
            if (Category == null || Category.Replace(" ", "") == string.Empty)
            {
                MessageBox.Show("Введите категорию");
                return;
            }
            if (FilePath == null)
            {
                MessageBox.Show("Выберите файл");
                return;
            }

            // Запуск события и закрытие окна
            MemeAdded?.Invoke(new Meme() { Name = Name, Category = Category, FilePath = FilePath});
            if (parameter is Window window)
                    window.Close();
        }

        /// <summary>
        /// Нажатие на фото
        /// </summary>
        /// <param name="parameter"></param>
        private void UpdateMemeImage(object parameter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
                FilePath = openFileDialog.FileName;
        }
        #endregion
    }
}
