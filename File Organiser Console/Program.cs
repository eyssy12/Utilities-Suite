namespace EyssyApps.File.Organiser.Console
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Configuration.Library;
    using Core.Library.Extensions;
    using Core.Library.Managers;
    using EyssyApps.Organiser.Library;
    using EyssyApps.Organiser.Library.Factories;
    using EyssyApps.Organiser.Library.Models.Organiser;
    using EyssyApps.Organiser.Library.Models.Settings;
    using EyssyApps.Organiser.Library.Providers;
    using EyssyApps.Organiser.Library.Tasks;
    using Newtonsoft.Json;
    using Ninject;
    using Ninject.Parameters;
    using SystemFile = System.IO.File;

    public class Program
    {
        public static void Main(string[] args)
        {
            
            Program p = new Program();
            p.Start();
        }

        private void Start()
        {
            string path2 = AppDomain.CurrentDomain.BaseDirectory.FindParentDirectory("File Organiser");

            string path = Path.Combine(path2, "file_extension_db.json");
            string data = SystemFile.ReadAllText(path);

            FileExtensionDatabaseModel result = JsonConvert.DeserializeObject<FileExtensionDatabaseModel>(data);

            StandardKernel kernel = new StandardKernel(new CommonBindings());

            IOrganiserFactory factory = kernel.Get<IOrganiserFactory>();

            ConstructorArgument argument = new ConstructorArgument("database", result);
            IFileExtensionProvider provider = kernel.Get<IFileExtensionProvider>(argument);
            IFileManager fileManager = factory.Create<IFileManager>();
            IDirectoryManager directoryManager = factory.Create<IDirectoryManager>();

            FileOrganiserSettings settings = new FileOrganiserSettings
            {
                OrgnisationType = OrganisationType.File,
                RootPath = @"C:\Users\Rob\Desktop\testFolder",
                DirectoryExemptions = new List<string>(),
                ExtensionExemptions = new List<string> { "txt" },
                FileExemptions = new List<string>()
            };

            TopDirectoryOrganiseFilesTask task = new TopDirectoryOrganiseFilesTask(Guid.NewGuid(), settings, provider, directoryManager, fileManager);
            task.Execute();
        }
    }
}