namespace Zagorapps.Core.Library.Windows
{
    using System;
    using System.Collections.Generic;
    using Events.Windows;
    using Execution;
    using Models;

    public interface IWmiManagementService : IProcess, IDisposable
    {
        event EventHandler<WmiEventArgs> EventReceived;

        IEnumerable<WmiDeviceInfo> GetBrightnesses();

        void SetBrightness(int value);
    }
}