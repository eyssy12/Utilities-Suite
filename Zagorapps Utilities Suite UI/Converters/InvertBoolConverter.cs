namespace Zagorapps.Utilities.Suite.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool original = (bool)value;

            return !original;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool original = (bool)value;
            return !original;
        }
    }
}