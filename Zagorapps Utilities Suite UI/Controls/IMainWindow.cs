namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using Library.Interoperability;

    public interface IMainWindow : IWindow
    {
        IInteropHandle InteropHandle { get; }
    }
}