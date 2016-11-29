namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Core.Library.Events;
    using Core.Library.Extensions;
    using Navigation;

    public abstract class SuiteBase : DefaultNavigatableBase<IViewControl>, ISuite
    {
        private readonly string suiteName;

        protected SuiteBase(string suiteName, IEnumerable<IViewControl> views)
            : base(views)
        {
            this.suiteName = suiteName;

            this.DefaultView.IsActive = true;
            this.Navigatables.ForEach(e => e.OnChangeView += this.ChangeView);
            this.OnNavigatableChanged += SuiteBase_OnNavigatableChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<EventArgs<IViewControl, object>> OnViewChanged;

        public IViewControl ActiveView
        {
            get { return this.Navigatables.First(v => v.IsActive); }
        }

        public IViewControl DefaultView
        {
            get { return this.DefaultNavigatable; }
        }

        public bool IsActive
        {
            get; set;
        }

        public string Identifier
        {
            get { return this.suiteName; }
        }

        public void Navigate(string viewName, object args)
        {
            IViewControl view = this.FindNavigatable(viewName);

            if (view == null)
            {
                // TODO: Raise a log event
            }
            else
            {
                this.Navigate(view, args);
            }
        }

        private void SuiteBase_OnNavigatableChanged(object sender, EventArgs<IViewControl, object> e)
        {
            Invoker.Raise(ref this.OnViewChanged, sender, e);
        }

        private void ChangeView(object sender, EventArgs<string, object> e)
        {
            this.Navigate(e.First, e.Second);
        }
    }
}