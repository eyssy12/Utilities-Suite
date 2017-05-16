namespace Zagorapps.Graphics.Library.Extensions
{
    using System;
    using System.Drawing;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;
    using Microsoft.Win32.SafeHandles;

    public static class GraphicsExtensions
    {
        public static BitmapSource ToBitmapSource(this Bitmap source)
        {
            using (SafeHBitmapHandle handle = new SafeHBitmapHandle(source))
            {
                return Imaging.CreateBitmapSourceFromHBitmap(
                    handle.DangerousGetHandle(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
        }

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        private sealed class SafeHBitmapHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            [SecurityCritical]
            public SafeHBitmapHandle(Bitmap bitmap) : base(true)
            {
                this.SetHandle(bitmap.GetHbitmap());
            }

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            protected override bool ReleaseHandle()
            {
                return GraphicsExtensions.DeleteObject(this.handle) > 0;
            }
        }
    }
}