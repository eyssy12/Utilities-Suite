namespace Zagorapps.Utilities.Suite.UI.Navigation
{
    using System;
    using System.ComponentModel;
    using Zagorapps.Core.Library.Events;

    public interface IViewControl : INavigatable, INotifyPropertyChanged
    {
        event EventHandler<EventArgs<string, object>> OnChangeView;

        void InitialiseView(object arg);

        void FinaliseView();
    }
}