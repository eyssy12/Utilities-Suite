namespace Zagorapps.Utilities.Suite.UI.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class SuiteAttribute : Attribute
    {
        protected readonly string Name;

        public SuiteAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "No name provided");
            }

            this.Name = name;
        }
    }
}