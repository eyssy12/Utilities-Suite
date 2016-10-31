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

    [Serializable]
    public class DirectoryOrganiserTask : OrganiseTaskBase
    {
        protected const string DefaultDirectoryName = "[Directories]";
            
        protected readonly IDirectoryManager DirectoryManager;

        private readonly Guid id;

        private FileOrganiserSettings settings;

        public DirectoryOrganiserTask(
            Guid id,
            string description,
            FileOrganiserSettings settings,
            IDirectoryManager directoryManager)
            : base(id, OrganiseType.Directory, description)
        {
            this.settings = settings;
            this.DirectoryManager = directoryManager;
        }

        public override void Execute()
        {
            this.OrganiseDirectories();
        }

        public override void Terminate()
        {
            // TODO: how to terminate code ?
            return;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
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