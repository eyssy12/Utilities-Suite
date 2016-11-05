namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core.Library.Extensions;
    using Core.Library.Managers;
    using Models.Settings;

    public class DirectoryOrganiserTask : OrganiseTaskBase
    {
        protected const string DefaultDirectoryName = "[Directories]";
            
        protected readonly IDirectoryManager DirectoryManager;

        public DirectoryOrganiserTask(
            Guid id,
            string description,
            FileOrganiserSettings settings,
            IDirectoryManager directoryManager)
            : base(id, description, settings, OrganiseType.Directory, TaskType.Organiser)
        {
            this.DirectoryManager = directoryManager;
        }

        protected override void HandleExecute()
        {
            // TODO: don't forget exemptions - don't try to add the folders to the folders we're moving them to
            // i.e. Test => Folders
            // i.e. Folders => Folders (shouldnt happen)

            IEnumerable<string> directories = this.DirectoryManager
                .GetDirectores(this.Settings.RootPath, searchOption: SearchOption.TopDirectoryOnly)
                .Except(this.Settings.DirectoryExemptions)
                .ToArray();

            string targetDirectoryName = string.IsNullOrWhiteSpace(this.Settings.TargetDirectoryName) ? DirectoryOrganiserTask.DefaultDirectoryName : this.Settings.TargetDirectoryName;
            string targetDirectoryPath = Path.Combine(this.Settings.RootPath, targetDirectoryName);

            if (this.DirectoryManager.Exists(targetDirectoryPath, create: true))
            {
                directories.ForEach(d => this.DirectoryManager.Move(d, targetDirectoryPath));
            }
        }

        protected override void HandleTerminate()
        {
            throw new NotImplementedException();
        }
    }
}