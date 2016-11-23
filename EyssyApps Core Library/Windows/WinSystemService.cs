namespace EyssyApps.Core.Library.Windows
{ 
    using System.Diagnostics;

    public class WinSystemService : IWinSystemService
    {
        public void OpenFolder(string folderName)
        {
            Process.Start(folderName);
        }
    }
}