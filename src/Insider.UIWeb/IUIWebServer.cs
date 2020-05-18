using System.Threading.Tasks;

namespace Insider.UIWeb
{
    public interface IUIWebServer
    {
        void Run();
        Task StopAsync();
    }
}
