using System;

namespace Insider.UIStreaming.Server.WebSockets
{
    public interface IHttpListenerWebSocketUIStreamingServerSettings
    {
        TimeSpan KeepAliveInterval { get; }
        int ReceiveBufferSize { get; }
        string AcceptingPath { get; }
        string[] ListeningUriPrefixes { get; }
    }
}
