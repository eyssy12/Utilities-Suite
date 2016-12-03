namespace Zagorapps.Utilities.Library.Tasks
{
    public interface IOrganiserTask : ITask
    {
        OrganiseType OrganiseType { get; }
    }
}