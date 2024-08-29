using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace WorkerCard
{
    internal class HeightWithMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FrameworkElement element)
            {
                // Высота элемента с учетом маргинов
                return element.ActualHeight + element.Margin.Top + element.Margin.Bottom;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
