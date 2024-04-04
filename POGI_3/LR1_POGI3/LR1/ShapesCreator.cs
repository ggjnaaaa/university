﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using LR1.shapes;

namespace LR1
{
    /// <summary>
    /// Класс для создания фигур
    /// </summary>
    internal class ShapesCreator : INotifyPropertyChanged
    {
        private Random random;

        // Конструктор
        public ShapesCreator()
        {
            random = new Random();
        }

        /// <summary>
        /// Создаёт треугольник по рандомным точкам.
        /// </summary>
        /// <returns>Возвращает созданный треугольник.</returns>
        public Shape createTriange()
        {
            Point2D p1 = generateRandomPoint();
            Point2D p2 = generateRandomPoint();
            Point2D p3 = generateRandomPoint();

            while(true)
            {
                if (((p3.getX() - p1.getX()) / (p2.getX() - p1.getX())) == ((p3.getY() - p1.getY()) / (p2.getY() - p1.getY())) && ((p3.getX() - p1.getX()) / (p2.getX() - p1.getX()) > 0))
                {
                    p3 = generateRandomPoint();
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
            Point2D p1 = generateRandomPoint();
            Point2D p2 = new Point2D(generateNextRandomNumber(250), p1.getY());
            Point2D p3 = new Point2D(p2.getX(), generateNextRandomNumber(250));
            Point2D p4 = new Point2D(p1.getX(), p3.getY());

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

            Point2D p1 = generateRandomPoint();
            Point2D p2 = new Point2D(p1.getX() + size, p1.getY());
            Point2D p3 = new Point2D(p2.getX(), p2.getY() + size);
            Point2D p4 = new Point2D(p1.getX(), p3.getY());

            Shape shape = new Rectangle(p1, p2, p3, p4);
            return shape;
        }

        /// <summary>
        /// Генерирует рандомную точку.
        /// </summary>
        /// <returns>Возвращает созданную точку</returns>
        private Point2D generateRandomPoint() => new Point2D(generateNextRandomNumber(maxHorizontalPointValue()), generateNextRandomNumber(maxVerticalPointValue()));
        private double generateNextRandomNumber(double maxValue) => (random.NextDouble() + random.Next((int)maxValue));
        private double maxVerticalPointValue() => Application.Current.MainWindow.Height - 180 - 34;
        private double maxHorizontalPointValue() => Application.Current.MainWindow.Width - 262 - 34;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}