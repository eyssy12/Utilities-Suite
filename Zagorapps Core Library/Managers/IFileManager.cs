namespace Zagorapps.Core.Library.Managers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public interface IFileManager
    {
        void Move(string filePath, string movePath);

        bool Exists(string filePath);

        void Serialize<T>(string filePath, T instance, Action<Stream, T> serializer);

        T Read<T>(string filePath, Func<Stream, T> deserializer);

        IEnumerable<byte> ReadBytes(string filePath);

        IEnumerable<string> ReadAllLines(string filePath);

        string ReadAllText(string filePath);

        void Write(string filePath, string contents, bool append = false);

        void WriteAllBytes(string filePath, byte[] bytes, bool append = false);

        void WriteAllText(string filePath, string contents);

        void Delete(string filePath);
    }
}