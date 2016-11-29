namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Controls;
    using Commands;
    using Navigation;
    using Zagorapps.Core.Library.Events;
    using Zagorapps.Organiser.Library.Factories;

    public abstract class ViewControlBase : UserControl, IViewControl
    {
        protected readonly IOrganiserFactory Factory;
        protected readonly ICommandProvider CommandProvider;

        private readonly string viewName;

        protected ViewControlBase(string viewName, IOrganiserFactory factory, ICommandProvider commandProvider)
        {
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

        public abstract void InitialiseView(object arg);
        
        protected void OnViewChange(string viewName, object args = null)
        {
            Invoker.Raise(ref this.OnChangeView, this, viewName, args);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}