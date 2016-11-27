namespace EyssyApps.Core.Library.Native
{
    using System;
    using System.Runtime.InteropServices;

    internal struct LASTINPUTINFO
    {
        public uint cbSize;

        public uint dwTime;
    }

    public static class NativeMethods
    {
        public const string User32 = "User32.dll",
            Kernel32 = "Kernel32.dll";

        public const int HWND_BROADCAST = 0xffff;

        public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");

        [DllImport(NativeMethods.User32)]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport(NativeMethods.User32)]
        public static extern int RegisterWindowMessage(string message);

        [DllImport(NativeMethods.User32)]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [DllImport(NativeMethods.Kernel32)]
        private static extern uint GetLastError();

        public static TimeSpan GetIdleTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)Marshal.SizeOf(lastInPut);
            NativeMethods.GetLastInputInfo(ref lastInPut);

            uint idleTime = ((uint)Environment.TickCount - lastInPut.dwTime);

            return TimeSpan.FromMilliseconds(idleTime);
        }

        /// <summary>
        /// Get the Last input time in milliseconds
        /// </summary>
        /// <returns></returns>
        public static long GetLastInputTime()
        {
            LASTINPUTINFO lastInPut = new LASTINPUTINFO();
            lastInPut.cbSize = (uint)Marshal.SizeOf(lastInPut);

            if (!NativeMethods.GetLastInputInfo(ref lastInPut))
            {
                throw new Exception(GetLastError().ToString());
            }

            return lastInPut.dwTime;
        }
    }
}