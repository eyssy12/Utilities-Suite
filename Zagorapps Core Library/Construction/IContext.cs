namespace Zagorapps.Core.Library.Construction
{
    public interface IContext
    {
        T GetValue<T>(string parameterName);
    }
}