namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System.ComponentModel;

    public interface IViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        string Validate(string propertyName);
    }
}