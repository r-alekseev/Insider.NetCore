using Insider.UIStreaming;
using System;
using System.Threading.Tasks;

namespace Insider.Local
{
    public class LocalInsider : IInsider
    {
        private readonly IUIStreamingServer _uiStreamingServer;
        private readonly IUIStreamingProtocol _uiStreamingProtocol;

        public LocalInsider(
            IUIStreamingServer uiStreamingServer,
            IUIStreamingProtocol uiStreamingProtocol)
        {
            _uiStreamingServer = uiStreamingServer ?? throw new ArgumentNullException(nameof(uiStreamingServer));
            _uiStreamingProtocol = uiStreamingProtocol ?? throw new ArgumentNullException(nameof(uiStreamingProtocol));
        }

        public void SetMetric(string[] key, int count, TimeSpan duration) => _uiStreamingProtocol
            .SetMetric(key, count, duration);

        public void SetState(string[] key, string value) => _uiStreamingProtocol
            .SetState(key, value);

        public void Run() => _uiStreamingServer
            .Run();

        public Task StopAsync()=> _uiStreamingServer
            .StopAsync();
    }
}
