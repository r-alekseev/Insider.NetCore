using Insider.Server.UIStreaming;
using System;

namespace Insider
{
    public interface ILocalInsiderConfiguration
    {
        Func<IUIStreamingServer> CreateUIStreamingServer { get; set; }
    }
}
