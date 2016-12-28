namespace Zagorapps.Core.Library.Extensions
{
    using System;
    using System.Diagnostics;

    public static class DiagnosticsExtensions
    {
        public static bool IsProcessRunning(this Process process)
        {
            try
            {
                Process.GetProcessById(process.Id);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
        
        public static string GetTotalProcessorTime(this Process process)
        {
            try
            {
                return process.TotalProcessorTime.TotalSeconds.ToString();
            }
            catch
            {
                return "Access Denied";
            }
        }
    }
}