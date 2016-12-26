namespace Zagorapps.Core.Library.Events
{
    public class EventArgs<TFirst, TSecond, TThird, TFourth> : EventArgs<TFirst, TSecond, TThird>
    {
        private readonly TFourth fourth;

        public EventArgs(TFirst first, TSecond second, TThird third, TFourth fourth) :
            base(first, second, third)
        {
            this.fourth = fourth;
        }

        public TFourth Fourth
        {
            get { return this.fourth; }
        }
    }
}