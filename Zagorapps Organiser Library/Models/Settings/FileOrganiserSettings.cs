namespace Zagorapps.Organiser.Library.Models.Settings
{
    using System.Collections.Generic;
    using ProtoBuf;

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class FileOrganiserSettings : OrganiserSettingsBase
    {
        public FileOrganiserSettings()
        {
            this.ExtensionExemptions = new List<string>();
            this.FileExemptions = new List<string>();
        }

        public IEnumerable<string> ExtensionExemptions { get; set; }

        public IEnumerable<string> FileExemptions { get; set; }
    }
}