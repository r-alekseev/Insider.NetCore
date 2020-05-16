using Insider.UIStreaming;
using System;

namespace Insider
{
    public interface ILocalInsiderConfiguration
    {
        Func<IUIStreamingServer> CreateUIStreamingServer { get; set; }
        Func<IUIStreamingProtocol> CreateUIStreamingProtocol { get; set; }
    }
}
