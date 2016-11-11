namespace EyssyApps.UI.Library.Controls
{
    using System.ComponentModel;

    public interface IMainWindow : INotifyPropertyChanged
    {
        IViewControl ActiveView { get; }
    }
}