namespace EyssyApps.Organiser.Library.Providers
{
    using System.Collections.Generic;
    using Models.Organiser;

    public interface IFileExtensionProvider
    {
        IEnumerable<FileExtensionMetadata> Get();

        FileExtensionCategory GetCategoryForExtension(string extension);
    }
}