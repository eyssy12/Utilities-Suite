namespace Zagorapps.Utilities.Suite.Library.Interop
{
    using System;
    using System.Windows;
    using System.Windows.Interop;

    public class InteropHandle : IInteropHandle
    {
        protected readonly WindowInteropHelper InteropHelper;

        public InteropHandle(Window window)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window), "No WPF window provided.");
            }

            this.InteropHelper = new WindowInteropHelper(window);
        }

        public IntPtr Handle
        {
            get { return this.InteropHelper.Handle; }
        }
    }
}