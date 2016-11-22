namespace EyssyApps.Organiser.Library.Exceptions
{
    using System;
    using Tasks;

    public class UnknownTaskException : Exception
    {
        public UnknownTaskException(string message) : base(message)
        {
        }
    }
}