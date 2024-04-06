using System.Linq;

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
        public void DrawShape(Shape shape)
        {
            var lines = shape.Lines;
            var points = shape.Points;
            if (!lines.Any())
            {
                for (var i = 0; i < points.Count - 1; i++) shape.Lines.Add(DrawLine(shape, points[i], points[i + 1]));
                shape.Lines.Add(DrawLine(shape, points[points.Count - 1], points[0]));
            }
            else
            {
                for (var i = 0; i < points.Count - 1; i++) lines[i].UpdatePoints(points[i], points[i + 1]);
                lines[points.Count - 1].UpdatePoints(points[points.Count - 1], points[0]);
            }
        }

        /// <summary>
        /// Рисует линию и подписывает фигуру на событие её нажатия.
        /// </summary>
        /// <param name="shape">Фигура-родитель</param>
        /// <param name="from">Начальная точка</param>
        /// <param name="to">Конечная точка</param>
        /// <returns>Возвращает готовую линию.</returns>
        private static DecoratedLine DrawLine(Shape shape, Point2D from, Point2D to)
        {
            var line = new DecoratedLine(shape, from, to);
            line.LineClick += shape.ShapeClicked;
            return line;
        }
    }
}
