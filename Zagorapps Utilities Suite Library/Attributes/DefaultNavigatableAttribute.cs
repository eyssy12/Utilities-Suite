namespace Zagorapps.Utilities.Suite.Library.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class DefaultNavigatableAttribute : NavigatableAttribute
    {
        public DefaultNavigatableAttribute(string identifier)
            : base(identifier)
        {
        }
    }
}