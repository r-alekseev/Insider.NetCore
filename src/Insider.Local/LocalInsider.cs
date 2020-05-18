using Insider.UIStreaming;
using Insider.UIWeb;
using System;
using System.Threading.Tasks;

namespace Insider.Local
{
    public class LocalInsider : IInsider
    {
        private readonly IUIStreamingServer _uiStreamingServer;
        private readonly IUIStreamingProtocol _uiStreamingProtocol;

        private readonly IUIWebServer _uiWebServer;

        public LocalInsider(
            IUIStreamingServer uiStreamingServer,
            IUIStreamingProtocol uiStreamingProtocol,
            IUIWebServer uiWebServer)
        {
            _uiStreamingServer = uiStreamingServer ?? throw new ArgumentNullException(nameof(uiStreamingServer));
            _uiStreamingProtocol = uiStreamingProtocol ?? throw new ArgumentNullException(nameof(uiStreamingProtocol));

            _uiWebServer = uiWebServer ?? throw new ArgumentNullException(nameof(uiWebServer));
        }

        public void SetMetric(string[] key, int count, TimeSpan duration) => _uiStreamingProtocol
            .SetMetric(key, count, duration);

        public void SetState(string[] key, string value) => _uiStreamingProtocol
            .SetState(key, value);

        public void Run()
        {
            _uiStreamingServer.Run();
            _uiWebServer.Run();
        }

        public async Task StopAsync()
        {
            await _uiWebServer.StopAsync();
            await _uiStreamingServer.StopAsync();
        }
    }
}
