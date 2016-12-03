namespace Zagorapps.Utilities.Library.Communications
{
    using System;
    using Core.Library.Communications;
    using Core.Library.Events;

    public interface IReceiveData
    {
        event EventHandler<EventArgs<IDataMessage>> MessageReceived;
    }
}