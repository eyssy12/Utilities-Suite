namespace Zagorapps.Core.Library.Windows
{
    using System.Diagnostics;

    public class WinSystemService : IWinSystemService
    {
        public void OpenFolder(string folderName)
        {
            this.StartProcess(folderName);
        }

        public void Restart()
        {
            this.StartShutDown("-f -r -t 5");
        }

        public void LogOff()
        {
            this.StartShutDown("-l");
        }

        public void LockMachine()
        {
            this.StartProcess(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
        }

        public void Shutdown()
        {
            this.StartShutDown("-f -s -t 5");
        }

        public void CancelShutdown()
        {
            this.StartShutDown("-a");
        }

        private void StartShutDown(string param)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.Arguments = "/C shutdown " + param;

            this.StartProcess(info);
        }

        private void StartProcess(string fileName)
        {
            Process.Start(fileName);
        }

        private void StartProcess(string fileName, string arguments)
        {
            Process.Start(fileName, arguments);
        }

        private void StartProcess(ProcessStartInfo info)
        {
            Process.Start(info);
        }
    }
}