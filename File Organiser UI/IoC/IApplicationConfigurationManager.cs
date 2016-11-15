namespace File.Organiser.UI.IoC
{
    using EyssyApps.Core.Library.Managers;

    public interface IApplicationConfigurationManager : IIniFileManager
    {
        string ConfigPath { get; }

        void SetValue(string section, string key, object value);

        bool ReadBoolean(string section, string key, bool defaultValue);

        void Save();
    }
}