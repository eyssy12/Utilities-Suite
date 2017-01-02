namespace Zagorapps.Utilities.Suite.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using Zagorapps.Utilities.Suite.Library;

    public class TaskTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                TaskType type = (TaskType)value;

                switch (type)
                {
                    case TaskType.Organiser:
                        return Visibility.Collapsed;
                    default:
                        return Visibility.Visible;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}