namespace Zagorapps.Utilities.Suite.UI.Services
{
    using System;
    using Zagorapps.Core.Library.Events;

    public interface ISuiteService
    {
        event EventHandler<EventArgs<string>> OnSuiteChangeRequested;

        void ChangeSuite(string identifier);
    }
}