using System;

namespace Insider.UIStreaming.Server.WebSockets
{
    public interface IHttpListenerUIWebServerSettings
    {
        string AcceptingPath { get; }
        string[] ListeningUriPrefixes { get; }
    }
}
