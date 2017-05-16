namespace Zagorapps.Core.Library.Windows
{
    using System;
    using System.Collections.Generic;
    using Events.Windows;
    using Execution;
    using Models;

    public interface IWmiManagementService : IProcess, IRaiseFailures, IDisposable
    {
        event EventHandler<WmiEventArgs> EventReceived;

        bool IsWmiSupported { get; }

        IEnumerable<WmiDeviceInfo> GetBrightnesses();

        void SetBrightness(int value);
    }
}