namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using System;
    using Core.Library.Events;

    public interface IEntityNavigator<TEntity>
        where TEntity : class
    {
        event EventHandler<EventArgs<TEntity, object>> OnEntityChanged;

        void Navigate(string entityName, object args);
    }
}