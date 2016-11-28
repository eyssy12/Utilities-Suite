namespace Zagorapps.Core.Library.Events
{
    using System;

    public class EventArgs<TFirst> : EventArgs
    {
        private readonly TFirst first;

        public EventArgs(TFirst first)
        {
            this.first = first;
        }

        public TFirst First
        {
            get { return this.first; }
        }
    }
}