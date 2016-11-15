namespace EyssyApps.Core.Library.Managers
{
    public interface IFileManager
    {
        void Move(string filePath, string movePath);

        bool Exists(string filePath);
    }
}