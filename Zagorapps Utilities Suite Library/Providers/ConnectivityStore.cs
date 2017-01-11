namespace Zagorapps.Utilities.Suite.Library.Providers
{
    using System.IO;
    using Core.Library.Managers;

    public class ConnectivityStore : LocalStoreProviderBase, IConnectivityStore
    {
        public ConnectivityStore(string baseDirectory, IFileManager fileManager, IDirectoryManager directoryManager)
            : base(baseDirectory, fileManager, directoryManager)
        {
        }

        public void SaveFile(string contents, string fileName, string savedBy)
        {
            string clientStorePath = this.GenerateClientStore(savedBy);

            if (this.DirectoryManager.Exists(clientStorePath, create: true))
            {
                this.FileManager.WriteAllText(this.GenerateFilePath(clientStorePath, fileName), contents);
            }
        }

        protected virtual string GenerateClientStore(string clientName)
        {
            return Path.Combine(this.BaseDirectory, clientName);
        }

        protected virtual string GenerateFilePath(string baseDirectory, string fileName)
        {
            return Path.Combine(baseDirectory, fileName);
        }
    }
}