namespace Zagorapps.Bluetooth.Library.Processors
{
    using System.Collections.Generic;
    using Core.Library.Execution;

    public interface ICommandsProcessor : IRaiseFailures
    {
        void Process(IEnumerable<byte> data);
    }
}