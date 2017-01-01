namespace Zagorapps.Utilities.Suite.UI.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Library.Events;
    using Core.Library.Extensions;
    using Library.Attributes;

    public abstract class DefaultNavigatableBase<TNavigatable> : IDefaultNavigatable<TNavigatable>
        where TNavigatable : INavigatable
    {
        protected readonly IEnumerable<TNavigatable> Navigatables;

        protected DefaultNavigatableBase(IEnumerable<TNavigatable> navigatables)
        {
            if (navigatables.IsEmpty())
            {
                throw new ArgumentNullException(nameof(navigatables), "no entities provided");
            }

            IEnumerable<TNavigatable> defaultNavigatable = navigatables.Where(v => v.GetType().GetCustomAttribute<DefaultNavigatableAttribute>() != null).ToArray();

            if (defaultNavigatable.Count() == 0)
            {
                throw new ArgumentException("No default entity provided.");
            }

            if (defaultNavigatable.Count() > 1)
            {
                throw new ArgumentException("More than one default entity provided.");
            }

            this.Navigatables = navigatables;
            this.DefaultNavigatable = defaultNavigatable.Single();
        }

        protected event EventHandler<EventArgs<TNavigatable, object>> OnNavigatableChanged;

        public TNavigatable DefaultNavigatable { get; private set; }

        protected void Navigate(TNavigatable navigatable, object args)
        {
            this.SetActiveNavigatable(navigatable);

            Invoker.Raise(ref this.OnNavigatableChanged, this, navigatable, args);
        }

        protected TNavigatable FindNavigatable(string identifier)
        {
            return this.Navigatables.FirstOrDefault(v => v.Identifier == identifier);
        }

        protected void SetActiveNavigatable(TNavigatable activeNavigatable)
        {
            this.Navigatables.ForEach(v => v.IsActive = false);

            activeNavigatable.IsActive = true;
        }
    }
}