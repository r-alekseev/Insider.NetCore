using System;

namespace Insider.UIStreaming
{
    public interface IUIStreamingProtocol
    {
        void SetState(string[] key, string value);
        void SetMetric(string[] key, int count, TimeSpan duration);
    }
}
