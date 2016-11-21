namespace EyssyApps.Organiser.Library
{
    using System;
    using System.Collections.Generic;

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