namespace Zagorapps.Utilities.Suite.Library.Exceptions
{
    using System;

    public class ViewNotFoundException : Exception
    {
        protected readonly string ViewName;

        public ViewNotFoundException(string viewName, string message)
            : base(message)
        {
            this.ViewName = viewName;
        }
    }
}