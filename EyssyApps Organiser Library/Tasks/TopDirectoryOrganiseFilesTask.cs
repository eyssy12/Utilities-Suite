namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using Core.Library.Extensions;
    using Core.Library.Managers;
    using Models.Organiser;
    using Models.Settings;
    using Providers;

    [Serializable] // TODO: separate task into two tasks, one for FileOrganiserTask, other for DirectoryOrganiserTask
    public class TopDirectoryOrganiseFilesTask : ITask
    {
        public const string DefaultDirectoryName = "[Directories]",
            DefaultMiscName = "[Unknown]", // TODO: have functionality that detects if there are new extensions found and let the user decide what category they belong to
            CategoryDirectoryFormat = "{0}/[{1}]";

        protected readonly IFileExtensionProvider Provider;
        protected readonly IDirectoryManager DirectoryManager;
        protected readonly IFileManager FileManager;

        private readonly Guid id;

        private FileOrganiserSettings settings;

        public TopDirectoryOrganiseFilesTask(
            Guid id,
            FileOrganiserSettings settings,
            IFileExtensionProvider provider,
            IDirectoryManager directoryManager,
            IFileManager fileManager)
        {

            if (settings.OrgnisationType <= OrganisationType.None)
            {
                // TODO: throw exception
            }

            this.id = id;
            this.settings = settings;

            this.Provider = provider;
            this.DirectoryManager = directoryManager;
            this.FileManager = fileManager;
        }
        
        public Guid Id
        {
            get { return this.id; }
        }

        public void Execute()
        {
            switch (this.settings.OrgnisationType)
            {
                case OrganisationType.File:

                    this.OrganiseFiles();
                    break;

                case OrganisationType.Directory:

                    this.OrganiseDirectories();
                    break;

                case OrganisationType.All:

                    this.OrganiseDirectories(); // this before files because file extensions will generate their own category foldes, unless we add or own directory exclusions
                    this.OrganiseFiles();
                    break;
            }
        }

        public void Terminate()
        {
            // TODO: how to terminate code ?
            return;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // TODO: implement serialization/deserialization
            throw new NotImplementedException();
        }

        protected void OrganiseDirectories()
        {
            // TODO: don't forget exemptions - don't try to add the folders to the folders we're moving them to
            // i.e. Test => Folders
            // i.e. Folders => Folders (shouldnt happen)

            IEnumerable<string> directories = this.DirectoryManager
                .GetDirectores(this.settings.RootPath, searchOption: SearchOption.TopDirectoryOnly)
                .Except(this.settings.DirectoryExemptions)
                .ToArray();

            string targetDirectoryName = string.IsNullOrWhiteSpace(this.settings.TargetDirectoryName) ? TopDirectoryOrganiseFilesTask.DefaultDirectoryName : this.settings.TargetDirectoryName;
            string targetDirectoryPath = Path.Combine(this.settings.RootPath, targetDirectoryName);

            if (this.DirectoryManager.Exists(targetDirectoryPath, create: true))
            {
                directories.ForEach(d => this.DirectoryManager.Move(d, targetDirectoryPath));
            }
        }

        protected void OrganiseFiles()
        {
            this.FilterFiles(
                    SearchOption.TopDirectoryOnly,
                    filePath => !this.settings.FileExemptions.Any(fe => fe == filePath),
                    filePath => !this.settings.ExtensionExemptions.Any(extension => filePath.EndsWith(extension, StringComparison.OrdinalIgnoreCase)))
                .GroupBy(f => Path.GetExtension(f))
                .ForEach(filePaths =>
                {
                    FileExtensionCategory category = this.Provider.GetCategoryForExtension(new string(filePaths.Key.Skip(1).ToArray())); // key is the extension

                    string categoryPath = this.CreateCategoryPath(this.settings.RootPath, category.Category);

                    this.MoveFiles(filePaths, categoryPath);
                });
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

        protected virtual string CreateCategoryPath(string rootPath, string directoryName)
        {
            return string.Format(TopDirectoryOrganiseFilesTask.CategoryDirectoryFormat, rootPath, directoryName);
        }
    }
}