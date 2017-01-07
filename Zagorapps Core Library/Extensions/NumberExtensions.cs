namespace Zagorapps.Core.Library.Extensions
{
    using System;
    
    public static class NumberExtensions
    {
        public static int ClipToRange(this int value, int min, int max)
        {
            return Math.Max(min, Math.Min(value, max));
        }
    }
}