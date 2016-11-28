﻿namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Library.Events;
    using Core.Library.Extensions;

    public class SuiteNavigator : DefaultEntityBase<ISuite>, ISuiteNavigator
    {
        public SuiteNavigator(IEnumerable<ISuite> suites)
            : base(suites)
        {
            this.Entities.ForEach(e => e.OnViewChanged += E_OnViewChanged);
        }

        private void E_OnViewChanged(object sender, EventArgs<IViewControl, object> e)
        {
            Invoker.Raise(ref this.OnViewChanged, this, e);
        }

        public ISuite ActiveSuite
        {
            get { return this.Entities.First(e => e.IsActive); }
        }

        public IViewControl ActiveSuiteView
        {
            get { return this.ActiveSuite.ActiveView; }
        }

        public event EventHandler<EventArgs<ISuite, object>> OnEntityChanged;
        public event EventHandler<EventArgs<IViewControl, object>> OnViewChanged;

        public void Navigate(string entityName, object args)
        {
            ISuite suite = this.FindSuite(entityName);

            if (suite != null)
            {
                this.SetActiveSuite(suite);

                Invoker.Raise(ref this.OnEntityChanged, this, suite, args);
            }
            else
            {
                // TODO: Raise a log event
            }
        }

        private ISuite FindSuite(string entityName)
        {
            return this.Entities.FirstOrDefault(v => v.SuiteName == entityName);
        }

        private void SetActiveSuite(ISuite activeSuite)
        {
            this.Entities.ForEach(v => v.IsActive = false);

            activeSuite.IsActive = true;
        }
    }
}