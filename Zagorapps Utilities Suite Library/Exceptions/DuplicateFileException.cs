namespace Zagorapps.Utilities.Suite.Library.Exceptions
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class DuplicateFileException : Exception
    {
        private IEnumerable<byte> data;
        private string fileName;

        public DuplicateFileException(string fileName, IEnumerable<byte> data, string message) 
            : base(message)
        {
            this.fileName = fileName;
            this.data = data;
        }
    }
}