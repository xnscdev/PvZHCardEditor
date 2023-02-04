﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace PvZHCardEditor
{
    internal class RequireFalseConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type type, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                if (value is bool b && b)
                {
                    return false;
                }
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] types, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
