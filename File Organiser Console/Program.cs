namespace EyssyApps.File.Organiser.Console
{
    using System;
    using System.IO;
    using EyssyApps.Organiser.Library.Models.Organiser;
    using Newtonsoft.Json;
    using SystemFile = System.IO.File;

    public class Program
    {
        public static void Main(string[] args)
        {
            // System.AppDomain.CurrentDomain.BaseDirectory

            string path2 = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);

            string to = Path.GetFullPath(Environment.CurrentDirectory);

            string path = @"C:\Users\Rob\Desktop\file_extension_db.json";
            string data = SystemFile.ReadAllText(path);

            FileExtensionDatabaseModel result = JsonConvert.DeserializeObject<FileExtensionDatabaseModel>(data);

            System.Console.WriteLine(result);
        }
    }
}