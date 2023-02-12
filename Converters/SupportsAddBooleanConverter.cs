using System.Globalization;
using System;
using System.Windows.Data;

namespace PvZHCardEditor.Converters
{
    internal class SupportsAddBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var node = (ComponentNode?)value;
            if (node is null || node.Value is null)
                return false;
            return node.AllowAdd && node.Value.AddValueType != ValueTargetType.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
