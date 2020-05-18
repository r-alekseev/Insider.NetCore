using Insider;
using Insider.Local;
using Insider.UIStreaming;
using Insider.UIStreaming.Protocol.JsonRpc;
using Insider.UIStreaming.Server.WebSockets;
using Insider.UIWeb;
using System;
using System.Collections.Concurrent;

namespace Profiler
{
    public static class LocalInsiderConfigurationExtensions
    {
        public static LocalInsider CreateInsider(this ILocalInsiderConfiguration configuration)
        {
            var uiStreamingServer = configuration.CreateUIStreamingServer();
            var uiStreamingProtocol = configuration.CreateUIStreamingProtocol();
            var uiWebServer = configuration.CreateUIWebServer();

            return new LocalInsider(uiStreamingServer, uiStreamingProtocol, uiWebServer);
        }


        public static ILocalInsiderConfiguration UseUIStreamingServer(this ILocalInsiderConfiguration configuration, Func<IUIStreamingServer> create)
        {
            configuration.CreateUIStreamingServer = create;
            return configuration;
        }

        public static ILocalInsiderConfiguration UseUIStreamingProtocol(this ILocalInsiderConfiguration configuration, Func<IUIStreamingProtocol> create)
        {
            configuration.CreateUIStreamingProtocol = create;
            return configuration;
        }

        public static ILocalInsiderConfiguration UseUIWebServer(this ILocalInsiderConfiguration configuration, Func<IUIWebServer> create)
        {
            configuration.CreateUIWebServer = create;
            return configuration;
        }


        public static ILocalInsiderConfiguration UseDummyUIStreamingServer(this ILocalInsiderConfiguration configuration) => configuration
            .UseUIStreamingServer(create: () => new DummyUIStreamingServer());

        public static ILocalInsiderConfiguration UseDummyUIStreamingProtocol(this ILocalInsiderConfiguration configuration) => configuration
            .UseUIStreamingProtocol(create: () => new DummyUIStreamingProtocol());

        public static ILocalInsiderConfiguration UseDummyUIWebServer(this ILocalInsiderConfiguration configuration) => configuration
            .UseUIWebServer(create: () => new DummyUIWebServer());


        public static ILocalInsiderConfiguration UseWebSocketUIStreamingServer(this ILocalInsiderConfiguration configuration, IHttpListenerWebSocketUIStreamingServerSettings settings, ConcurrentQueue<byte[]> messagesQueue) => configuration
            .UseUIStreamingServer(create: () => new HttpListenerWebSocketUIStreamingServer(settings, messagesQueue));

        public static ILocalInsiderConfiguration UseJsonRpcUIStreamingProtocol(this ILocalInsiderConfiguration configuration, ConcurrentQueue<byte[]> messagesQueue) => configuration
            .UseUIStreamingProtocol(create: () => new JsonRpcUIStreamingProtocol(messagesQueue));

        public static ILocalInsiderConfiguration UseHttpUIWebServer(this ILocalInsiderConfiguration configuration, IHttpListenerUIWebServerSettings settings) => configuration
            .UseUIWebServer(create: () => new HttpListenerUIWebServer(settings));


        public static ILocalInsiderConfiguration ConfigureDefault(this ILocalInsiderConfiguration configuration)
        {
            var messagesQueue = new ConcurrentQueue<byte[]>();

            return configuration
                .UseJsonRpcUIStreamingProtocol(messagesQueue)
                .UseWebSocketUIStreamingServer(new HttpListenerWebSocketUIStreamingServerSettings(), messagesQueue)
                .UseHttpUIWebServer(new HttpListenerUIWebServerSettings());
        }
    }
}
