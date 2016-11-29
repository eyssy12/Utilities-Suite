namespace Zagorapps.Utilities.Suite.UI.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class SuiteAttribute : Attribute
    {
        private readonly string identifier, friendlyName;

        public SuiteAttribute(string identifier, string friendlyName)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new ArgumentNullException(nameof(identifier), "No name provided");
            }

            if (string.IsNullOrWhiteSpace(friendlyName))
            {
                throw new ArgumentNullException(nameof(friendlyName), "No friendly name provided");
            }

            this.identifier = identifier;
            this.friendlyName = friendlyName;
        }

        public string Name
        {
            get { return this.identifier; }
        }

        public string FriendlyName
        {
            get { return this.friendlyName; }
        }
    }
}