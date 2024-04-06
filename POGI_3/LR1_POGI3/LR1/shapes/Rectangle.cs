using System;
using System.Collections.Generic;

namespace LR1.shapes
{
    /// <summary>
    /// Класс прямоугольника
    /// </summary>
    internal class Rectangle : Shape
    {
        public Rectangle(Point2D p1, Point2D p2, Point2D p3, Point2D p4) : base(new List<Point2D> {p1, p2, p3, p4}) { }

        protected override string GetAllInfo()
        {
            var result = new object[] { Points[0].X, Points[0].Y,
                                            Points[1].X, Points[1].Y,
                                            Points[2].X, Points[2].Y,
                                            Points[3].X, Points[3].Y,
                                            GetArea(), GetPerimeter()};

            for (var i = 0; i < 10; i++)
                result[i] = Math.Round((double)result[i], 2);

            var str = string.Format("Координаты точек: \n({0} ; {1}), ({2} ; {3}), ({4} ; {5}), ({6} ; {7}) \nПлощадь: {8} \nПериметр: {9}", result);

            return str;
        }
    }
}
