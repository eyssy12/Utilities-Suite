namespace EyssyApps.UI.Library.Controls
{
    using System;
    using System.Collections.Generic;
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

        protected void SetEnumeration<TEnum>(object parameter, ref TEnum field)
            where TEnum : struct
        {
            TEnum value;
            if (Enum.TryParse<TEnum>(parameter.ToString(), out value))
            {
                field = value;
            }
        }

        protected T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static bool CompareExchange<T>(T proposedValue, ref T originalValue)
        {
            if (EqualityComparer<T>.Default.Equals(proposedValue, originalValue))
            {
                return false;
            }

            originalValue = proposedValue;

            return true;
        }
    }
}