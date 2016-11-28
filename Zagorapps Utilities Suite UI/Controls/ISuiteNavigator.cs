namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using Zagorapps.Core.Library.Events;

    public interface ISuiteNavigator : IEntityNavigator<ISuite>
    {
        event EventHandler<EventArgs<IViewControl, object>> OnViewChanged;

        ISuite ActiveSuite { get; }

        IViewControl ActiveSuiteView { get; }
    }
}