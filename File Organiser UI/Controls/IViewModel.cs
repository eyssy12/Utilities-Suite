namespace File.Organiser.UI.Controls
{
    using System.ComponentModel;

    public interface IViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        string Validate(string propertyName);
    }
}