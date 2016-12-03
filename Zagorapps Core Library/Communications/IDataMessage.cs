namespace Zagorapps.Core.Library.Communications
{
    using System;
    using System.Runtime.Serialization;

    public interface IDataMessage : ISerializable
    {
        DateTime CreatedTime { get; }
    }
}