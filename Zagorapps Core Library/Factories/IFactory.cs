namespace Zagorapps.Core.Library.Factories
{
    public interface IFactory
    {
        TInstance Create<TInstance>();
    }
}