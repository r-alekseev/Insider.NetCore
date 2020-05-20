using Insider.UIWeb;
using Insider.UIWeb.Content;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Insider.UIStreaming.Server.WebSockets
{
    public class HttpListenerUIWebServer : IUIWebServer
    {
        private readonly IHttpListenerUIWebServerSettings _settings;

        private readonly HttpListener _httpListener = new HttpListener();

        private const string INDEX_PAGE_EMBEDDED_RESOURCE_NAME = "Insider.UIWeb.Content.Insider_Web.content.index.html";
        private ReadOnlyMemory<byte> _indexPage;

        public HttpListenerUIWebServer(IHttpListenerUIWebServerSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public void Run()
        {
            if (HttpListener.IsSupported)
            {
                foreach (string prefix in _settings.ListeningUriPrefixes)
                {
                    _httpListener.Prefixes.Add(prefix);
                }

                LoadIndexPage();

                _httpListener.Start();

                Task.Factory.StartNew(async () => await AcceptAsync());
            }
        }

        private void LoadIndexPage()
        {
            var assembly = Assembly.GetAssembly(typeof(ContentAssemblyType));
            var ms = new MemoryStream();
            using (var mrs = assembly.GetManifestResourceStream(INDEX_PAGE_EMBEDDED_RESOURCE_NAME))
            {
                mrs.CopyTo(ms);
            }
            _indexPage = new ReadOnlyMemory<byte>(ms.ToArray());
        }

        private async Task AcceptAsync()
        {
            while (_httpListener.IsListening)
            {
                var context = await _httpListener.GetContextAsync();

                if (context.Request.IsWebSocketRequest || 
                    context.Request.HttpMethod != "GET" || 
                    context.Request.RawUrl != _settings.AcceptingPath)
                {
                    context.Response.StatusCode = 400;
                }

                context.Response.ContentType = "text/html";
                context.Response.StatusCode = 200;
                context.Response.ContentLength64 = _indexPage.Length;

                await context.Response.OutputStream.WriteAsync(_indexPage);

                context.Response.OutputStream.Close();
            }
        }

        public async Task StopAsync()
        {
            if (HttpListener.IsSupported)
            {
                _httpListener.Stop();
            }

            await Task.Yield();
        }
    }
}
