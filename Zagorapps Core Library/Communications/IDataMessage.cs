namespace Zagorapps.Core.Library.Communications
{
    using System;
    using System.Runtime.Serialization;

    public interface IDataMessage : ISerializable
    {
        string From { get; }

        DateTime CreatedTime { get; }

        object Data { get; }
    }
}