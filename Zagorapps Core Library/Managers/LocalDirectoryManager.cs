namespace Zagorapps.Core.Library.Managers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class LocalDirectoryManager : IDirectoryManager
    {
        public const string DefaultSearchPattern = "*";

        public void Create(string targetPath)
        {
            Directory.CreateDirectory(targetPath);
        }

        public void Move(string sourcePath, string targetPath)
        {
            Directory.Move(sourcePath, targetPath);
        }

        public bool Exists(string targetPath, bool create = false)
        {
            if (Directory.Exists(targetPath))
            {
                return true;
            }

            return create 
                ? Directory.CreateDirectory(targetPath).Exists
                : false;
        }

        public IEnumerable<string> GetDirectores(
            string rootPath,
            string searchPattern = LocalDirectoryManager.DefaultSearchPattern,
            SearchOption searchOption = SearchOption.AllDirectories)
        {
            return Directory.GetDirectories(rootPath, searchPattern, searchOption);
        }

        public IEnumerable<string> GetFiles(
            string rootPath, 
            string searchPattern = LocalDirectoryManager.DefaultSearchPattern, 
            SearchOption searchOption = SearchOption.AllDirectories)
        {
            return Directory.GetFiles(rootPath, searchPattern, searchOption);
        }
    }
}