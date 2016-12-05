namespace Zagorapps.Bluetooth.Library.Data
{
    public interface ICommandOperation<in T> : IOperation
    {
        bool Invoke(T argument);
    }
}