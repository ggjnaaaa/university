using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR2
{
    internal class HoverButton : Button
    {
        private const int ANIMATIONSPEED = 10;
        private const int MAXPANELHEIGHT = 150;
        private const int REDUCINGBUTTON = 20;

        protected int _currentPanelHeight;
        protected bool _imgHasBeenUpdated;
        private bool _isPanelUp;
        protected bool _isPanelDown;

        protected Image _currentImage;
        protected string _text;

        /// <summary>
        /// Создаёт новую кнопку.
        /// </summary>
        public HoverButton()
        {
            _currentPanelHeight = 1;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
        }

        /// <summary>
        /// Создаёт новую кнопку на основе старой.
        /// </summary>
        /// <param name="lastButton">Старая кнопка.</param>
        public HoverButton(HoverButton lastButton) : this()
        {
            _currentImage = lastButton._currentImage;
            _imgHasBeenUpdated = true;
            _text = lastButton._text;

            Width = lastButton.Width;
            Height = lastButton.Height;
            Location = lastButton.Location;
        }

        /// <summary>
        /// Обновляет фотографию на кнопке.
        /// </summary>
        /// <param name="img">Новая фотография.</param>
        /// <param name="info">Новая информаия о фотографии.</param>
        public void UpdateImg(Image img, string info)
        {
            _currentImage = img;
            _text = info;
            _imgHasBeenUpdated = true;

            DrawImg();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (_currentImage != null && _imgHasBeenUpdated) DrawImg();

            if (_currentPanelHeight > 1) // Шторка для текста только если высота больше 1
            {
                // Добавление шторки с градиентом
                using (var gradientBrush = new LinearGradientBrush(
                                    new Rectangle(0, 0, Width, _currentPanelHeight),
                                    Color.Black, Color.Transparent,
                                    LinearGradientMode.Vertical))
                {
                    pe.Graphics.FillRectangle(gradientBrush, new Rectangle(0, 0, Width, _currentPanelHeight));
                }

                // Добавление текста
                using (var stringFormat = new StringFormat())
                {
                    Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, 0);

                    stringFormat.Alignment = StringAlignment.Center;
                    pe.Graphics.DrawString(_text, this.Font, new SolidBrush(Color.Linen),
                        new Rectangle(Width / 4, 0, Width / 2, _currentPanelHeight),
                        stringFormat);
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            var graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new Rectangle(0, 0, Width, Height));
            Region = new Region(graphicsPath);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            DownPanel();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            UpPanel();
        }
        
        /// <summary>
        /// Асинхронно поднимает панель для текста.
        /// </summary>
        protected async void DownPanel()
        {
            _isPanelUp = true;
            _isPanelDown = false;

            // Обновление внешнего вида пока шторка не опустится полностью или пока мышка не уйдёт с кнопки
            while (_currentPanelHeight < MAXPANELHEIGHT && !_isPanelDown)
            {
                await Task.Delay(1);

                // Обновление текущего размера шторки
                _currentPanelHeight += MAXPANELHEIGHT / ANIMATIONSPEED;
                if (_currentPanelHeight > MAXPANELHEIGHT) _currentPanelHeight = MAXPANELHEIGHT;
                _imgHasBeenUpdated = true;

                // Обновление положения кнопки
                var reductionByX = REDUCINGBUTTON / ANIMATIONSPEED;
                var reductionByY = REDUCINGBUTTON / ANIMATIONSPEED;
                Width -= reductionByX;
                Height -= reductionByY;
                Location = new Point(Convert.ToInt32(Location.X + reductionByX / 2 ) , Convert.ToInt32(Location.Y + reductionByY / 2 ));
            }
        }

        /// <summary>
        /// Асинхронно опускает панель для текста.
        /// </summary>
        protected async void UpPanel()
        {
            _isPanelDown = true;
            _isPanelUp = false;

            // Обновление внешнего вида пока шторка не поднимется полностью или пока мышка вернётся на кнопку
            while (_currentPanelHeight > 1 && !_isPanelUp)
            {
                await Task.Delay(1);

                // Обновление текущего размера шторки
                _currentPanelHeight -= MAXPANELHEIGHT / ANIMATIONSPEED;
                if (_currentPanelHeight < 1) _currentPanelHeight = 1;
                _imgHasBeenUpdated = true;

                // Обновление положения кнопки
                var reductionByX = REDUCINGBUTTON / ANIMATIONSPEED;
                var reductionByY = REDUCINGBUTTON / ANIMATIONSPEED;
                Width += reductionByX;
                Height += reductionByY;
                Location = new Point(Convert.ToInt32(Location.X - reductionByX / 2 ), Convert.ToInt32(Location.Y - reductionByY / 2 ));
            }
        }

        /// <summary>
        /// Рисует изображение из _currentImage.
        /// </summary>
        protected void DrawImg()
        {
            double ratio;
            Bitmap scaledImage;

            // Расчёт соотношения
            if (Width > Height)
            {
                ratio = (double)Width / _currentImage.Width;
                scaledImage = new Bitmap(_currentImage, Width, (int)(_currentImage.Height * ratio));
            }
            else
            {
                ratio = (double)Height / _currentImage.Height;
                scaledImage = new Bitmap(_currentImage, (int)(_currentImage.Width * ratio), Height);
            }

            // Расчёт размера изображения подогнанного под размеры кнопки
            var buttonImage = new Bitmap(Width, Height);
            using (var g = Graphics.FromImage(buttonImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(scaledImage, (int)(Width - _currentImage.Width * ratio) / 2, (int)(Height - _currentImage.Height * ratio) / 2);
            }
            BackgroundImage = buttonImage;
            BackgroundImageLayout = ImageLayout.Center;
        }
    }
}
