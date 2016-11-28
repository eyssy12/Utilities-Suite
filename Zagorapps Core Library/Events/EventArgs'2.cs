namespace Zagorapps.Core.Library.Events
{
    public class EventArgs<TFirst, TSecond> : EventArgs<TFirst>
    {
        private readonly TSecond second;
         
        public EventArgs(TFirst first, TSecond second) 
            : base(first)
        {
            this.second = second;
        }

        public TSecond Second
        {
            get { return this.second; }
        }
    }
}