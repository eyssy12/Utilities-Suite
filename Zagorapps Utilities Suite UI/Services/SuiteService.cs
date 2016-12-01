namespace Zagorapps.Utilities.Suite.UI.Services
{
    using System;
    using Core.Library.Events;

    public class SuiteService : ISuiteService
    {
        public event EventHandler<EventArgs<string>> OnSuiteChangeRequested;

        public void ChangeSuite(string suite)
        {
            Invoker.Raise(ref this.OnSuiteChangeRequested, this, suite);
        }
    }
}