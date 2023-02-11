using System.Globalization;
using System;
using System.Windows.Data;

namespace PvZHCardEditor.Converters
{
    internal class SupportsAddBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((ComponentNode?)value)?.Value?.SupportsAdd ?? false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
