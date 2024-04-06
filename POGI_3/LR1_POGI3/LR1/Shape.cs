using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace LR1
{
    /// <summary>
    /// Абстрактный класс для фигуры
    /// </summary>
    internal abstract class Shape : INotifyPropertyChanged
    {
        public event EventHandler ShapeClick;
        protected readonly Drawer Drawer;

        // Поля/свойства информации о фигуре.
        private double _lineThickness;
        /// <summary>
        /// Толщина линии (также при изменении меняет толщину у всех линий внутри себя)
        /// </summary>
        internal double LineThickness
        {
            get => _lineThickness;
            set
            {
                _lineThickness = value;
                if (Lines != null)
                    foreach (var line in Lines) line.Thickness = value;
            }
        }
        private Brush _color;
        /// <summary>
        /// Цвет линии (также при изменении меняет цвет у всех линий внутри себя)
        /// </summary>
        internal Brush Color
        {
            get => _color;
            set
            {
                _color = value;
                if (Lines != null)
                    foreach (var line in Lines) line.ChangeColor(value);
            }
        }
        // Возвращает всю информацию о фигуре, используется для биндинга текста с информацией
        public string AllInfo
        {
            get => GetAllInfo();
            private set => OnPropertyChanged();
        }
        public List<Point2D> Points { get; protected set; }
        internal List<DecoratedLine> Lines { get; set; }

        protected Shape(List<Point2D> points)
        {
            Points = points;
            Drawer = new Drawer();
            LineThickness = 3;

            Lines= new List<DecoratedLine>(); 
            foreach (var point in points) point.PointChanged += UpdateInfo;
            DrawShape();
        }

        /// <summary>
        /// Рисует/перерисовывает фигуру
        /// </summary>
        public void DrawShape() => Drawer.DrawShape(this);
        /// <summary>
        /// Смещает фигуру по слайдерам
        /// </summary>
        /// <param name="shiftX"></param>
        /// <param name="shiftY"></param>
        public void ShiftSlider(double shiftX, double shiftY)
        {
            foreach (var pt in Points) pt.ShiftSlider(shiftX, shiftY);
            DrawShape();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Возвращает периметр</returns>
        public double GetPerimeter()
        {
            double result = 0;
            foreach (var line in Lines)
                result += line.Length();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Возвращает площадь</returns>
        public double GetArea()
        {
            double area = 0;

            for (var i = 0; i < Points.Count; i++)
            {
                var j = (i + 1) % Points.Count;
                area += Points[i].X * Points[j].Y - Points[j].X * Points[i].Y;
            }

            return Math.Abs(area / 2);
        }

        private void UpdateInfo(object sender, EventArgs args) => AllInfo = "";
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Возвращает всю информацию о фигуре</returns>
        protected abstract string GetAllInfo();

        internal void ShapeClicked(object sender, EventArgs e)
        {
            Color = Brushes.Blue;
            ShapeClick?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
