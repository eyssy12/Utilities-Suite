namespace File.Organiser.UI.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class ViewModelExtensions
    {
        public static bool CompareExchange<T>(this T proposedValue, ref T originalValue)
        {
            if (EqualityComparer<T>.Default.Equals(proposedValue, originalValue))
            {
                return false;
            }

            originalValue = proposedValue;

            return true;
        }

        public static void SetEnumeration<TEnum>(this object parameter, ref TEnum field)
            where TEnum : struct
        {
            TEnum value;
            if (Enum.TryParse(parameter.ToString(), out value))
            {
                field = value;
            }
        }

        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}