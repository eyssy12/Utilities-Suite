namespace EyssyApps.Organiser.Library.Tasks
{
    using EyssyApps.Organiser.Library.Models.Settings;

    public interface IOrganiseTask : ITask
    {
        OrganiseType OrganiseType { get; }
    }
}