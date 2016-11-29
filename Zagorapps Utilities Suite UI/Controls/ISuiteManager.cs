namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using Zagorapps.Core.Library.Events;

    public interface ISuiteManager : IEntityNavigator<ISuite>
    {
        event EventHandler<EventArgs<IViewControl, object>> OnSuiteViewChanged;

        ISuite ActiveSuite { get; }

        IViewControl ActiveSuiteView { get; }
    }
}