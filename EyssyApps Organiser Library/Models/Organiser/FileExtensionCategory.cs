namespace EyssyApps.Organiser.Library.Models.Organiser
{
    using System.Collections.Generic;

    public class FileExtensionCategory
    {
        public string Category { get; set; }

        public IEnumerable<FileExtensionMetadata> Extensions { get; set; }
    }
}