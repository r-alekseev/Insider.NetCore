using Insider.UIStreaming;
using System;

namespace Insider
{
    public class LocalInsiderConfiguration : ILocalInsiderConfiguration
    {
        public Func<IUIStreamingServer> CreateUIStreamingServer { get; set; } = () => new DummyUIStreamingServer();
        public Func<IUIStreamingProtocol> CreateUIStreamingProtocol { get; set; } = () => new DummyUIStreamingProtocol();
    }
}
