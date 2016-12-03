namespace Zagorapps.Utilities.Library.Providers
{
    using System;
    using System.IO;
    using Core.Library.Managers;
    using Models.Settings;
    using ProtoBuf;
    using Tasks;

    public class OrganiserSettingsProvider : LocalStoreProviderBase, IOrganiserSettingsProvider
    {
        public OrganiserSettingsProvider(string storePath, IFileManager fileManager, IDirectoryManager directoryManager)
            : base(storePath, fileManager, directoryManager)
        {
        }

        public TSettings Get<TSettings>(Guid identity) where TSettings : OrganiserSettingsBase
        {
            return this.FileManager.Read(this.GenerateFilePath(identity), stream =>
            {
                return Serializer.Deserialize<TSettings>(stream);
            });
        }

        public TSettings Get<TSettings>(ITask task) where TSettings : OrganiserSettingsBase
        {
            return this.Get<TSettings>(task.Identity);
        }

        public void Save<TSettings>(TSettings settings) where TSettings : OrganiserSettingsBase
        {
            this.FileManager.Serialize<TSettings>(this.GenerateFilePath(settings.Reference), settings, (stream, instance) =>
            {
                Serializer.Serialize(stream, instance);
            });
        }

        protected virtual string GenerateFilePath(Guid identity)
        {
            return Path.Combine(this.BaseDirectory, identity.ToString());
        }
    }
}