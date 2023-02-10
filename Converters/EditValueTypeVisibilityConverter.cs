using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace PvZHCardEditor.Converters
{
    internal class EditValueTypeVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (EditValueType)value;
            var neededType = (EditValueType)parameter;
            return type == neededType ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
