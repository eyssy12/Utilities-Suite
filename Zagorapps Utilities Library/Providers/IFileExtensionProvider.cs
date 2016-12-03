namespace Zagorapps.Utilities.Library.Providers
{
    using System.Collections.Generic;
    using Models.Organiser;

    public interface IFileExtensionProvider
    {
        IEnumerable<FileExtensionMetadata> GetAllExtensions();

        IEnumerable<FileExtensionCategory> GetAllCategories();

        FileExtensionCategory GetCategoryForExtension(string extension);
    }
}