namespace Zagorapps.Organiser.Library.Models.Settings
{
    using System;
    using ProtoBuf;

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    [ProtoInclude(10, typeof(FileOrganiserSettings))]
    [ProtoInclude(11, typeof(DirectoryOrganiserSettings))]
    public abstract class OrganiserSettingsBase
    {
        public Guid Reference { get; set; }
        
        public string RootPath { get; set; }

        public string TargetDirectoryName { get; set; }
    }
}