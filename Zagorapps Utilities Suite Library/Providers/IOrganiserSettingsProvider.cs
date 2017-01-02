namespace Zagorapps.Utilities.Suite.Library.Providers
{
    using System;
    using Models.Settings;
    using Tasks;

    public interface IOrganiserSettingsProvider
    {
        TSettings Get<TSettings>(ITask task) where TSettings : OrganiserSettingsBase;

        TSettings Get<TSettings>(Guid identity) where TSettings : OrganiserSettingsBase;

        bool Save<TSettings>(TSettings settings) where TSettings : OrganiserSettingsBase;

        bool Delete(Guid identity);

        bool Delete(ITask task);
    }
}