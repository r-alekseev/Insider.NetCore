using System.Threading.Tasks;

namespace Insider.UIWeb
{
    public class DummyUIWebServer : IUIWebServer
    {
        public void Run()
        {
        }

        public async Task StopAsync() => await Task.Yield();
    }
}
