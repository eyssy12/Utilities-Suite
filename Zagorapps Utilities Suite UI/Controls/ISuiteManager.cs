namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using Zagorapps.Core.Library.Events;

    public interface ISuiteManager
    {
        event EventHandler<EventArgs<ISuite, object>> OnSuiteChanged;

        event EventHandler<EventArgs<IViewControl, object>> OnSuiteViewChanged;

        ISuite ActiveSuite { get; }

        IViewControl ActiveSuiteView { get; }

        void Navigate(string entityName, object args);
    }
}