namespace Zagorapps.Organiser.Library.Tasks
{
    public interface IOrganiserTask : ITask
    {
        OrganiseType OrganiseType { get; }
    }
}