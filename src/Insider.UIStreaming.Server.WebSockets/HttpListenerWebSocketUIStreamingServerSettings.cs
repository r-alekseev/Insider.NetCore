using System;

namespace Insider.UIStreaming.Server.WebSockets
{
    public class HttpListenerWebSocketUIStreamingServerSettings : IHttpListenerWebSocketUIStreamingServerSettings
    {
        public TimeSpan KeepAliveInterval { get; set; } = TimeSpan.FromMinutes(2);
        public int ReceiveBufferSize { get; set; } = 1024 * 4;
        public string AcceptingPath { get; set; } = "/insider/ui-streaming";
        public string[] ListeningUriPrefixes { get; set; } = new[] { "http://localhost:8888/" };
    }
}
