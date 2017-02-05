namespace Zagorapps.Utilities.Suite.Library.Providers
{
    using System;
    using System.IO;
    using Core.Library.Managers;

    public class ConnectivityStore : LocalStoreProviderBase, IConnectivityStore
    {
        public ConnectivityStore(string baseDirectory, IFileManager fileManager, IDirectoryManager directoryManager)
            : base(baseDirectory, fileManager, directoryManager)
        {
        }

        public string GetClientStorePath(string client)
        {
            return this.GenerateClientStore(client);
        }

        public void SaveFile(byte[] contents, string fileName, string client, bool append = false)
        {
            string clientStorePath = this.GenerateClientStore(client);

            if (this.DirectoryManager.Exists(clientStorePath, create: true))
            {
                this.FileManager.WriteAllBytes(this.GenerateFilePath(clientStorePath, fileName), contents, append);
            }
        }

        public void SaveFile(string contents, string fileName, string client, bool append = false)
        {
            string clientStorePath = this.GenerateClientStore(client);

            if (this.DirectoryManager.Exists(clientStorePath, create: true))
            {
                if (append)
                {
                    this.FileManager.Write(this.GenerateFilePath(clientStorePath, fileName), contents, append);
                }
                else
                {
                    this.FileManager.WriteAllText(this.GenerateFilePath(clientStorePath, fileName), contents);
                }
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