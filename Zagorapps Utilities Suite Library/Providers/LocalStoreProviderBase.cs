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
                throw new ArgumentNullException(nameof(fileManager), ""); // TODO: resources
            }

            if (directoryManager == null)
            {
                throw new ArgumentNullException(nameof(directoryManager), "");
            }

            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                throw new ArgumentNullException(nameof(baseDirectory), "");
            }

            this.FileManager = fileManager;
            this.DirectoryManager = directoryManager;
            this.BaseDirectory = baseDirectory;

            this.DirectoryManager.Create(baseDirectory);
        }
    }
}