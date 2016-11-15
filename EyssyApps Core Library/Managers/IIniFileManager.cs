namespace EyssyApps.Core.Library.Managers
{
    using IniParser.Model;

    public interface IIniFileManager
    {
        IniData ReadFile(string path);

        void WriteFile(string path, IniData data);

        void SetCommentIndicator(string comment);

        void SetValue(IniData data, string section, string key, object value);

        bool ReadBoolean(IniData data, string section, string key, bool defaultValue);
    }
}