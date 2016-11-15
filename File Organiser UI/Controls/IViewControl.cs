namespace File.Organiser.UI.Controls
{
    using System;
    using System.ComponentModel;
    using EyssyApps.Core.Library.Events;

    public interface IViewControl : INotifyPropertyChanged
    {
        string ViewControlName { get; }

        bool IsDefault { get; }

        bool IsActive { get; set; }

        event EventHandler<EventArgs<string, object>> OnChangeView;

        void ActivateView(object arg);
    }
}