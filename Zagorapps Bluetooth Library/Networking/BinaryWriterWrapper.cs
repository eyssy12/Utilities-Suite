namespace Zagorapps.Bluetooth.Library.Networking
{
    using System;
    using System.IO;

    public class BinaryWriterWrapper : IBinaryWriter
    {
        protected readonly BinaryWriter Writer;

        public BinaryWriterWrapper(BinaryWriter writer)
        {
            this.Writer = writer;
        }

        public void Close()
        {
            this.Writer.Close();
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        public void Flush()
        {
            this.Writer.Flush();
        }

        public void Write(object contents)
        {
            this.Writer.Write(contents.ToString());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Writer != null)
                {
                    this.Writer.Dispose();
                }
            }
        }
    }
}