namespace Zagorapps.Utilities.Suite.Library.Providers
{
    using System;
    using Core.Library.Managers;

    public abstract class LocalStoreProviderBase
    {
        protected readonly IFileManager FileManager;
        protected readonly IDirectoryManager DirectoryManager;
        protected readonly string BaseDirectory;

        protected LocalStoreProviderBase(string baseDirectory, IFileManager fileManager, IDirectoryManager directoryManager)
        {
            if (fileManager == null)
            {
                throw new ArgumentNullException(nameof(fileManager), "No file manager provided"); // TODO: resources
            }

            if (directoryManager == null)
            {
                throw new ArgumentNullException(nameof(directoryManager), "No directory manager provided.");
            }

            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                throw new ArgumentNullException(nameof(baseDirectory), "No base directory provided for the local store");
            }

            this.FileManager = fileManager;
            this.DirectoryManager = directoryManager;
            this.BaseDirectory = baseDirectory;

            this.DirectoryManager.Create(baseDirectory);
        }
    }
}