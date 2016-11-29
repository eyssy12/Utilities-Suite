namespace Zagorapps.Core.Library.Managers
{
    using System.Collections.Generic;
    using System.IO;

    public interface IDirectoryManager
    {
        void Create(string targetPath);

        void Move(string sourcePath, string targetPath);

        bool Exists(string targetPath, bool create = false);

        IEnumerable<string> GetDirectores(
            string rootPath,
            string searchPattern = LocalDirectoryManager.DefaultSearchPattern,
            SearchOption searchOption = SearchOption.AllDirectories);

        IEnumerable<string> GetFiles(
            string rootPath, 
            string searchPattern = LocalDirectoryManager.DefaultSearchPattern, 
            SearchOption searchOption = SearchOption.AllDirectories);
    }
}