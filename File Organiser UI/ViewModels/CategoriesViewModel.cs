namespace File.Organiser.UI.ViewModels
{
    using System.Collections.Generic;

    public class CategoriesViewModel
    {
        public string Category { get; set; }

        public IEnumerable<FileExtensionViewModel> Extensions { get; set; }
    }
}