using System.Globalization;
using System;
using System.Windows.Data;
using PvZHCardEditor.Components;

namespace PvZHCardEditor.Converters
{
    internal class IsEffectEntityBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var node = (ComponentNode?)value;
            if (node is null)
                return false;
            return node.Parent?.ComponentName == nameof(EffectEntitiesDescriptor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
