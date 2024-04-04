﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LR1
{
    /// <summary>
    /// Класс для двумерной точки
    /// </summary>
    internal class Point2D : INotifyPropertyChanged
    {
        public event EventHandler PointChaged;
        public readonly int Index;
        private double _x;
        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                OnPropertyChanged(nameof(X));
                PointChaged?.Invoke(this, EventArgs.Empty);
            }
        }
        private double _y;
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
                PointChaged?.Invoke(this, EventArgs.Empty);
            }
        }
        private double lastSliderX;
        private double lastSliderY;

        public Point2D(double x, double y, int index)
        {
            _x = x;
            _y = y;
            lastSliderX = 0;
            lastSliderY = 0;

            Index = index;
        }

        /// <summary>
        /// Сдвигает точку согласно значению слайдера.
        /// </summary>
        /// <param name="shiftX">Сдвиг по X</param>
        /// <param name="shiftY">Сдвиг по Y</param>
        public void shiftSlider(double shiftX, double shiftY)
        {
            X = shiftX == 0 ? X : X + shiftX - lastSliderX;
            Y = shiftY == 0 ? Y : Y + shiftY - lastSliderY;
            lastSliderX = shiftX == 0 ? lastSliderX : shiftX;
            lastSliderY = shiftY == 0 ? lastSliderY : shiftY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Возвращает расстояние между точками.</returns>
        public double getDistance(Point2D point) => Math.Sqrt(Math.Pow(X - point.X, 2) + Math.Pow(Y - point.Y, 2));

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
