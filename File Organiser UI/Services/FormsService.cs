namespace File.Organiser.UI.Services
{
    using WinForms = System.Windows.Forms;

    public class FormsService : IFormsService
    {
        public string SelectFolderPathDialog()
        {
            var dialog = new WinForms.FolderBrowserDialog();

            WinForms.DialogResult result = dialog.ShowDialog();

            if (string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                return string.Empty;
            }

            return dialog.SelectedPath;
        }
    }
}