using Insider.Server.UIStreaming;
using System;

namespace Insider
{
    public class LocalInsiderConfiguration : ILocalInsiderConfiguration
    {
        public Func<IUIStreamingServer> CreateUIStreamingServer { get; set; } = () => new DummyUIStreamingServer();
    }
}
