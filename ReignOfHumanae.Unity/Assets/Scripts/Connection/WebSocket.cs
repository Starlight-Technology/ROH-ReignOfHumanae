using Newtonsoft.Json;

using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Assembly_CSharp.Assets.Scripts.Connection
{
    public class WebSocket
    {
        private readonly ClientWebSocket _webSocket;

        public WebSocket()
        {
            _webSocket = new();
        }

        /// <summary>
        /// Connect to WebSocket
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        {
            await _webSocket.ConnectAsync(uri, cancellationToken);
        }

        public async Task SendJsonAsync(object payload, CancellationToken cancellationToken)
        {
            string json = JsonConvert.SerializeObject(payload);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(json);
            await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
        }

        public async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            await _webSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }
    }
}