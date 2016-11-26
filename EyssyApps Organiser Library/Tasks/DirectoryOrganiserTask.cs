namespace EyssyApps.Organiser.Library.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core.Library.Extensions;
    using Core.Library.Managers;
    using Models.Settings;
    using Providers;

    public class DirectoryOrganiserTask : OrganiserTaskBase
    {
        protected const string DefaultDirectoryName = "[Directories]";
            
        protected readonly IDirectoryManager DirectoryManager;
        protected readonly IOrganiserSettingsProvider SettingsProvider;

        public DirectoryOrganiserTask(
            string name,
            string description,
            IOrganiserSettingsProvider settingsProvider,
            IDirectoryManager directoryManager,
            Guid? identity = null)
            : base(identity, name, description, OrganiseType.Directory, TaskType.Organiser)
        {
            this.SettingsProvider = settingsProvider;
            this.DirectoryManager = directoryManager;
        }

        protected override void HandleExecute()
        {
            this.OnStateChanged(TaskState.Started);

            // TODO: don't forget exemptions - don't try to add the folders to the folders we're moving them to
            // i.e. Test => Folders
            // i.e. Folders => Folders (shouldnt happen)

            DirectoryOrganiserSettings settings = this.SettingsProvider.Get<DirectoryOrganiserSettings>(this.Identity);

            IEnumerable<string> directories = this.DirectoryManager
                .GetDirectores(settings.RootPath, searchOption: SearchOption.TopDirectoryOnly)
                .Except(settings.DirectoryExemptions)
                .ToArray();

            string targetDirectoryName = string.IsNullOrWhiteSpace(settings.TargetDirectoryName) ? DirectoryOrganiserTask.DefaultDirectoryName : settings.TargetDirectoryName;
            string targetDirectoryPath = Path.Combine(settings.RootPath, targetDirectoryName);

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