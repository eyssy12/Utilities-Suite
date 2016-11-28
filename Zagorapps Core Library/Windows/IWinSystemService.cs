namespace Zagorapps.Core.Library.Windows
{
    public interface IWinSystemService
    {
        void OpenFolder(string folderName);

        void Restart();

        void LogOff();

        void LockMachine();

        void Shutdown();

        void CancelShutdown();
    }
}