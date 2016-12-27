namespace Zagorapps.Utilities.Suite.UI.Navigation
{
    public interface IDefaultNavigatable<TNavigatable>
        where TNavigatable : INavigatable
    {
        TNavigatable DefaultNavigatable { get; }
    }
}