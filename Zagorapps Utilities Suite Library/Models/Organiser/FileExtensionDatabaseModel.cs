namespace Zagorapps.Utilities.Suite.Library.Models.Organiser
{
    using System.Collections.Generic;

    public class FileExtensionDatabaseModel
    {
        public string Description { get; set; }

        public IEnumerable<FileExtensionCategory> Categories { get; set; }
    }
}