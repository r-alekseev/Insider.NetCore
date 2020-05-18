using Insider.UIStreaming;
using Insider.UIWeb;
using System;

namespace Insider
{
    public interface ILocalInsiderConfiguration
    {
        Func<IUIStreamingServer> CreateUIStreamingServer { get; set; }
        Func<IUIStreamingProtocol> CreateUIStreamingProtocol { get; set; }

        Func<IUIWebServer> CreateUIWebServer { get; set; }
    }
}
