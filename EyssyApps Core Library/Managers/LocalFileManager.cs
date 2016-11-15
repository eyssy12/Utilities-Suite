namespace EyssyApps.Core.Library.Managers
{
    using System.IO;

    public class LocalFileManager : IFileManager
    {
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public void Move(string filePath, string movePath)
        {
            File.Move(filePath, movePath);
        }
    }
}