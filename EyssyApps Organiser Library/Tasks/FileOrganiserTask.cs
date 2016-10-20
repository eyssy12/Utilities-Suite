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
    public class FileOrganiserTask : ITask
    {
        protected const string CategoryDirectoryFormat = "{0}/[{1}]";

        protected readonly IFileExtensionProvider Provider;
        protected readonly IDirectoryManager DirectoryManager;
        protected readonly IFileManager FileManager;

        private readonly Guid id;

        private FileOrganiserSettings settings;

        public FileOrganiserTask(
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
            this.OrganiseFiles();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            throw new NotImplementedException();
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