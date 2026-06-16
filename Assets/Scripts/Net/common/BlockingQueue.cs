using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LekeNet
{
    class BlockingQueue<T>
    {
        private readonly Queue<T> queue = new Queue<T>();
        public BlockingQueue()
        {

        }

        public void Enqueue(T item)
        {
            lock (queue)
            {
                queue.Enqueue(item);
                if (queue.Count == 1)
                {
                    Monitor.PulseAll(queue);
                }
            }
        }
        public T Dequeue()
        {
            lock (queue)
            {
                while (queue.Count == 0)
                {
                    if (closing)
                    {
                        return default(T);
                    }
                    Monitor.Wait(queue);
                }
                T item = queue.Dequeue();
                return item;
            }
        }

        public void Reset()
        {
            Close();
            lock (queue)
            {
                this.queue.Clear();
            }
            closing = false;
        }

        volatile bool closing;
        public void Close()
        {
            lock (queue)
            {
                closing = true;
                Monitor.PulseAll(queue);
            }
        }

        public int Count
        {
            get
            {
                return queue.Count;
            }
        }

    }
}
