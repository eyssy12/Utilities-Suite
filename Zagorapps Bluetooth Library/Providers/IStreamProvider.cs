namespace Zagorapps.Bluetooth.Library.Providers
{
    using System.IO;
    using Networking;

    public interface IStreamProvider
    {
        IBinaryWriter CreateBinaryWriter(BinaryWriter writer);

        IBinaryWriter CreateBinaryWriter(Stream stream);

        IBinaryReader CreateBinaryReader(BinaryReader reader);

        IBinaryReader CreateBinaryReader(Stream stream);
    }
}