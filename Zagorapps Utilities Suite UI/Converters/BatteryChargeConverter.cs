namespace Zagorapps.Utilities.Suite.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class BatteryChargeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                bool isCharging = bool.Parse(value.ToString());

                if (isCharging)
                {
                    return 1;
                }

                return 0.4;
            }

            return 0.4;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}