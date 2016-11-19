namespace EyssyApps.Core.Library.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerationExtensions
    {
        public static IEnumerable<T> GetValues<T>(this Type type)
        {
            return type.GetValues<T>(new T[0]);
        }

        public static IEnumerable<T> GetValues<T>(this Type type, params T[] exclusions)
        {
            return Enum.GetValues(type).Cast<T>().Except(exclusions).ToArray();
        }

        public static IEnumerable<T> GetValues<T>(this object requester)
        {
            return typeof(T).GetValues<T>();
        }

        public static IEnumerable<T> GetValues<T>(this object requester, params T[] exclusions)
        {
            return typeof(T).GetValues(exclusions);
        }
    }
}