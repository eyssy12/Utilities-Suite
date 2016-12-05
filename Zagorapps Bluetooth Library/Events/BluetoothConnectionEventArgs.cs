namespace Zagorapps.Bluetooth.Library.Events
{
    using Core.Library.Events;

    public class BluetoothConnectionEventArgs : EventArgs<string, byte[]>
    {
        public BluetoothConnectionEventArgs(string raiser, byte[] arg)
            : base(raiser, arg)
        {
        }

        public string Raiser
        {
            get { return this.First; }
        }

        public byte[] Arg
        {
            get { return this.Second; }
        }
    }
}