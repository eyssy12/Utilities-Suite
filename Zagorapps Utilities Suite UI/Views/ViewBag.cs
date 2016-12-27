namespace Zagorapps.Utilities.Suite.UI.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Library.Attributes;
    using Navigation;

    public static class ViewBag
    {
        private static IReadOnlyDictionary<string, string> viewNames;

        static ViewBag()
        {
            viewNames = Assembly
                .GetCallingAssembly()
                .GetTypes()
                .Where(t =>
                    typeof(IViewControl).IsAssignableFrom(t) &&
                    !t.IsAbstract &&
                    !t.IsInterface)
                .Select(t => new
                {
                    Type = t.FullName,
                    Identifier = t.GetCustomAttribute<NavigatableAttribute>().Identifier
                })
                .ToDictionary(k => k.Type, v => v.Identifier);
        }

        public static string GetViewName<TView>() where TView : IViewControl
        {
            Type requested = typeof(TView);

            // I can rely on this always working because the static initialiser would throw an exception if any of the IViewControls don't have an attribute defined.
            return ViewBag.viewNames[requested.FullName];
        }
    }
}