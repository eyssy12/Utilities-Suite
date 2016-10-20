namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using Core.Library.Extensions;
    using Core.Library.Managers;
    using Models.Settings;

    [Serializable] // TODO: separate task into two tasks, one for FileOrganiserTask, other for DirectoryOrganiserTask
    public class DirectoryOrganiserTask : ITask
    {
        public const string DefaultDirectoryName = "[Directories]",
            DefaultMiscName = "[Unknown]"; // TODO: have functionality that detects if there are new extensions found and let the user decide what category they belong to
            
        protected readonly IDirectoryManager DirectoryManager;

        private readonly Guid id;

        private FileOrganiserSettings settings;

        public DirectoryOrganiserTask(
            Guid id,
            FileOrganiserSettings settings,
            IDirectoryManager directoryManager)
        {

            if (settings.OrgnisationType <= OrganisationType.None)
            {
                // TODO: throw exception
            }

            this.id = id;
            this.settings = settings;
            this.DirectoryManager = directoryManager;
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
                    
                    break;

                case OrganisationType.Directory:

                    this.OrganiseDirectories();
                    break;

                case OrganisationType.All:

                    this.OrganiseDirectories(); // this before files because file extensions will generate their own category foldes, unless we add or own directory exclusions
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

            string targetDirectoryName = string.IsNullOrWhiteSpace(this.settings.TargetDirectoryName) ? DirectoryOrganiserTask.DefaultDirectoryName : this.settings.TargetDirectoryName;
            string targetDirectoryPath = Path.Combine(this.settings.RootPath, targetDirectoryName);

            if (this.DirectoryManager.Exists(targetDirectoryPath, create: true))
            {
                directories.ForEach(d => this.DirectoryManager.Move(d, targetDirectoryPath));
            }
        }
    }
}