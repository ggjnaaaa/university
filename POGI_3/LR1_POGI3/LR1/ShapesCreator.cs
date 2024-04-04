using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using LR1.shapes;

namespace LR1
{
    /// <summary>
    /// Класс для создания фигур
    /// </summary>
    internal class ShapesCreator : INotifyPropertyChanged
    {
        private Random _random;

        // Конструктор
        public ShapesCreator()
        {
            _random = new Random();
        }

        /// <summary>
        /// Создаёт треугольник по рандомным точкам.
        /// </summary>
        /// <returns>Возвращает созданный треугольник.</returns>
        public Shape createTriangle()
        {
            Point2D p1 = generateRandomPoint(0);
            Point2D p2 = generateRandomPoint(1);
            Point2D p3 = generateRandomPoint(2);

            while(true)
            {
                if (((p3.X - p1.X) / (p2.X - p1.X)) == ((p3.Y - p1.Y) / (p2.Y - p1.Y)) && ((p3.X - p1.X) / (p2.X - p1.X) > 0))
                {
                    p3 = generateRandomPoint(2);
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
        public Shape createRectangle()
        {
            Point2D p1 = generateRandomPoint(0);
            Point2D p2 = new Point2D(generateNextRandomNumber(250), p1.Y, 1);
            Point2D p3 = new Point2D(p2.X, generateNextRandomNumber(250), 2);
            Point2D p4 = new Point2D(p1.X, p3.Y, 3);

            Shape shape = new Rectangle(p1, p2, p3, p4);
            return shape;
        }

        /// <summary>
        /// Создаёт квадрат по рандомным точкам.
        /// </summary>
        /// <returns>Возвращает созданный квадрат</returns>
        public Shape createSquare()
        {
            double size = generateNextRandomNumber(maxHorizontalPointValue() / 2);

            Point2D p1 = generateRandomPoint(0);
            Point2D p2 = new Point2D(p1.X + size, p1.Y, 1);
            Point2D p3 = new Point2D(p2.X, p2.Y + size, 2);
            Point2D p4 = new Point2D(p1.X, p3.Y, 3);

            Shape shape = new Rectangle(p1, p2, p3, p4);
            return shape;
        }

        /// <summary>
        /// Генерирует рандомную точку.
        /// </summary>
        /// <returns>Возвращает созданную точку</returns>
        private Point2D generateRandomPoint(int index) => new Point2D(generateNextRandomNumber(maxHorizontalPointValue()), generateNextRandomNumber(maxVerticalPointValue()), index);
        private double generateNextRandomNumber(double maxValue) => (_random.NextDouble() + _random.Next((int)maxValue));
        private double maxVerticalPointValue() => Application.Current.MainWindow.Height - 180 - 34;
        private double maxHorizontalPointValue() => Application.Current.MainWindow.Width - 262 - 34;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
