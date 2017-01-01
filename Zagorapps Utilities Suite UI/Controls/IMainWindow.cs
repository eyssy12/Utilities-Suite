namespace Zagorapps.Utilities.Suite.UI.Controls
{
    using Library.Interop;

    public interface IMainWindow : IWindow
    {
        IInteropHandle InteropHandle { get; }
    }
}