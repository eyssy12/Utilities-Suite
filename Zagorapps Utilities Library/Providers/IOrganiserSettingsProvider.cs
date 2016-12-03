namespace Zagorapps.Utilities.Library.Providers
{
    using System;
    using Models.Settings;
    using Tasks;

    public interface IOrganiserSettingsProvider
    {
        TSettings Get<TSettings>(ITask task) where TSettings : OrganiserSettingsBase;

        TSettings Get<TSettings>(Guid identity) where TSettings : OrganiserSettingsBase;

        void Save<TSettings>(TSettings settings) where TSettings : OrganiserSettingsBase;
    }
}