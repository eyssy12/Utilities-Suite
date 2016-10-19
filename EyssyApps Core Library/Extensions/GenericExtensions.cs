namespace EyssyApps.Core.Library.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class GenericExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }
    }
}