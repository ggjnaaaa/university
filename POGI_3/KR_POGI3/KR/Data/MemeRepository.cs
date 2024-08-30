using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace KR.Data
{
    /// <summary>
    /// Класс работы с .json файлом для записи в Meme
    /// </summary>
    internal class MemeRepository
    {
        private readonly string _filePath;

        public MemeRepository(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Загружает мемы в ObservableCollection из .json файла
        /// </summary>
        /// <returns>Список мемов</returns>
        public ObservableCollection<Meme> LoadMemes()
        {
            if (!File.Exists(_filePath))
                return new ObservableCollection<Meme>();

            var json = File.ReadAllText(_filePath);
            var memes = JsonConvert.DeserializeObject<ObservableCollection<Meme>>(json);
            memes = memes ?? new ObservableCollection<Meme>();
            return memes;
        }

        /// <summary>
        /// Ищет уникальные категории в списке мемов
        /// </summary>
        /// <param name="memes">Список мемов для поиска категорий</param>
        /// <returns>Список уникальных категорий</returns>
        public List<string> GetUniqueCategories(ObservableCollection<Meme> memes)
        {
            var list = new List<string>
            {
                "Показать все"
            };
            list.AddRange(memes.Select(m => m.Category).Distinct().ToList());
            return list;
        }

        /// <summary>
        /// Ищет уникальные категории в списке мемов из файла .json
        /// </summary>
        /// <returns>Список уникальных категорий</returns>
        public List<string> GetUniqueCategories() => GetUniqueCategories(LoadMemes());

        /// <summary>
        /// Сохраняет мемы в .json файл
        /// </summary>
        /// <param name="memes">Список мемов</param>
        public void SaveMemes(ObservableCollection<Meme> memes)
        {
            var json = JsonConvert.SerializeObject(memes, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        /// <summary>
        /// Добавляет мем в список с моментальным сохранением в файл
        /// </summary>
        /// <param name="meme">Мем для добавления</param>
        /// <param name="memes">Список мемов</param>
        public void AddMeme(Meme meme, ObservableCollection<Meme> memes)
        {
            memes.Add(meme);
            SaveMemes(memes);
        }

        /// <summary>
        /// Удаляет мем из списка с моментальным сохранением в файл
        /// </summary>
        /// <param name="meme">Мем для удаления</param>
        /// <param name="memes">Список мемов</param>
        public void RemoveMeme(Meme meme, ObservableCollection<Meme> memes)
        {
            memes.Remove(meme);
            SaveMemes(memes);
        }
    }
}
