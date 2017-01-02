namespace Zagorapps.Utilities.Suite.UI.Comparators
{
    using System.Collections.Generic;
    using ViewModels;

    public class ProcessViewModelComparator : IEqualityComparer<ProcessViewModel>
    {
        public bool Equals(ProcessViewModel x, ProcessViewModel y)
        {
            return x.ProcessName == y.ProcessName;
        }

        public int GetHashCode(ProcessViewModel obj)
        {
            return 0;
        }
    }
}