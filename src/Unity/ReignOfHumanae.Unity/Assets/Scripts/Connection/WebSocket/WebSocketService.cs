using MessagePack;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Connection.WebSocket
{
    public class WebSocketService
    {
        private ClientWebSocket _socket;
        private CancellationTokenSource _cts;

        private readonly ConcurrentQueue<byte[]> _receiveQueue = new();

        public event Action OnConnected;
        public event Action OnDisconnected;
        public event Action<string> OnError;
        public event Action<byte[]> OnMessage;

        public async Task ConnectAsync(string url)
        {
            _socket = new ClientWebSocket();
            _cts = new CancellationTokenSource();

            try
            {
                await _socket.ConnectAsync(new Uri(url), _cts.Token);
                OnConnected?.Invoke();

                _ = Task.Run(ReceiveLoop);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        public async Task SendAsync(object message)
        {
            if (_socket == null || _socket.State != WebSocketState.Open)
                return;

            byte[] data = MessagePackSerializer.Serialize(message);

            await _socket.SendAsync(
                new ArraySegment<byte>(data),
                WebSocketMessageType.Binary,
                true,
                _cts.Token);
        }

        private async Task ReceiveLoop()
        {
            var buffer = new byte[8192];

            try
            {
                while (_socket.State == WebSocketState.Open)
                {
                    using var ms = new MemoryStream();
                    WebSocketReceiveResult result;

                    do
                    {
                        result = await _socket.ReceiveAsync(
                            new ArraySegment<byte>(buffer),
                            _cts.Token);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await CloseAsync();
                            return;
                        }

                        ms.Write(buffer, 0, result.Count);

                        Debug.Log($"[WS RAW] frame received, bytes={result.Count}, end={result.EndOfMessage}");

                    } while (!result.EndOfMessage);

                    _receiveQueue.Enqueue(ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message);
            }
        }

        public void Dispatch()
        {
            while (_receiveQueue.TryDequeue(out var data))
            {
                OnMessage?.Invoke(data);
            }
        }

        public async Task CloseAsync()
        {
            try
            {
                if (_socket.State == WebSocketState.Open)
                {
                    await _socket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Client closing",
                        CancellationToken.None);
                }
            }
            catch { }

            OnDisconnected?.Invoke();
        }
    }
}
