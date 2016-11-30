namespace Zagorapps.Utilities.Suite.UI.Extensions
{
    using System;

    public static class NavigatableExtensions
    {
        public static string GenerateIdentity<TType>()
        {
            return typeof(TType).GenerateIdentity();
        }

        public static string GenerateIdentity(this Type type)
        {
            return type.FullName;
        }
    }
}