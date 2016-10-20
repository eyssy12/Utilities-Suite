namespace EyssyApps.Organiser.Library.Tasks
{
    public interface IOrganiseTask : ITask
    {
        OrganiseType Type { get; }
    }
}