namespace Zagorapps.Utilities.Suite.Library.Interoperability
{
    using System;
    using System.Windows;
    using System.Windows.Interop;

    public class WindowInteropHandle : IInteropHandle
    {
        protected readonly WindowInteropHelper InteropHelper;

        public WindowInteropHandle(Window window)
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