namespace EyssyApps.Core.Library.Windows.Registry
{
    using System;
    using System.Reflection;
    using Microsoft.Win32;

    public class ApplicationRegistryManager : IApplicationRegistryManager
    {
        protected const string KeyFormat = "{0}{1}",
            RegistryLocation = @"HKEY_LOCAL_MACHINE\SOFTWARE\",
            StartupKey = @"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        protected readonly string ApplicationName;

        protected readonly RegistryKey ApplicationKey;

        public ApplicationRegistryManager(string applicationName)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                throw new ArgumentNullException(nameof(applicationName), "Application name not provided");
            }

            this.ApplicationName = applicationName;

            this.ApplicationKey = this.InitialiseApplicationKey();
        }

        public void SetValue(string name, object value)
        {
            this.ApplicationKey.SetValue(name, value);
        }

        public void RemoveValue(string name, object value)
        {
            this.ApplicationKey.DeleteValue(name);
        }

        public object GetValue(string name, object defaultValue)
        {
            return this.ApplicationKey.GetValue(name) ?? defaultValue;
        }

        public RegistryKey OpenKey(string name, bool writable)
        {
            return Registry.CurrentUser.OpenSubKey(name, writable);
        }

        public void SetRunOnStartup(bool value)
        {
            RegistryKey key = this.OpenKey(ApplicationRegistryManager.StartupKey, true);

            if (value)
            {
                key.SetValue(this.ApplicationName, Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                key.DeleteValue(this.ApplicationName, false);
            }
        }

        private RegistryKey InitialiseApplicationKey()
        {
            string keyFormatted = string.Format(ApplicationRegistryManager.KeyFormat, ApplicationRegistryManager.RegistryLocation, this.ApplicationName);

            RegistryKey key = Registry.CurrentUser.OpenSubKey(keyFormatted, true);

            if (key == null)
            {
                return Registry.CurrentUser.CreateSubKey(keyFormatted, true);
            }

            return key;
        }
    }
}