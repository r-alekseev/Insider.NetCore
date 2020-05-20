using System;

namespace Insider.UIStreaming.Server.WebSockets
{
    public class HttpListenerUIWebServerSettings : IHttpListenerUIWebServerSettings
    {
        public string AcceptingPath { get; set; } = "/insider/ui-web";
        public string[] ListeningUriPrefixes { get; set; } = new[] { "http://localhost:8080/" };
    }
}
