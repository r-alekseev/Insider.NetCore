using System.Threading.Tasks;

namespace Insider.UIStreaming
{
    public class DummyUIStreamingServer : IUIStreamingServer
    {
        public void Run()
        {
        }

        public async Task StopAsync() => await Task.Yield();
    }
}
