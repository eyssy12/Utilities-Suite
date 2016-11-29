namespace Zagorapps.Utilities.Suite.UI.Navigation
{
    using System;
    using Core.Library.Events;

    public interface INavigatableNavigator
    {
        event EventHandler<EventArgs<INavigatable, object>> OnNavigatableChanged;

        void Navigate(string identifier, object args);
    }
}