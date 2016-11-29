﻿namespace Zagorapps.Utilities.Suite.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class HasValueToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string data = value.ToString();

                return false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}