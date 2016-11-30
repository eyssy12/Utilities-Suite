namespace Zagorapps.Utilities.Suite.UI.Suites
{
    using System.Collections.Generic;
    using Attributes;
    using Navigation;
    using Organiser.Library;

    [DefaultNavigatable]
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