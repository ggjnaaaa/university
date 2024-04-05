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
        public event EventHandler ShapeClicked;
        protected readonly Drawer drawer;

        // Поля/свойства информации о фигуре.
        private double _lineThickness;
        /// <summary>
        /// Толщина линии (также при изменении меняет толщину у всех линий внутри себя)
        /// </summary>
        internal double LineThickness
        {
            get { return _lineThickness; }
            set
            {
                _lineThickness = value;
                if (Lines != null)
                    foreach (DecoratedLine line in Lines) line.Thickness = value;
            }
        }
        private Brush _color;
        /// <summary>
        /// Цвет линии (также при изменении меняет цвет у всех линий внутри себя)
        /// </summary>
        internal Brush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                foreach (DecoratedLine line in Lines) line.changeColor(value);
            }
        }
        // Возвращает всю информацию о фигуре, используется для биндинга текста с информацией
        public string AllInfo
        {
            get
            {
                return getAllInfo();
            }
            private set
            {
                OnPropertyChanged(nameof(AllInfo));
            }
        }
        public List<Point2D> Points { get; protected set; }
        internal List<DecoratedLine> Lines { get; set; }

        protected Shape(List<Point2D> points)
        {
            Points = points;
            drawer = new Drawer();
            LineThickness = 3;

            Lines= new List<DecoratedLine>();
            foreach (Point2D point in points) point.PointChanged += updateInfo;
            drawShape();
        }

        /// <summary>
        /// Рисует/перерисовывает фигуру
        /// </summary>
        public void drawShape() => drawer.drawShape(this);
        /// <summary>
        /// Смещает фигуру по слайдерам
        /// </summary>
        /// <param name="shiftX"></param>
        /// <param name="shiftY"></param>
        public void shiftSlider(double shiftX, double shiftY)
        {
            foreach (Point2D pt in Points) pt.shiftSlider(shiftX, shiftY);
            drawShape();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Возвращает периметр</returns>
        public abstract double getPerimeter();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Возвращает площадь</returns>
        public abstract double getArea();

        private void updateInfo(object sender, EventArgs args) => AllInfo = "";
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Возвращает всю информацию о фигуре</returns>
        protected abstract string getAllInfo();

        internal void shapeClicked(object sender, EventArgs e)
        {
            Color = Brushes.Blue;
            ShapeClicked?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
