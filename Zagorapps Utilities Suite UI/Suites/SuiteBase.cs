namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Controls;
    using Core.Library.Events;
    using Core.Library.Extensions;

    public abstract class SuiteBase : DefaultEntityBase<IViewControl>, ISuite
    {
        private readonly string suiteName;

        protected SuiteBase(string suiteName, IEnumerable<IViewControl> views)
            : base(views)
        {
            this.suiteName = suiteName;

            this.DefaultView.IsActive = true;
            this.Entities.ForEach(e => e.OnChangeView += ChangeView);
        }

        public IViewControl ActiveView
        {
            get { return this.Entities.First(v => v.IsActive); }
        }

        public IViewControl DefaultView
        {
            get { return this.DefaultEntity; }
        }

        public bool IsActive
        {
            get; set;
        }

        public string SuiteName
        {
            get { return this.suiteName; }
        }

        public event EventHandler<EventArgs<IViewControl, object>> OnViewChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Navigate(string viewName, object args)
        {
            IViewControl view = this.FindView(viewName);

            if (view != null)
            {
                this.SetActiveView(view);

                Invoker.Raise(ref this.OnViewChanged, this, view, args);
            }
            else
            {
                // TODO: Raise a log event
            }
        }

        private void ChangeView(object sender, EventArgs<string, object> e)
        {
            this.Navigate(e.First, e.Second);
        }

        private IViewControl FindView(string viewName)
        {
            return this.Entities.FirstOrDefault(v => v.ViewControlName == viewName);
        }

        private void SetActiveView(IViewControl activeView)
        {
            this.Entities.ForEach(v => v.IsActive = false);

            activeView.IsActive = true;
        }
    }
}