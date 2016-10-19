namespace EyssyApps.Core.Library.Events
{
    using System;

    public static class Invoker
    {
        public static void Raise<TEventArgs>(ref EventHandler<TEventArgs> handler, object sender, TEventArgs args)
        {
            EventHandler<TEventArgs> snapshot = handler;

            if (snapshot != null)
            {
                snapshot(sender, args);
            }
        }

        public static void Raise(ref EventHandler<EventArgs> handler, object sender)
        {
            Invoker.Raise(ref handler, sender, EventArgs.Empty);
        }

        public static void Raise<T1>(ref EventHandler<EventArgs<T1>> handler, object sender, T1 param1)
        {
            Invoker.Raise(ref handler, sender, new EventArgs<T1>(param1));
        }

        public static void Raise<T1, T2>(ref EventHandler<EventArgs<T1, T2>> handler, object sender, T1 param1, T2 param2)
        {
            Invoker.Raise(ref handler, sender, new EventArgs<T1, T2>(param1, param2));
        }
    }
}