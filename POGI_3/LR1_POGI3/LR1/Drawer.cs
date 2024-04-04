using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LR1
{
    /// <summary>
    /// Класс для рисования фигур
    /// </summary>
    internal class Drawer
    {
        /// <summary>
        /// Рисование (также обновление) фигуры.
        /// </summary>
        /// <param name="shape">Фигура которую нужно нарисовать (обновить)</param>
        public void drawShape(Shape shape)
        {
            List<DecoratedLine> lines = shape.Lines;
            List<Point2D> points = shape.Points;
            if (lines.Count() <= 0)
            {
                for (int i = 0; i < points.Count - 1; i++) shape.Lines.Add(drawLine(shape, points[i], points[i + 1]));
                shape.Lines.Add(drawLine(shape, points[points.Count - 1], points[0]));
            }
            else
            {
                for (int i = 0; i < points.Count - 1; i++) lines[i].updatePoints(points[i], points[i + 1]);
                lines[points.Count - 1].updatePoints(points[points.Count - 1], points[0]);
            }
        }

        /// <summary>
        /// Рисует линию и подписывает фигуру на событие её нажатия.
        /// </summary>
        /// <param name="shape">Фигура-родитель</param>
        /// <param name="from">Начальная точка</param>
        /// <param name="to">Конечная точка</param>
        /// <returns>Возвращает готовую линию.</returns>
        private DecoratedLine drawLine(Shape shape, Point2D from, Point2D to)
        {
            DecoratedLine line = new DecoratedLine(shape, from, to);
            line.LineClicked += shape.shapeClicked;
            return line;
        }
    }
}
