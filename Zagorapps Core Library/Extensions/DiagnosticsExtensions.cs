namespace Zagorapps.Core.Library.Extensions
{
    using System;
    using System.Diagnostics;

    public static class DiagnosticsExtensions
    {
        public static bool IsProcessRunning(this int processId)
        {
            try
            {
                Process.GetProcessById(processId);
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

        public static bool IsProcessRunning(this Process process)
        {
            return process.Id.IsProcessRunning();
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