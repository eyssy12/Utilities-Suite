namespace Zagorapps.Utilities.Suite.UI.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Library.Events;
    using Core.Library.Extensions;

    public class SuiteManager : DefaultNavigatableBase<ISuite>, ISuiteManager
    {
        public SuiteManager(IEnumerable<ISuite> suites)
            : base(suites)
        {
            this.Navigatables.ForEach(e => e.OnViewChanged += this.E_OnViewChanged);
            this.OnNavigatableChanged += SuiteManager_OnNavigatableChanged;
        }

        public event EventHandler<EventArgs<ISuite, object>> OnSuiteChanged;

        public event EventHandler<EventArgs<IViewControl, object>> OnSuiteViewChanged;

        public ISuite ActiveSuite
        {
            get { return this.Navigatables.First(e => e.IsActive); }
        }

        public IViewControl ActiveSuiteView
        {
            get { return this.ActiveSuite.ActiveView; }
        }

        public void Navigate(string suiteName, object args)
        {
            ISuite suite = this.FindNavigatable(suiteName);

            if (suite == null)
            {
                // TODO: Raise a log event
            }
            else
            {
                this.Navigate(suite, args);
            }
        }

        protected void SuiteManager_OnNavigatableChanged(object sender, EventArgs<ISuite, object> e)
        {
            Invoker.Raise(ref this.OnSuiteChanged, this, e);
        }

        protected void E_OnViewChanged(object sender, EventArgs<IViewControl, object> e)
        {
            Invoker.Raise(ref this.OnSuiteViewChanged, this, e);
        }
    }
}