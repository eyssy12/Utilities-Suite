namespace Zagorapps.Utilities.Suite.Library.Providers
{
    public interface IConnectivityStore
    {
        void SaveFile(byte[] contents, string fileName, string client, bool append = false);

        void SaveFile(string contents, string fileName, string client, bool append = false);

        string GetClientStorePath(string client);
    }
}