using System;
using System.Collections.Concurrent;
using System.Text;

namespace Insider.UIStreaming.Protocol.JsonRpc
{
    public class JsonRpcUIStreamingProtocol : IUIStreamingProtocol
    {
        private readonly ConcurrentQueue<byte[]> _messagesQueue;

        public JsonRpcUIStreamingProtocol(ConcurrentQueue<byte[]> messagesQueue)
        {
            _messagesQueue = messagesQueue ?? throw new ArgumentNullException(nameof(messagesQueue));
        }

        private void Enqueue(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            _messagesQueue.Enqueue(buffer);
        }

        public void SetMetric(string[] key, int count, TimeSpan duration)
        {
            Enqueue($"{{\"jsonrpc\": \"2.0\", \"method\": \"metric-set\", \"params\": {{\"key\": \"{string.Join('.', key)}\", \"count\": {count}, \"duration\": {Convert.ToInt32(duration.TotalMilliseconds)}}}");
        }

        public void SetState(string[] key, string value)
        {
            Enqueue($"{{\"jsonrpc\": \"2.0\", \"method\": \"state-set\", \"params\": {{\"key\": \"{string.Join('.', key)}\", \"value\": \"{value}\"}}");
        }
    }
}
