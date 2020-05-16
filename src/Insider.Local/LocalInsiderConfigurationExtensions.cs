using Insider;
using Insider.Local;
using Insider.UIStreaming;
using Insider.UIStreaming.Protocol.JsonRpc;
using Insider.UIStreaming.Server.WebSockets;
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

            return new LocalInsider(uiStreamingServer, uiStreamingProtocol);
        }


        public static ILocalInsiderConfiguration ConfigureDefault(this ILocalInsiderConfiguration configuration)
        {
            var messagesQueue = new ConcurrentQueue<byte[]>();

            return configuration
                .UseJsonRpcUIStreamingProtocol(messagesQueue)
                .UseWebSocketUIStreamingServer(new HttpListenerWebSocketUIStreamingServerSettings(), messagesQueue);
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


        public static ILocalInsiderConfiguration UseDummyUIStreamingServer(this ILocalInsiderConfiguration configuration) => configuration
            .UseUIStreamingServer(create: () => new DummyUIStreamingServer());

        public static ILocalInsiderConfiguration UseDummyUIStreamingProtocol(this ILocalInsiderConfiguration configuration) => configuration
            .UseUIStreamingProtocol(create: () => new DummyUIStreamingProtocol());


        public static ILocalInsiderConfiguration UseWebSocketUIStreamingServer(this ILocalInsiderConfiguration configuration, HttpListenerWebSocketUIStreamingServerSettings settings, ConcurrentQueue<byte[]> messagesQueue) => configuration
            .UseUIStreamingServer(create: () => new HttpListenerWebSocketUIStreamingServer(settings, messagesQueue));

        public static ILocalInsiderConfiguration UseJsonRpcUIStreamingProtocol(this ILocalInsiderConfiguration configuration, ConcurrentQueue<byte[]> messagesQueue) => configuration
            .UseUIStreamingProtocol(create: () => new JsonRpcUIStreamingProtocol(messagesQueue));
    }
}
