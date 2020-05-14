using Insider;
using Insider.Local;
using Insider.Server.UIStreaming;
using System;

namespace Profiler
{
    public static class LocalInsiderConfigurationExtensions
    {
        public static LocalInsider CreateInsider(this ILocalInsiderConfiguration settings)
        {
            return new LocalInsider(settings);
        }

        public static ILocalInsiderConfiguration ConfigureDefault(this ILocalInsiderConfiguration settings)
        {
            // add websockets ui-streaming
            // add ui-web
            return settings;
        }

        public static ILocalInsiderConfiguration UseUIStreaming(this ILocalInsiderConfiguration settings, Func<IUIStreamingServer> create)
        {
            settings.CreateUIStreamingServer = create;
            return settings;
        }

        public static ILocalInsiderConfiguration UseDummyUIStreaming(this ILocalInsiderConfiguration settings) => settings
            .UseUIStreaming(create: () => new DummyUIStreamingServer());
    }
}
