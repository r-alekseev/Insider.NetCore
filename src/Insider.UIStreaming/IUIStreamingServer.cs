using System.Threading.Tasks;

namespace Insider.UIStreaming
{
    public interface IUIStreamingServer
    {
        void Run();
        Task StopAsync();
    }
}
