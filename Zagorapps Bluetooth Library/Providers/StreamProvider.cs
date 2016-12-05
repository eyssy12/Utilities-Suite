namespace Zagorapps.Bluetooth.Library.Providers
{
    using System.IO;
    using Networking;

    public class StreamProvider : IStreamProvider
    {
        public IBinaryReader CreateBinaryReader(Stream stream)
        {
            return this.CreateBinaryReader(new BinaryReader(stream));
        }

        public IBinaryReader CreateBinaryReader(BinaryReader reader)
        {
            return new BinaryReaderWrapper(reader);
        }

        public IBinaryWriter CreateBinaryWriter(Stream stream)
        {
            return this.CreateBinaryWriter(new BinaryWriter(stream));
        }

        public IBinaryWriter CreateBinaryWriter(BinaryWriter writer)
        {
            return new BinaryWriterWrapper(writer);
        }
    }
}