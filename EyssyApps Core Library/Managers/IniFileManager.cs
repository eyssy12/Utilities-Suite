namespace EyssyApps.Core.Library.Managers
{
    using IniParser;
    using IniParser.Model;

    public class IniFileManager : IIniFileManager
    {
        protected readonly FileIniDataParser Ini;
        
        public IniFileManager()
        {
            this.Ini = new FileIniDataParser();
        }

        public IniData ReadFile(string path)
        {
            return this.Ini.ReadFile(path);
        }

        public void SetCommentIndicator(string comment)
        {
            if (!string.IsNullOrWhiteSpace(comment))
            {
                this.Ini.Parser.Configuration.CommentString = comment;
            }
        }

        public void WriteFile(string path, IniData data)
        {
            this.Ini.WriteFile(path, data);
        }

        public bool ReadBoolean(IniData data, string section, string key, bool defaultValue)
        {
            bool value;
            if (bool.TryParse(data[section][key], out value))
            {
                return value;
            }

            return defaultValue;
        }

        public void SetValue(IniData data, string section, string key, object value)
        {
            data[section][key] = value.ToString();
        }
    }
}