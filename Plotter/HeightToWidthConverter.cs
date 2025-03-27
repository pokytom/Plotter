using System;
using System.Globalization;
using System.Windows.Data;

namespace Plotter
{
    public class HeightToWidthConverter : IValueConverter
    {
        public double Spacing { get; set; } = 20; // Set spacing value for margin

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height)
            {
                // You can adjust the formula based on desired spacing
                return height - Spacing * 2; // Subtracting the margin
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}