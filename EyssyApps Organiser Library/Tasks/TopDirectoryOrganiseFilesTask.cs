namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using Core.Library.Extensions;
    using Core.Library.Managers;
    using Models.Organiser;
    using Models.Settings;
    using Providers;

    [Serializable]
    public class TopDirectoryOrganiseFilesTask : ITask
    {
        public const string DefaultDirectoryName = "[Directories]",
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

            if (this.settings.OrgnisationType <= OrganisationType.None)
            {
                // throw exception
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
            // TODO: add logic for dealing 

            // pass in settings for exclusions/exmeptions
            // i.e. ignore files with extension(s), dont move files with the following file name ... ,etc.

            // Directory manager will do a lookup of all file names based on the rootPath
            // we will filter out the 

            if (this.settings.OrgnisationType.HasFlag(OrganisationType.All))
            {
                Task.WaitAll(
                    Task.Run(() => this.OrganiseDirectories()),
                    Task.Run(() => this.OrganiseFiles()));
            }
            else if (this.settings.OrgnisationType.HasFlag(OrganisationType.File))
            {
                this.OrganiseFiles();
            }
            else if (this.settings.OrgnisationType.HasFlag(OrganisationType.Directory))
            {
                this.OrganiseDirectories();
            }
        }

        protected void OrganiseFiles()
        {
            IEnumerable<string> files = this.DirectoryManager
                .GetFiles(this.settings.RootPath, searchOption: SearchOption.TopDirectoryOnly)
                .Except(this.settings.FileExemptions) // exclude out configured full path exemptions
                .Where(filePath => this.settings.ExtensionExemptions.Any(extension => filePath.EndsWith(extension, StringComparison.OrdinalIgnoreCase))) // exclude files based on configured file extensions
                .ToArray();

            files
                .GroupBy(f => Path.GetExtension(f))
                .ForEach(filePaths =>
                {
                    FileExtensionCategory category = this.Provider.GetCategoryForExtension(filePaths.Key); // key is the extension

                    string categoryPath = this.CreateCategoryPath(this.settings.RootPath, category.Category);

                    if (this.DirectoryManager.Exists(categoryPath, create: true))
                    {
                        filePaths.ForEach(filePath =>
                        {
                            this.FileManager.Move(filePath, categoryPath);
                        });
                    }
                });
        }

        protected virtual string CreateCategoryPath(string rootPath, string directoryName)
        {
            return string.Format(TopDirectoryOrganiseFilesTask.CategoryDirectoryFormat, rootPath, directoryName);
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

        public void Terminate()
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}