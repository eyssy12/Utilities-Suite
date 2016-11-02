namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core.Library.Extensions;
    using Core.Library.Managers;
    using Models.Organiser;
    using Models.Settings;
    using Providers;

    public class FileOrganiserTask : OrganiseTaskBase
    {
        protected const string CategoryDirectoryFormat = "{0}/[{1}]";

        protected readonly IFileExtensionProvider Provider;
        protected readonly IFileManager FileManager;
        protected readonly IDirectoryManager DirectoryManager;

        public FileOrganiserTask(
            Guid id,
            string description,
            FileOrganiserSettings settings, 
            IFileExtensionProvider provider,
            IFileManager fileManager,
            IDirectoryManager directoryManager)
            : base(id, description, settings, OrganiseType.File)
        {
            this.Provider = provider;
            this.FileManager = fileManager;
            this.DirectoryManager = directoryManager;
        }

        protected override void HandleExecute()
        {
            this.FilterFiles(
                    SearchOption.TopDirectoryOnly,
                    filePath => !this.Settings.FileExemptions.Any(fe => fe == filePath),
                    filePath => !this.Settings.ExtensionExemptions.Any(extension => filePath.EndsWith(extension, StringComparison.OrdinalIgnoreCase)))
                .GroupBy(f => Path.GetExtension(f))
                .ForEach(filePaths =>
                {
                    FileExtensionCategory category = this.Provider.GetCategoryForExtension(new string(filePaths.Key.Skip(1).ToArray())); // key is the extension

                    string categoryPath;
                    if (category == null)
                    {
                        categoryPath = this.CreateCategoryPath(this.Settings.RootPath, OrganiseTaskBase.DefaultUnkownName);
                    }
                    else
                    {
                        categoryPath = this.CreateCategoryPath(this.Settings.RootPath, category.Value);
                    }

                    this.MoveFiles(filePaths, categoryPath);
                });
        }

        protected override void HandleTerminate()
        {
            throw new NotImplementedException();
        }

        protected IEnumerable<string> FilterFiles(SearchOption searchOption, params Func<string, bool>[] filters)
        {
            IEnumerable<string> files = this.DirectoryManager.GetFiles(this.Settings.RootPath, searchOption: searchOption).ToArray();

            if (filters.SafeAny())
            {
                filters.ForEach(filter =>
                {
                    files = files.Where(filter);
                });
            }

            return files;
        }

        protected void MoveFiles(IEnumerable<string> filePaths, string targetCategoryPath)
        {
            if (this.DirectoryManager.Exists(targetCategoryPath, create: true))
            {
                filePaths.ForEach(filePath =>
                {
                    this.FileManager.Move(filePath, Path.Combine(targetCategoryPath, Path.GetFileName(filePath)));
                });
            }
        }

        protected virtual string CreateCategoryPath(string rootPath, string directoryName)
        {
            return string.Format(FileOrganiserTask.CategoryDirectoryFormat, rootPath, directoryName);
        }
    }
}