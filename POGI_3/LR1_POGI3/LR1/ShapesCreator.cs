using System;
using System.Windows;
using LR1.shapes;

namespace LR1
{
    /// <summary>
    /// Класс для создания фигур
    /// </summary>
    internal class ShapesCreator
    {
        private static readonly Random _random = new Random();
        /// <summary>
        /// Создаёт треугольник по рандомным точкам.
        /// </summary>
        /// <returns>Возвращает созданный треугольник.</returns>
        public static Shape CreateTriangle()
        {
            var p1 = GenerateRandomPoint(0);
            var p2 = GenerateRandomPoint(1);
            var p3 = GenerateRandomPoint(2);

            while(true)
            {
                if (((p3.X - p1.X) / (p2.X - p1.X)) == ((p3.Y - p1.Y) / (p2.Y - p1.Y)) && ((p3.X - p1.X) / (p2.X - p1.X) > 0))
                {
                    p3 = GenerateRandomPoint(2);
                }
                else break;
            }

            Shape shape = new Triangle(p1, p2, p3);
            return shape;
        }

        /// <summary>
        /// Создаёт прямоугольник по рандомным точкам.
        /// </summary>
        /// <returns>Возвращает созданный прямоугольник.</returns>
        public static Shape CreateRectangle()
        {
            var p1 = GenerateRandomPoint(0);
            var p2 = new Point2D(GenerateNextRandomNumber(250), p1.Y, 1);
            var p3 = new Point2D(p2.X, GenerateNextRandomNumber(250), 2);
            var p4 = new Point2D(p1.X, p3.Y, 3);

            Shape shape = new Rectangle(p1, p2, p3, p4);
            return shape;
        }

        /// <summary>
        /// Создаёт квадрат по рандомным точкам.
        /// </summary>
        /// <returns>Возвращает созданный квадрат</returns>
        public static Shape CreateSquare()
        {
            var size = GenerateNextRandomNumber(MaxHorizontalPointValue() / 2);

            var p1 = GenerateRandomPoint(0);
            var p2 = new Point2D(p1.X + size, p1.Y, 1);
            var p3 = new Point2D(p2.X, p2.Y + size, 2);
            var p4 = new Point2D(p1.X, p3.Y, 3);

            Shape shape = new Rectangle(p1, p2, p3, p4);
            return shape;
        }

        /// <summary>
        /// Генерирует рандомную точку.
        /// </summary>
        /// <returns>Возвращает созданную точку</returns>
        private static Point2D GenerateRandomPoint(int index) => new Point2D(GenerateNextRandomNumber(MaxHorizontalPointValue()), GenerateNextRandomNumber(MaxVerticalPointValue()), index);
        private static double GenerateNextRandomNumber(double maxValue) => (_random.NextDouble() + _random.Next((int)maxValue));
        private static double MaxVerticalPointValue() => Application.Current.MainWindow.Height - 181 - 85;
        private static double MaxHorizontalPointValue() => Application.Current.MainWindow.Width - 263 - 35;
    }
}
