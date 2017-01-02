namespace Zagorapps.Utilities.Suite.Library.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core.Library.Extensions;
    using Core.Library.Managers;
    using Exceptions;
    using Models.Organiser;
    using Models.Settings;
    using Providers;

    public class FileOrganiserTask : OrganiserTaskBase
    {
        protected const string CategoryDirectoryFormat = "{0}/[{1}]";

        protected readonly IFileExtensionProvider ExtensionProvider;
        protected readonly IFileManager FileManager;
        protected readonly IDirectoryManager DirectoryManager;
        protected readonly IOrganiserSettingsProvider SettingsProvider;

        public FileOrganiserTask(
            string name,
            string description,
            IOrganiserSettingsProvider settingsProvider, 
            IFileExtensionProvider extensionProvider,
            IFileManager fileManager,
            IDirectoryManager directoryManager,
            Guid? identity = null)
            : base(identity, name, description, OrganiseType.File, TaskType.Organiser)
        {
            // TODO: guard conditions

            this.ExtensionProvider = extensionProvider;
            this.FileManager = fileManager;
            this.DirectoryManager = directoryManager;
            this.SettingsProvider = settingsProvider;
        }

        protected override void HandleExecute()
        {
            this.OnStateChanged(TaskState.Started);

            FileOrganiserSettings settings = this.SettingsProvider.Get<FileOrganiserSettings>(this.Identity);

            this.FilterFiles(
                    settings.RootPath,
                    SearchOption.TopDirectoryOnly,
                    filePath => !settings.FileExemptions.Any(fe => fe == filePath),
                    filePath => !settings.ExtensionExemptions.Any(extension => filePath.EndsWith(extension, StringComparison.OrdinalIgnoreCase)))
                .GroupBy(f => Path.GetExtension(f))
                .ForEach(filePaths =>
                {
                    FileExtensionCategory category = this.ExtensionProvider.GetCategoryForExtension(new string(filePaths.Key.Skip(1).ToArray())); // key is the extension, with the '.'

                    string categoryPath;
                    if (category == null)
                    {
                        categoryPath = this.CreateCategoryPath(settings.RootPath, OrganiserTaskBase.DefaultUnkownName);
                    }
                    else
                    {
                        categoryPath = this.CreateCategoryPath(settings.RootPath, category.Value);
                    }

                    this.MoveFiles(filePaths, categoryPath);
                });

            this.OnStateChanged(TaskState.Finished);
        }

        protected override void HandleTerminate()
        {
            throw new NotImplementedException();
        }

        protected IEnumerable<string> FilterFiles(string root, SearchOption searchOption, params Func<string, bool>[] filters)
        {
            IEnumerable<string> files = this.DirectoryManager.GetFiles(root, searchOption: searchOption).ToArray();

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