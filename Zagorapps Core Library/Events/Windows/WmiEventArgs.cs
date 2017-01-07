namespace Zagorapps.Core.Library.Events.Windows
{
    using System;

    public class WmiEventArgs : EventArgs
    {
        private readonly bool isActive;
        private readonly byte brightness;
        private readonly string instanceName;

        public WmiEventArgs(bool isActive, byte brightness, string instanceName)
        {
            this.isActive = isActive;
            this.brightness = brightness;
            this.instanceName = instanceName;
        }

        public bool IsActive { get { return this.isActive; } }

        public byte Brightness { get { return this.brightness; } }

        public string InstanceName { get { return this.instanceName; } }
    }
}