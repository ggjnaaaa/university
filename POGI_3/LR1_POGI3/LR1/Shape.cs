using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LR1
{
    /// <summary>
    /// Абстрактный класс для фигуры
    /// </summary>
    internal abstract class Shape
    {
        public event EventHandler ShapeClicked;
        protected readonly Drawer drawer;

        // Поля/свойства информации о фигуре.
        private double _lineThikness;
        /// <summary>
        /// Толщина линии (также при изменении меняет толщину у всех линий внутри себя)
        /// </summary>
        internal double LineThickness
        {
            get { return _lineThikness; }
            set
            {
                _lineThikness = value;
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
        public string AllInfo { get => getAllInfo(); }
        protected internal List<Point2D> Points { get; protected set; }
        public List<DecoratedLine> Lines { get; set; }

        protected Shape(List<Point2D> points)
        {
            Points = points;
            drawer = new Drawer();
            LineThickness = 3;

            Lines= new List<DecoratedLine>();
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
    }
}
