using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LR1
{
    /// <summary>
    /// Класс для двумерной точки
    /// </summary>
    internal class Point2D : INotifyPropertyChanged
    {
        public event EventHandler PointChanged;
        /// <summary>
        /// Индекс в массиве Point объекта Shape
        /// </summary>
        public readonly int Index;
        private double _x;
        /// <summary>
        /// Положение точки по оси X
        /// </summary>
        public double X
        {
            get => _x;
            set
            {
                _x = value;
                OnPropertyChanged();
                PointChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private double _y;
        /// <summary>
        /// Положение точки по оси Y
        /// </summary>
        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged();
                PointChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private double _lastSliderX;
        private double _lastSliderY;

        // Конструктор
        public Point2D(double x, double y, int index)
        {
            _x = x;
            _y = y;
            _lastSliderX = 0;
            _lastSliderY = 0;

            Index = index;
        }

        /// <summary>
        /// Сдвигает точку согласно значению слайдера.
        /// </summary>
        /// <param name="shiftX">Сдвиг по X</param>
        /// <param name="shiftY">Сдвиг по Y</param>
        public void ShiftSlider(double shiftX, double shiftY)
        {
            X = shiftX == 0 ? X : X + shiftX - _lastSliderX;
            Y = shiftY == 0 ? Y : Y + shiftY - _lastSliderY;
            _lastSliderX = shiftX == 0 ? _lastSliderX : shiftX;
            _lastSliderY = shiftY == 0 ? _lastSliderY : shiftY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Возвращает расстояние между точками.</returns>
        public double GetDistance(Point2D point) => Math.Sqrt(Math.Pow(X - point.X, 2) + Math.Pow(Y - point.Y, 2));

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
