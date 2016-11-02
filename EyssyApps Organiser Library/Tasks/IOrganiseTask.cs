namespace EyssyApps.Organiser.Library.Tasks
{
    using EyssyApps.Organiser.Library.Models.Settings;

    public interface IOrganiseTask : ITask
    {
        OrganiseType Type { get; }

        FileOrganiserSettings Settings { get; }
    }
}