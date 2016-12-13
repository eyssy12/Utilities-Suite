namespace Zagorapps.Bluetooth.Library.Messaging
{
    using ProtoBuf;

    [ProtoContract]
    public class ProtobufMessage : IProtobufMessage
    {
        public string Prepare()
        {
            return string.Empty;
        }
    }
}