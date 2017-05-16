namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Controls;
    using Commands;
    using Navigation;
    using Zagorapps.Core.Library.Events;
    using Zagorapps.Utilities.Suite.Library.Factories;

    public abstract class ViewControlBase : UserControl, IViewControl
    {
        protected readonly IOrganiserFactory Factory;
        protected readonly ICommandProvider CommandProvider;

        private readonly string viewName;

        protected ViewControlBase(string viewName, IOrganiserFactory factory, ICommandProvider commandProvider)
        {
            // TODO: add static Guard class.
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory), "No factory has been provided.");
            }

            if (commandProvider == null)
            {
                throw new ArgumentNullException(nameof(commandProvider), "A command provider is missing.");
            }

            if (string.IsNullOrWhiteSpace(viewName))
            {
                throw new ArgumentNullException(nameof(viewName), "No view name has been provided - the main window would not be able to route between the views.");
            }

            this.Factory = factory;
            this.CommandProvider = commandProvider;

            this.viewName = viewName;
        }

        public event EventHandler<EventArgs<string, object>> OnChangeView;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Identifier
        {
            get { return this.viewName; }
        }

        public bool IsActive { get; set; }

        public abstract void InitialiseView(object arg); // gets called when user explicitly navigates to the view

        public abstract void FinaliseView(); // gets called when user switches to a different view (close procedure effectively)

        protected void OnViewChange(string viewName, object args = null)
        {
            this.FinaliseView();

            Invoker.Raise(ref this.OnChangeView, this, viewName, args);
        }

        protected void NotifyableAction<T>(T item, Action<T> action, string propertyName)
        {
            action(item);

            this.OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}