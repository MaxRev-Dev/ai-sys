using System.Collections;
using System.Collections.Generic;

namespace ObjectDetectionWeb.Data
{
    public class LogQueue : IEnumerable<string>
    {
        private readonly Queue<string> _queue = new Queue<string>();
        public int Count => _queue.Count;
        public void Enqueue(string v) => _queue.Enqueue(v);
        public string Dequeue() => _queue.Dequeue();

        /// <inheritdoc />
        public IEnumerator<string> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_queue).GetEnumerator();
        }
    }
}