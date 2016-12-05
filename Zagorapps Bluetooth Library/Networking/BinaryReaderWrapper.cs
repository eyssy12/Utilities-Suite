namespace Zagorapps.Bluetooth.Library.Networking
{
    using System;
    using System.IO;

    public class BinaryReaderWrapper : IBinaryReader
    {
        protected readonly BinaryReader Reader;

        public BinaryReaderWrapper(BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader), "message");
            }

            this.Reader = reader;
        }

        public void Close()
        {
            this.Reader.Close();
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        public byte[] ReadBytes(int count)
        {
            return this.Reader.ReadBytes(count);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Reader != null)
                {
                    this.Reader.Dispose();
                }
            }
        }
    }
}