namespace Zagorapps.Utilities.Suite.UI.Managers
{
    using System;
    using Navigation;
    using Suites;
    using Zagorapps.Core.Library.Events;

    public interface ISuiteManager
    {
        event EventHandler<EventArgs<ISuite, object>> OnSuiteChanged;

        event EventHandler<EventArgs<IViewControl, object>> OnSuiteViewChanged;

        ISuite ActiveSuite { get; }

        IViewControl ActiveSuiteView { get; }

        void NavigateToDefault();

        void Navigate(string suiteName, object args);
    }
}