namespace Zagorapps.Bluetooth.Library.Extensions
{
    using System.Linq;
    using Networking;

    public static class StreamExtensions
    {
        public const byte EmptyByteIndicator = 0;
        public const int DefaultReadAmount = 256;

        public static byte[] ReadAndTrimBytes(this IBinaryReader reader, int amountToRead = StreamExtensions.DefaultReadAmount)
        {
            return reader.ReadBytes(amountToRead).TrimBytes();
        }

        public static byte[] TrimBytes(this byte[] source)
        {
            return source.Where(s => s != StreamExtensions.EmptyByteIndicator).ToArray();
        }
    }
}