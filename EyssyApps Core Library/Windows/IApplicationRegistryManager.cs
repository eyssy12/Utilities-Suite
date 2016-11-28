namespace Zagorapps.Core.Library.Windows
{
    using Microsoft.Win32;

    public interface IApplicationRegistryManager
    {
        void SetValue(string name, object value);

        object GetValue(string name, object defaultValue);

        void RemoveValue(string name, object value);

        void SetRunOnStartup(bool value, string applicationPath);

        RegistryKey OpenKey(string name, bool writable);
    }
}