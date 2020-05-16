using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Insider.UIStreaming.Server.WebSockets
{
    public class HttpListenerWebSocketUIStreamingServer : IUIStreamingServer
    {
        private readonly ConcurrentQueue<byte[]> _messagesQueue;

        private readonly HttpListener _httpListener = new HttpListener();

        private readonly ConcurrentDictionary<Guid, WebSocket> _connections;

        private readonly IHttpListenerWebSocketUIStreamingServerSettings _settings;

        public HttpListenerWebSocketUIStreamingServer(IHttpListenerWebSocketUIStreamingServerSettings settings, ConcurrentQueue<byte[]> messagesQueue)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            _messagesQueue = messagesQueue ?? throw new ArgumentNullException(nameof(messagesQueue));

            _connections = new ConcurrentDictionary<Guid, WebSocket>();
        }

        public void Run()
        {
            if (HttpListener.IsSupported)
            {
                foreach (string prefix in _settings.ListeningUriPrefixes)
                {
                    _httpListener.Prefixes.Add(prefix);
                }

                _httpListener.Start();
                Task.Factory.StartNew(async () => await StreamAsync());
                Task.Factory.StartNew(async () => await AcceptAsync(
                    onAccept: ws => Task.Factory.StartNew(async () => await CatchCloseAsync(ws))));
            }
        }

        private async Task StreamAsync()
        {
            int counter = 0;

            while (_httpListener.IsListening)
            {
                if (_messagesQueue.IsEmpty)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    continue;
                }

                if (!_messagesQueue.TryDequeue(out byte[] buffer))
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    continue;
                }

                Debug.WriteLine($"{++counter}.\tsend to {_connections.Count} connections: {Encoding.UTF8.GetString(buffer)}");

                foreach ((Guid id, WebSocket webSocket) in _connections)
                {
                    if (webSocket.State == WebSocketState.Open)
                    {
                        await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
        }

        private async Task AcceptAsync(Action<WebSocket> onAccept)
        {
            while (_httpListener.IsListening)
            {
                var context = await _httpListener.GetContextAsync();

                if (context.Request.RawUrl == _settings.AcceptingPath)
                {
                    var webSocketContext = await context
                        .AcceptWebSocketAsync(subProtocol: null,
                            receiveBufferSize: _settings.ReceiveBufferSize,
                            keepAliveInterval: _settings.KeepAliveInterval);

                    onAccept(webSocketContext.WebSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
        }

        private async Task CatchCloseAsync(WebSocket webSocket)
        {
            Guid id = Guid.NewGuid();

            _connections[id] = webSocket;

            var buffer = new Memory<byte>(new byte[1024]);

            var received = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

            while (received.MessageType != WebSocketMessageType.Close)
            {
                received = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
            }

            _connections.TryRemove(id, out WebSocket _);
        }

        public async Task StopAsync()
        {
            if (HttpListener.IsSupported)
            {
                foreach ((Guid id, WebSocket webSocket) in _connections)
                {
                    if (webSocket.State == WebSocketState.Open)
                    {
                        await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Server Stopped", CancellationToken.None);
                    }
                }

                _httpListener.Stop();
            }
        }
    }
}
