using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace LR1.shapes
{
    /// <summary>
    /// Класс треугольника
    /// </summary>
    internal class Triangle : Shape
    {
        public Triangle(Point2D p1, Point2D p2, Point2D p3) : base(new List<Point2D> { p1, p2, p3 }) { }

        public override double getArea()
        {
            double p = getPerimeter() / 2.0;
            return Math.Sqrt(p * (p - Lines[0].length()) *  (p - Lines[1].length()) * (p - Lines[2].length()));
        }
        public override double getPerimeter() => (Lines[0].length() + Lines[1].length() + Lines[2].length());
        protected override string getAllInfo()
        {
            object[] result = new object[] { Points[0].getX(), Points[0].getY(),
                                            Points[1].getX(), Points[1].getY(),
                                            Points[2].getX(), Points[2].getY(),
                                            getArea(), getPerimeter()};

            for (int i = 0; i < 8; i++)
                result[i] = Math.Round((double)result[i], 2);

            return string.Format("Координаты точек: \n({0},{1}), ({2},{3}), ({4},{5}) \nПлощадь: {6} \nПериметр: {7}", result);
        }
        
    }
}
