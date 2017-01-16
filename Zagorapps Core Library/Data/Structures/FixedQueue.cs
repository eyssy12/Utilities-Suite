namespace Zagorapps.Core.Library.Data.Structures
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class FixedQueue<T>
    {
        private readonly object @lock = new object();

        private readonly ConcurrentQueue<T> queue;
        private readonly int maxCapacity;

        public FixedQueue(int maxCapacity)
        {
            this.queue = new ConcurrentQueue<T>();
            this.maxCapacity = maxCapacity;
        }

        public void Enqueue(T item)
        {
            lock (@lock)
            {
                if (this.queue.Count == maxCapacity)
                {
                    this.Dequeue();
                }

                this.queue.Enqueue(item);
            }
        }

        public void Clear()
        {
            lock (@lock)
            {
                while (this.queue.Count > 0)
                {
                    this.Dequeue();
                }
            }
        }

        public bool Any()
        {
            lock (@lock)
            {
                return this.queue.Count > 0;
            }
        }

        public IEnumerable<T> GetAll()
        {
            lock (@lock)
            {
                return this.queue.ToArray();
            }
        }

        private void Dequeue()
        {
            T item;
            this.queue.TryDequeue(out item);
        }
    }
}