namespace EyssyApps.UI.Library.Controls
{
    using System.ComponentModel;
    using Organiser.Library.Extensions;

    public abstract class ViewModelBase : IViewModel
    {
        protected ViewModelBase()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string propertyName]
        {
            get { return this.Validate(propertyName); }
        }

        public virtual string Validate(string propertyName)
        {
            return string.Empty;
        }

        protected void SetField<T>(ref T field, T value, string propertyName)
        {
            if (value.CompareExchange(ref field))
            {
                this.OnPropertyChanged(propertyName);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}