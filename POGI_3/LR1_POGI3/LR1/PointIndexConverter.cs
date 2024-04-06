using System;
using System.Globalization;
using System.Windows.Data;

namespace LR1
{
    internal class PointIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is Point2D point ? $"p{point.Index + 1}" : string.Empty;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
