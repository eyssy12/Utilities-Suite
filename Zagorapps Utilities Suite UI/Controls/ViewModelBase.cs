namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.ComponentModel;
    using Extensions;

    public abstract class ViewModelBase : IViewModel
    {
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

        public TResult NotifyableAction<T, TResult>(T item, Func<T, TResult> action, string propertyName)
        {
            TResult result = action(item);

            this.OnPropertyChanged(propertyName);

            return result;
        }

        public void NotifyableAction<T>(T item, Action<T> action, string propertyName)
        {
            action(item);

            this.OnPropertyChanged(propertyName);
        }

        protected void SetField<T>(ref T field, T value, string propertyName)
        {
            field = value;

            this.OnPropertyChanged(propertyName);
        }

        protected void SetFieldIfChanged<T>(ref T field, T value, string propertyName)
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