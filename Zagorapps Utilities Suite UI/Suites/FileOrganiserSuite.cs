namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System.Collections.Generic;
    using Library.Attributes;
    using Navigation;

    [Suite(FileOrganiserSuite.Name, "File Organiser")]
    public class FileOrganiserSuite : SuiteBase
    {
        public const string Name = nameof(FileOrganiserSuite);

        public FileOrganiserSuite(IEnumerable<IViewControl> views)
            : base(FileOrganiserSuite.Name, views)
        {
        }
    }
}