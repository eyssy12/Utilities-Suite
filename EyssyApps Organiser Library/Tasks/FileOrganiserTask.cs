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

        private FileOrganiserSettings settings;

        public FileOrganiserTask(
            string name,
            string description,
            FileOrganiserSettings settings, 
            IFileExtensionProvider provider,
            IFileManager fileManager,
            IDirectoryManager directoryManager,
            Guid? identity = null)
            : base(identity, name, description,  OrganiseType.File, TaskType.Organiser)
        {
            this.settings = settings;

            this.Provider = provider;
            this.FileManager = fileManager;
            this.DirectoryManager = directoryManager;
        }

        protected override void HandleExecute()
        {
            this.OnStateChanged(TaskState.Started);

            this.FilterFiles(
                    SearchOption.TopDirectoryOnly,
                    filePath => !this.settings.FileExemptions.Any(fe => fe == filePath),
                    filePath => !this.settings.ExtensionExemptions.Any(extension => filePath.EndsWith(extension, StringComparison.OrdinalIgnoreCase)))
                .GroupBy(f => Path.GetExtension(f))
                .ForEach(filePaths =>
                {
                    FileExtensionCategory category = this.Provider.GetCategoryForExtension(new string(filePaths.Key.Skip(1).ToArray())); // key is the extension

                    string categoryPath;
                    if (category == null)
                    {
                        categoryPath = this.CreateCategoryPath(this.settings.RootPath, OrganiseTaskBase.DefaultUnkownName);
                    }
                    else
                    {
                        categoryPath = this.CreateCategoryPath(this.settings.RootPath, category.Value);
                    }

                    this.MoveFiles(filePaths, categoryPath);
                });

            this.OnStateChanged(TaskState.Finished);
        }

        protected override void HandleTerminate()
        {
            throw new NotImplementedException();
        }

        protected IEnumerable<string> FilterFiles(SearchOption searchOption, params Func<string, bool>[] filters)
        {
            IEnumerable<string> files = this.DirectoryManager.GetFiles(this.settings.RootPath, searchOption: searchOption).ToArray();

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
                    string destinationPath = Path.Combine(targetCategoryPath, Path.GetFileName(filePath));

                    if (this.FileManager.Exists(destinationPath))
                    {
                        throw new DuplicateFileException(Path.GetFileName(destinationPath), this.FileManager.ReadBytes(destinationPath), "Duplicate file found at '" + destinationPath + "'");
                    }

                    this.FileManager.Move(filePath, destinationPath);
                });
            }
        }

        protected virtual string CreateCategoryPath(string rootPath, string directoryName)
        {
            return string.Format(FileOrganiserTask.CategoryDirectoryFormat, rootPath, directoryName);
        }
    }
}