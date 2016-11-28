namespace Zagorapps.Utilities.Suite.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class SliderValueToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch(parameter.ToString())
                {
                    case "InitialWaitTime":
                        return "Initial Wait Time (" + value.ToString() + " seconds)";
                    case "Interval":
                        return "Interval (" + value.ToString() + " seconds)";
                }

                return value.ToString() + " seconds";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}