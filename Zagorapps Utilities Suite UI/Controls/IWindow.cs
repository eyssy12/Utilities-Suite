namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System.ComponentModel;
    using Navigation;

    public interface IWindow : INotifyPropertyChanged
    {
        IViewControl ActiveView { get; }

        object TryRetrieveResource(string name);

        void ShowWindow();

        void CloseWindow();
    }
}