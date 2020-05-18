using Insider.UIStreaming;
using Insider.UIWeb;
using System;

namespace Insider
{
    public class LocalInsiderConfiguration : ILocalInsiderConfiguration
    {
        public Func<IUIStreamingServer> CreateUIStreamingServer { get; set; } = () => new DummyUIStreamingServer();
        public Func<IUIStreamingProtocol> CreateUIStreamingProtocol { get; set; } = () => new DummyUIStreamingProtocol();

        public Func<IUIWebServer> CreateUIWebServer { get; set; } = () => new DummyUIWebServer();
    }
}
