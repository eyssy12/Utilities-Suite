namespace EyssyApps.Organiser.Library.Models.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public class FileOrganiserSettings : OrganiserSettingsBase
    {
        public FileOrganiserSettings()
        {
            this.ExtensionExemptions = Enumerable.Empty<string>();
            this.FileExemptions = Enumerable.Empty<string>();
        }

        public IEnumerable<string> ExtensionExemptions { get; set; }

        public IEnumerable<string> FileExemptions { get; set; }
    }
}