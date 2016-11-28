namespace Zagorapps.Core.Library.Execution
{
    public interface IExecute<TItem>
        where TItem : class
    {
        void Execute(TItem item);
    }
}