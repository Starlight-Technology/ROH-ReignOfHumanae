using Assets.Scripts.Models.Character;

using MessagePack;
using MessagePack.Resolvers;

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
        private readonly SemaphoreSlim _sendLock = new(1, 1);

        private readonly ConcurrentQueue<RealtimeEnvelope> _receiveQueue = new();

        public event Action OnConnected;
        public event Action OnDisconnected;
        public event Action<string> OnError;
        public event Action<RealtimeEnvelope> OnMessage;

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

        public async Task SendAsync(RealtimeEnvelope message)
        {
            if (_socket?.State != WebSocketState.Open)
            {
                Debug.LogWarning("[WS] Cannot send, WebSocket is not open");
                return;
            }

            try
            {
                if (!MessagePackBootstrap.IsInitialized)
                {
                    MessagePackBootstrap.Initialize();
                }

                var options = MessagePackSerializerOptions.Standard
                                .WithResolver(StandardResolver.Instance)
                                .WithSecurity(MessagePackSecurity.UntrustedData);
                var bytes = MessagePackSerializer.Serialize(message, options);

                await _sendLock.WaitAsync();
                try
                {
                    await _socket.SendAsync(
                        new ArraySegment<byte>(bytes),
                        WebSocketMessageType.Binary,
                        true,
                        CancellationToken.None
                    );
                }
                finally
                {
                    _sendLock.Release();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[WS] Send error: {ex.Message}");
                throw;
            }
        }

        private async Task ReceiveLoop()
        {
            var buffer = new byte[8192];

            try
            {
                while (_socket?.State == WebSocketState.Open)
                {
                    byte[] messageBytes;
                    using (var stream = new MemoryStream())
                    {
                        WebSocketReceiveResult result;
                        do
                        {
                            result = await _socket.ReceiveAsync(
                                new ArraySegment<byte>(buffer),
                                CancellationToken.None);

                            if (result.MessageType == WebSocketMessageType.Close)
                            {
                                await _sendLock.WaitAsync();
                                try
                                {
                                    if (_socket.State == WebSocketState.CloseReceived)
                                    {
                                        await _socket.CloseOutputAsync(
                                            WebSocketCloseStatus.NormalClosure,
                                            string.Empty,
                                            CancellationToken.None);
                                    }
                                }
                                finally
                                {
                                    _sendLock.Release();
                                }

                                OnDisconnected?.Invoke();
                                return;
                            }

                            stream.Write(buffer, 0, result.Count);
                        }
                        while (!result.EndOfMessage);

                        messageBytes = stream.ToArray();
                    }

                    try
                    {
                        var options = MessagePackBootstrap.Options ?? MessagePackSerializerOptions.Standard;
                        var message = MessagePackSerializer.Deserialize<RealtimeEnvelope>(messageBytes, options);

                        _receiveQueue.Enqueue(message);
                    }
                    catch (MessagePackSerializationException ex)
                    {
                        Debug.LogError($"[WS] Deserialization error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[WS] Receive error: {ex.Message}");
                OnError?.Invoke(ex.Message);
                OnDisconnected?.Invoke();
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
                _cts?.Cancel();

                await _sendLock.WaitAsync();
                try
                {
                    if (_socket?.State == WebSocketState.Open)
                    {
                        await _socket.CloseAsync(
                            WebSocketCloseStatus.NormalClosure,
                            "Client closing",
                            CancellationToken.None);
                    }
                }
                finally
                {
                    _sendLock.Release();
                }
            }
            catch { }

            OnDisconnected?.Invoke();
        }
    }
}
