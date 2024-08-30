using KR.Data;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace KR.ViewModels
{
    /// <summary>
    /// Основа для вьюмоделей.
    /// Реализовывает INotifyPropertyChanged и предоставляет работу с .json и пустым изображением
    /// </summary>
    internal abstract class VMBase : INotifyPropertyChanged
    {
        private readonly string JsonFilePath;
        protected readonly MemeRepository MemeRepository;

        protected readonly BitmapImage BlankPic = new BitmapImage(new Uri("pack://application:,,,/Resources/EmptyImage.jpg"));

        /// <summary>
        /// Делает BitmapImage из пути к файлу
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected BitmapImage GetBitmapImageFromPath(string path)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Absolute);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            image.Freeze();

            return image;
        }

        public VMBase()
        {
            JsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/data.json");
            MemeRepository = new MemeRepository(JsonFilePath);
        }

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
