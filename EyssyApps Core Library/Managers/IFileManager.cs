namespace EyssyApps.Core.Library.Managers
{
    using System.Collections.Generic;

    public interface IFileManager
    {
        void Move(string filePath, string movePath);

        bool Exists(string filePath);

        IEnumerable<byte> ReadBytes(string filePath);
    }
}