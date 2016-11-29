namespace Zagorapps.Utilities.Suite.UI.Navigation
{
    using System;
    using Zagorapps.Core.Library.Events;

    public interface IDefaultNavigatable<TNavigatable>
        where TNavigatable : INavigatable
    {
        TNavigatable DefaultNavigatable { get; }
    }
}