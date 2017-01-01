namespace Zagorapps.Utilities.Suite.Library.Models.Settings
{
    using System.Collections.Generic;
    using ProtoBuf;

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class DirectoryOrganiserSettings : OrganiserSettingsBase
    {
        public DirectoryOrganiserSettings()
        {
            this.DirectoryExemptions = new List<string>();
        }

        public IEnumerable<string> DirectoryExemptions { get; set; }
    }
}