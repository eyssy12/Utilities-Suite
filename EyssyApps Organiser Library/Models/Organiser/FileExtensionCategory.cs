namespace EyssyApps.Organiser.Library.Models.Organiser
{
    using System.Collections.Generic;

    public class FileExtensionCategory
    {
        public string Value { get; set; }

        public IEnumerable<FileExtensionMetadata> Extensions { get; set; }
    }
}