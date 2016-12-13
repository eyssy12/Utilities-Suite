namespace Zagorapps.Bluetooth.Library.Messaging
{
    using System;
    using System.Xml.Linq;

    public class XmlMessage : IXmlMessage
    {
        private readonly XDocument source;

        public XmlMessage(XDocument source)
        {
            this.source = source;
        }

        public XDocument Source
        {
            get { return this.source; }
        }

        public string Prepare()
        {
            return this.Source.ToString(SaveOptions.DisableFormatting);
        }
    }
}