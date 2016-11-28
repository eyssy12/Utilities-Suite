namespace Zagorapps.Core.Library.Extensions
{
    using System;
    public static class ObjectExtensions
    {
        public static bool TryDispose(this object @object)
        {
            IDisposable disposable = @object as IDisposable;

            if (disposable == null)
            {
                return false;
            }

            disposable.Dispose();

            return true;
        }

        public static void TryDisposeAll(params object[] objects)
        {
            objects.ForEach(o => o.TryDispose());
        }
    }
}