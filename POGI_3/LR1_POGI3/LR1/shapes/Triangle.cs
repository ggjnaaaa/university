using System;
using System.Collections.Generic;

namespace LR1.shapes
{
    /// <summary>
    /// Класс треугольника
    /// </summary>
    internal class Triangle : Shape
    {
        public Triangle(Point2D p1, Point2D p2, Point2D p3) : base(new List<Point2D> { p1, p2, p3 }) { }

        protected override string GetAllInfo()
        {
            var result = new object[] { Points[0].X, Points[0].Y,
                                            Points[1].X, Points[1].Y,
                                            Points[2].X, Points[2].Y,
                                            GetArea(), GetPerimeter()};

            for (var i = 0; i < 8; i++)
                result[i] = Math.Round((double)result[i], 2);

            return string.Format("Координаты точек: \n({0},{1}), ({2},{3}), ({4},{5}) \nПлощадь: {6} \nПериметр: {7}", result);
        }
        
    }
}
