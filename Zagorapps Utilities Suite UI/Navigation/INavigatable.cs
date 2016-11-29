namespace Zagorapps.Utilities.Suite.UI.Navigation
{
    public interface INavigatable
    {
        string Identifier { get; }

        bool IsActive { get; set; }
    }
}