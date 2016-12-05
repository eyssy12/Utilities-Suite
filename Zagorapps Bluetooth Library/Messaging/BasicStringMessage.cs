namespace Zagorapps.Bluetooth.Library.Messaging
{
    using System;

    public class BasicStringMessage : IBasicStringMessage
    {
        protected readonly string Payload;

        public BasicStringMessage(string payload)
        {
            if (string.IsNullOrWhiteSpace(payload))
            {
                throw new ArgumentNullException(nameof(payload), "message");
            }

            this.Payload = payload;
        }

        public string Prepare()
        {
            return this.Payload;
        }
    }
}