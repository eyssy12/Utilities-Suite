namespace Zagorapps.Core.Library.Events
{
    public class EventArgs<TFirst, TSecond, TThird> : EventArgs<TFirst, TSecond>
    {
        private readonly TThird third;

        public EventArgs(TFirst first, TSecond second, TThird third)
            : base(first, second)
        {
            this.third = third;
        }

        public TThird Third
        {
            get { return this.third; }
        }
    }
}