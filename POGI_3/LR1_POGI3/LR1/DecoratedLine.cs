using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LR1
{
    /// <summary>
    /// Класс-декоратор для класса Line.
    /// Сделан для привязки точек и фигуры-родителя к определённой линии.
    /// </summary>
    internal class DecoratedLine
    {
        public event EventHandler LineClicked;
        public readonly Shape parentShape;
        public Line Line { get; private set; }
        public Point2D Point1 { get; private set; }
        public Point2D Point2 { get; private set; }
        private double _thickness;
        public double Thickness
        {
            get { return _thickness; }
            set
            {
                _thickness = value;
                Line.StrokeThickness = _thickness;
            }
        }


        public DecoratedLine(Shape parentShape, Point2D p1, Point2D p2)
        {
            this.parentShape = parentShape;

            Line = new Line();
            Line.Stroke = Brushes.Red;
            Line.MouseLeftButtonDown += LineClickedHandler;
            Thickness = parentShape.LineThickness;
            updatePoints(p1, p2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Возвращает длину линии.</returns>
        public double length()
        {
            return Point1.getDistance(Point2);
        }

        /// <summary>
        /// Меняет цвет линии.
        /// </summary>
        /// <param name="newColor"></param>
        public void changeColor(Brush newColor) => Line.Stroke = newColor;

        /// <summary>
        /// Обновляет точки как у линии, так и у декоратора
        /// </summary>
        /// <param name="from">Начальная точка линии</param>
        /// <param name="to">Конечная точка линии</param>
        public void updatePoints(Point2D from, Point2D to)
        {
            Line.X1 = from.getX(); Line.Y1 = from.getY(); Line.X2 = to.getX(); Line.Y2 = to.getY();
            Point1 = from;
            Point2 = to;
        }

        private void LineClickedHandler(object sender, MouseButtonEventArgs e)
        {
            LineClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
