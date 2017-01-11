namespace Zagorapps.Utilities.Suite.Library.Providers
{
    public interface IConnectivityStore
    {
        void SaveFile(string contents, string fileName, string savedBy);
    }
}