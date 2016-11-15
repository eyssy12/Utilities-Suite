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

        private DirectoryOrganiserSettings settings;

        public DirectoryOrganiserTask(
            Guid id,
            string description,
            DirectoryOrganiserSettings settings,
            IDirectoryManager directoryManager)
            : base(id, description,  OrganiseType.Directory, TaskType.Organiser)
        {
            this.settings = settings;

            this.DirectoryManager = directoryManager;
        }

        protected override void HandleExecute()
        {
            this.OnStateChanged(TaskState.Started);

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

            this.OnStateChanged(TaskState.Finished);
        }

        protected override void HandleTerminate()
        {
            throw new NotImplementedException();
        }
    }
}