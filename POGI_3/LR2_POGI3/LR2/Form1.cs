using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LR2
{
    public partial class Form1 : Form
    {
        private string dataFolderPath = "GalleryImages";
        private Queue<string> _imageQueue;

        public Form1()
        {
            InitializeComponent();
            _imageQueue = new Queue<string>();
            LoadImagesFromFolder();
            ShowNextImg();
            this.Shown += new EventHandler(MainForm_Shown);
        }


        private void MainForm_Shown(object sender, EventArgs e)
        {
            UpdateView();
            RefreshForm();
        }

        /// <summary>
        /// Обновляет внешний вид при запуске приложения
        /// </summary>
        private void UpdateView()
        {
            // Цвет кнопок
            AddPhotoBtn.BackColor = Color.FromArgb(60, 60, 60);
            ChangeShapeBtn.BackColor = Color.FromArgb(60, 60, 60);

            // Цвет шрифта на кнопках
            AddPhotoBtn.ForeColor = Color.FromArgb(255, 255, 255);
            ChangeShapeBtn.ForeColor = Color.FromArgb(255, 255, 255);

            // Цвет рамок кнопок
            AddPhotoBtn.FlatAppearance.BorderColor = Color.FromArgb(110, 110, 110);
            ChangeShapeBtn.FlatAppearance.BorderColor = Color.FromArgb(110, 110, 110);

            AddPhotoBtn.Invalidate();
            AddPhotoBtn.Update();
            ChangeShapeBtn.Invalidate();
            ChangeShapeBtn.Update();

            // Цвет окна
            this.BackColor = Color.FromArgb(46, 46, 46);
        }

        /// <summary>
        /// Обновляет внешний вид кнопок и окна после загрузки
        /// </summary>
        private void RefreshForm()
        {
            // Обновление формы
            this.Invalidate();
            this.Update();

            // Обновление кнопок
            AddPhotoBtn.Invalidate();
            AddPhotoBtn.Update();
            ChangeShapeBtn.Invalidate();
            ChangeShapeBtn.Update();
        }

        /// <summary>
        /// Загружает пути к файлу в очередь из папки GalleryImages.
        /// </summary>
        private void LoadImagesFromFolder()
        {
            if (Directory.Exists(dataFolderPath))
            {
                // Заполнение очереди фотографий
                foreach (var imagePath in Directory.GetFiles(dataFolderPath))
                {
                    try
                    {
                        _imageQueue.Enqueue(imagePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения {imagePath}: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Не найдена папка GalleryImages рядом с исполняемым файлом.");
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки смены формы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeShapeBtn_Click(object sender, EventArgs e)
        {
            var gallery = Gallery;
            Controls.Remove(Gallery);  // Удаление кнопки из формы

            // Создание кнопки другой формы
            HoverButton newButton;
            if (gallery is EllipseHoverButton btn)
                newButton = new HoverButton(gallery);
            else
                newButton = new EllipseHoverButton(gallery);
            newButton.MouseClick += Gallery_Click;

            // Добавление кнопки в форму
            Gallery = newButton;
            Controls.Add(newButton);
        }

        /// <summary>
        /// Обработчик нажатия кнопки добавления фотографий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Изображения (*.jpg, *.jpeg, *.png, *.gif, *.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var fileName in openFileDialog.FileNames)
                {
                    try
                    {
                        _imageQueue.Enqueue(fileName);
                        SaveGalleryImage(fileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки изображения {fileName}: {ex.Message}");
                    }
                }

                ShowNextImg();
            }
        }

        /// <summary>
        /// Сохраняет вбранные файлы в папку GalleryImages.
        /// </summary>
        /// <param name="fileName"></param>
        private void SaveGalleryImage(string fileName)
        {
            var image = Image.FromFile(fileName);
            var imagePath = Path.Combine(dataFolderPath, $"{Guid.NewGuid()}.jpg");
            image.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        /// <summary>
        /// Обработчик нажатия на галерею.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Gallery_Click(object sender, EventArgs e) => ShowNextImg();

        /// <summary>
        /// Обновляет фотографию на кнопке.
        /// </summary>
        private void ShowNextImg()
        {
            if (_imageQueue.Count > 0)
            {
                var nextImageFileName = _imageQueue.Dequeue();
                var nextImage = Image.FromFile(nextImageFileName);
                var fileInfo = new FileInfo(nextImageFileName);
                _imageQueue.Enqueue(nextImageFileName);

                Gallery.UpdateImg(nextImage,
                        $"\n{Path.GetFileNameWithoutExtension(fileInfo.Name)}" +
                        $"\nДата создания: {fileInfo.CreationTime.ToString("dd.MM.yyyy")}");
            }
            else
            {
                MessageBox.Show("Нет изображений в папке GalleryImages.");
            }
        }
    }
}