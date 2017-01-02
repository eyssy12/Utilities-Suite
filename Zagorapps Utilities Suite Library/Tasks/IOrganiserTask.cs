namespace Zagorapps.Utilities.Suite.Library.Tasks
{
    public interface IOrganiserTask : ITask
    {
        OrganiseType OrganiseType { get; }
    }
}