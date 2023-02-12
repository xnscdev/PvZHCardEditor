using System;
using System.Globalization;
using System.Windows.Data;

namespace PvZHCardEditor.Converters
{
    internal class SupportsDeleteBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var node = (ComponentNode?)value;
            if (node is null)
                return false;
            return node.Parent?.AllowAdd ?? true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
