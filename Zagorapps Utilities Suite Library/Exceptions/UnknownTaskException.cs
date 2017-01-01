namespace Zagorapps.Utilities.Suite.Library.Exceptions
{
    using System;

    [Serializable]
    public class UnknownTaskException : Exception
    {
        public UnknownTaskException(string message) : base(message)
        {
        }
    }
}