using KR.Commands;
using KR.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace KR.ViewModels
{
    /// <summary>
    /// Вьюмодель для работы с главным окном
    /// </summary>
    internal class MemeCatalogVM : VMBase
    {
        public MemeCatalogVM()
        {
            AddMemeCommand = new RelayCommand(AddMeme);
            DeleteMemeCommand = new RelayCommand(RemoveMeme);
            SaveCommand = new RelayCommand(SaveListToJson);

            memes = MemeRepository.LoadMemes();
            Categories = MemeRepository.GetUniqueCategories(memes);

            FilteredMemes = CollectionViewSource.GetDefaultView(memes);
            FilteredMemes.Filter = FilterMemes;  // Устанавливаем фильтр
            FilteredMemes.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending)); // Сортируем по имени
        }

        #region Свойства
        public List<string> _categories;
        public List<string> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Meme> memes { get; }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                FilteredMemes.Refresh(); // Обновляем фильтр при изменении категории
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilteredMemes.Refresh(); // Обновляем фильтр при изменении текста поиска
            }
        }

        private ICollectionView _filteredMemes;
        public ICollectionView FilteredMemes
        {
            get => _filteredMemes;
            set
            {
                _filteredMemes = value;
                OnPropertyChanged();
            }
        }

        private Meme _selectedMeme;
        public Meme SelectedMeme
        {
            get => _selectedMeme;
            set
            {
                _selectedMeme = value;
                OnPropertyChanged();
                if (value != null)
                    SelectedMemeWasUpdated();
                else
                    MemeFilePath = null;
            }
        }

        public string MemeFilePath
        {
            get => SelectedMeme == null ? "Выберите картинку в списке слева" : SelectedMeme.FilePath;
            set
            {
                OnPropertyChanged();
                OnPropertyChanged(nameof(MemeImage));
            }
        }

        public BitmapImage MemeImage
        {
            get => File.Exists(MemeFilePath) ? GetBitmapImageFromPath(MemeFilePath) : BlankPic;
            set => OnPropertyChanged();
        }

        public ICommand AddMemeCommand { get; }
        public ICommand DeleteMemeCommand { get; }
        public ICommand SaveCommand { get; }
        #endregion

        #region Обработка нажатий
        /// <summary>
        /// Нажатие на "Удалить"
        /// </summary>
        /// <param name="parameter"></param>
        private void RemoveMeme(object parameter)
        {
            MemeRepository.RemoveMeme(SelectedMeme, memes);
            Categories = MemeRepository.GetUniqueCategories(memes);
        }

        /// <summary>
        /// Нажатие на "Добавить"
        /// </summary>
        /// <param name="parameter"></param>
        private void AddMeme(object parameter)
        {
            var addMemeViewModel = new MemeAddVM();
            addMemeViewModel.MemeAdded += (meme) => memes.Add(meme);

            var addMemeWindow = new MemeAddWindow
            {
                DataContext = addMemeViewModel
            };
            addMemeWindow.ShowDialog();
            Categories = MemeRepository.GetUniqueCategories(memes);
        }

        /// <summary>
        /// Нажатие на "Сохранить изменения"
        /// </summary>
        /// <param name="parameter"></param>
        private void SaveListToJson(object parameter)
        {
            MemeRepository.SaveMemes(memes);
        }
        #endregion
        /// <summary>
        /// Действия при обновлении выбранного мема
        /// </summary>
        private void SelectedMemeWasUpdated() => MemeFilePath = SelectedMeme.FilePath;

        /// <summary>
        /// Фильтр для FilteredMemes
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool FilterMemes(object obj)
        {
            if (obj is Meme meme)
            {
                bool categoryMatches = true;
                if (SelectedCategory != "Показать все")
                    categoryMatches = string.IsNullOrEmpty(SelectedCategory) || meme.Category == SelectedCategory;
                bool nameMatches = string.IsNullOrEmpty(SearchText) || meme.Name.ToLower().Contains(SearchText.ToLower());
                return categoryMatches && nameMatches;
            }

            return false;
        }
    }
}
