namespace Zagorapps.Configuration.Library.Extensions
{
    using Core.Library.Extensions;
    using SimpleInjector;

    public static class BindingsExtensions
    {
        public static void RegisterBindings(this Container container, params BindingsBase[] bindings)
        {
            if (bindings.SafeAny())
            {
                bindings.ForEach(b =>
                {
                    b.RegisterBindingsToContainer(container);
                });
            }
        }
    }
}