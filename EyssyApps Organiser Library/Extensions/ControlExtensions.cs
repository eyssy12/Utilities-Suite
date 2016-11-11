namespace EyssyApps.Organiser.Library.Extensions
{
    using System.Collections.Generic;

    public static class ControlExtensions
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
    }
}