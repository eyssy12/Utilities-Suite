namespace EyssyApps.Organiser.Library.Models.Settings
{
    using System.Collections.Generic;
    using System.Linq;

    public class DirectoryOrganiserSettings : OrganiserSettingsBase
    {
        public DirectoryOrganiserSettings()
        {
            this.DirectoryExemptions = Enumerable.Empty<string>();
        }

        public IEnumerable<string> DirectoryExemptions { get; set; }
    }
}