using System;
using System.Globalization;
using System.Windows.Data;

namespace PvZHCardEditor.Converters
{
    internal class RadioCheckedEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Enum)value).Equals((Enum)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? parameter : Enum.GetValues(targetType).GetValue(0)!;
        }
    }
}
