namespace EyssyApps.Core.Library.Windows
{
    using Microsoft.Win32;

    public interface IApplicationRegistryManager
    {
        void SetValue(string name, object value);

        object GetValue(string name);

        void RemoveValue(string name, object value);

        void SetRunOnStartup(bool value);

        RegistryKey OpenKey(string name, bool writable);
    }
}