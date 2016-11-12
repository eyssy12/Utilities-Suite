namespace File.Organiser.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EyssyApps.Core.Library.Events;
    using EyssyApps.Core.Library.Extensions;

    public class ViewNavigator : IViewNavigator
    {
        protected readonly IEnumerable<IViewControl> Views;

        public ViewNavigator(IEnumerable<IViewControl> views)
        {
            if (views.IsEmpty())
            {
                throw new ArgumentNullException(nameof(views), "No views provided."); // TODO: resources
            }

            int defaultViewCount = views.Count(v => v.IsDefault);

            if (defaultViewCount == 0)
            {
                throw new ArgumentException("No default view provided.");
            }

            if (defaultViewCount > 1)
            {
                throw new ArgumentException("More than one default view provided.");
            }

            this.Views = views;
            this.Views.ForEach(v => v.OnChangeView += ChangeView);
        }

        public IViewControl ActiveView
        {
            get { return this.Views.First(v => v.IsActive); }
        }

        public event EventHandler<EventArgs<IViewControl>> OnViewChanged;

        public void Navigate(string viewName)
        {
            IViewControl view = this.FindView(viewName);

            if (view != null)
            {
                this.SetActiveView(view);

                Invoker.Raise(ref this.OnViewChanged, this, view);
            }
            else
            {
                // TODO: Raise a log event
            }
        }

        private void ChangeView(object sender, EventArgs<string> e)
        {
            this.Navigate(e.First);
        }

        private IViewControl FindView(string viewName)
        {
            return this.Views.FirstOrDefault(v => v.ViewControlName == viewName);
        }

        private void SetActiveView(IViewControl activeView)
        {
            this.Views.ForEach(v => v.IsActive = false);
            activeView.IsActive = true;
        }
    }
}