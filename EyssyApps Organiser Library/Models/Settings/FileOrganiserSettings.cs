namespace EyssyApps.Organiser.Library.Models.Settings
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class FileOrganiserSettings
    {
        public string RootPath { get; set; }

        public string TargetDirectoryName { get; set; }

        public IEnumerable<string> ExtensionExemptions { get; set; }

        public IEnumerable<string> FileExemptions { get; set; }

        public IEnumerable<string> DirectoryExemptions { get; set; }
    }
}