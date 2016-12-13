namespace Zagorapps.Bluetooth.Library.Messaging
{
    using System.Xml.Linq;

    public interface IXmlMessage : IMessage
    {
        XDocument Source { get; }
    }
}