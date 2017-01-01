namespace Zagorapps.Utilities.Suite.UI.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Library.Attributes;
    using Navigation;

    public static class NavigatableExtensions
    {
        public static string GenerateIdentity<TNavigatable>() where TNavigatable : INavigatable
        {
            return typeof(TNavigatable).GenerateIdentity();
        }

        public static string GenerateIdentity(this Type type)
        {
            return type.FullName;
        }

        public static IEnumerable<Tuple<SuiteAttribute, bool>> GetAllSuitesOrderByDefaultNavigatable(this Assembly assembly, params string[] exclusions)
        {
            return assembly
                .GetTypes()
                .Where(t => t.IsDefined(typeof(SuiteAttribute), false))
                .Select(t => new Tuple<SuiteAttribute, bool>(t.GetCustomAttribute<SuiteAttribute>(), t.GetCustomAttribute<DefaultNavigatableAttribute>() != null))
                .Where(t => !exclusions.Contains(t.Item1.Name))
                .OrderByDescending(t => t.Item2)
                .ToArray();
        }
    }
}