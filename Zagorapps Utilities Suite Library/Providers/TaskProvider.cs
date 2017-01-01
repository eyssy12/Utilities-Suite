namespace Zagorapps.Utilities.Suite.Library.Providers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core.Library.Managers;
    using Factories;
    using ProtoBuf;
    using Tasks;

    public class TaskProvider : LocalStoreProviderBase, ITaskProvider
    {
        protected readonly IOrganiserFactory Factory;

        public TaskProvider(string taskPath, IOrganiserFactory factory, IFileManager fileManager, IDirectoryManager directoryManager)
            : base(taskPath, fileManager, directoryManager)
        {
            this.Factory = factory;
        }

        public ITask Get(Guid identity)
        {
            return this.RebuildTask(this.ReadMetadata(identity));
        }

        public IEnumerable<ITask> GetAll()
        {
            IEnumerable<string> files = this.DirectoryManager.GetFiles(this.BaseDirectory, searchOption: SearchOption.TopDirectoryOnly).ToArray();

            return files.Select(file => this.RebuildTask(this.ReadMetadata(file))).ToArray();
        }

        public void Save(ITask task)
        {
            TaskMetadata metadata = new TaskMetadata
            {
                Identity = task.Identity,
                Name = task.Name,
                Description = task.Description,
                OrganiserType = task is IOrganiserTask ? (task as IOrganiserTask).OrganiseType : OrganiseType.None,
                TaskType = task.TaskType
            };

            this.FileManager.Serialize<TaskMetadata>(
                this.GenerateFilePath(task), 
                metadata, 
                (stream, entity) =>
                {
                    Serializer.Serialize(stream, entity);
                });
        }

        public void Delete(ITask task)
        {
            this.FileManager.Delete(this.GenerateFilePath(task));
        }

        protected TaskMetadata ReadMetadata(Guid identity)
        {
            return this.ReadMetadata(this.GenerateFilePath(identity));
        }

        protected TaskMetadata ReadMetadata(string filePath)
        {
            return this.FileManager.Read(filePath, stream => Serializer.Deserialize<TaskMetadata>(stream));
        }

        protected virtual string GenerateFilePath(ITask task)
        {
            return this.GenerateFilePath(task.Identity);
        }

        protected virtual string GenerateFilePath(Guid identity)
        {
            return Path.Combine(this.BaseDirectory, identity.ToString());
        }

        protected ITask RebuildTask(TaskMetadata metadata)
        {
            IFileManager fileManager = this.Factory.Create<IFileManager>();
            IDirectoryManager directoryManager = this.Factory.Create<IDirectoryManager>();
            IFileExtensionProvider provider = this.Factory.Create<IFileExtensionProvider>();
            IOrganiserSettingsProvider settingsProvider = this.Factory.Create<IOrganiserSettingsProvider>();

            if (metadata.OrganiserType == OrganiseType.File)
            {
                return new FileOrganiserTask(
                    metadata.Name, 
                    metadata.Description,
                    settingsProvider, 
                    provider,
                    fileManager, 
                    directoryManager,
                    identity: metadata.Identity);
            }

            return new DirectoryOrganiserTask(
                metadata.Name,
                metadata.Description, 
                settingsProvider, 
                directoryManager, 
                identity: metadata.Identity);
        }

        [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
        protected internal class TaskMetadata
        {
            public Guid Identity { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public OrganiseType OrganiserType { get; set; }

            public TaskType TaskType { get; set; }
        }
    }
}