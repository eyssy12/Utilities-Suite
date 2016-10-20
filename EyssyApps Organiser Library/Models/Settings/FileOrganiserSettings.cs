namespace EyssyApps.Organiser.Library.Models.Settings
{
    using System.Collections.Generic;

    public class FileOrganiserSettings
    {
        public string RootPath { get; set; }

        public string TargetDirectoryName { get; set; }

        public OrganiseType OrgnisationType { get; set; }

        public IEnumerable<string> ExtensionExemptions { get; set; }

        public IEnumerable<string> FileExemptions { get; set; }

        public IEnumerable<string> DirectoryExemptions { get; set; }
    }
}