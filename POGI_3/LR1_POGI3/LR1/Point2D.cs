using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR1
{
    /// <summary>
    /// Класс для двумерной точки
    /// </summary>
    internal class Point2D
    {
        private double x;
        private double y;
        private double lastSliderX;
        private double lastSliderY;

        public Point2D(double x, double y)
        {
            this.x = x;
            this.y = y;
            lastSliderX = 0;
            lastSliderY = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Возвращает положение точки по X</returns>
        public double getX() => x;
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Возвращает положение точки по Y</returns>
        public double getY() => y;

        /// <summary>
        /// Сдвигает точку согласно значению слайдера.
        /// </summary>
        /// <param name="shiftX">Сдвиг по X</param>
        /// <param name="shiftY">Сдвиг по Y</param>
        public void shiftSlider(double shiftX, double shiftY)
        {
            x = shiftX == 0 ? x : x + shiftX - lastSliderX;
            y = shiftY == 0 ? y : y + shiftY - lastSliderY;
            lastSliderX = shiftX == 0 ? lastSliderX : shiftX;
            lastSliderY = shiftY == 0 ? lastSliderY : shiftY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Возвращает расстояние между точками.</returns>
        public double getDistance(Point2D point) => Math.Sqrt(Math.Pow(x - point.x, 2) + Math.Pow(y - point.y, 2));
    }
}
