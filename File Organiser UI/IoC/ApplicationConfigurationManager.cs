namespace File.Organiser.UI.IoC
{
    using System;
    using EyssyApps.Core.Library.Managers;
    using IniParser.Model;

    public class ApplicationConfigurationManager : IniFileManager, IApplicationConfigurationManager
    {
        public const string SectionSettings = "Settings",
            KeyRunOnStartup = "runOnStartup",
            CommentIndicator = ";";

        protected readonly IniData Configuration;

        private readonly string configPath;

        public ApplicationConfigurationManager(string configPath, string commentIndicator = ApplicationConfigurationManager.CommentIndicator)
        {
            if (string.IsNullOrWhiteSpace(configPath))
            {
                throw new ArgumentNullException(nameof(configPath), "A path for the configuration file has not been provided.");
            }

            this.configPath = configPath;

            this.SetCommentIndicator(commentIndicator);

            try
            {
                this.Configuration = this.ReadFile(configPath);
            }
            catch (Exception ex)
            {
                throw new Exception("Problem reading file" + ex.Message);
            }
        }

        public string ConfigPath
        {
            get { return this.configPath; }
        }

        public bool ReadBoolean(string section, string key, bool defaultValue)
        {
            return this.ReadBoolean(this.Configuration, section, key, defaultValue);
        }

        public void Save()
        {
            this.WriteFile(this.ConfigPath, this.Configuration);
        }

        public void SetValue(string section, string key, object value)
        {
            this.SetValue(this.Configuration, section, key, value);
        }
    }
}