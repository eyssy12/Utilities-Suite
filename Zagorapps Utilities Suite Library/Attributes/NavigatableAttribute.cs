namespace Zagorapps.Utilities.Suite.Library.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class NavigatableAttribute : Attribute
    {
        private string identifier;

        public NavigatableAttribute(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentNullException(nameof(identifier), "No identifier for the navigatable attribute provided");
            }

            this.identifier = identifier;
        }

        public string Identifier
        {
            get { return this.identifier; }
        }
    }
}