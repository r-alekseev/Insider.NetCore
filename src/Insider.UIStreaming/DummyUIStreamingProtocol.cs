using System;

namespace Insider.UIStreaming
{
    public class DummyUIStreamingProtocol : IUIStreamingProtocol
    {
        public void SetMetric(string[] key, int count, TimeSpan duration)
        {
        }

        public void SetState(string[] key, string value)
        {
        }
    }
}
