namespace Zagorapps.Bluetooth.Library.Data
{
    using System;
    using Zagorapps.Core.Library.Events;

    public interface IOperation
    {
        event EventHandler<EventArgs<DateTime>> OperationStarted;
    }
}