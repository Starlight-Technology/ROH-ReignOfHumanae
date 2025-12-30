using MessagePack;

using ROH.Contracts.WebSocket;

using System.Net.WebSockets;

namespace ROH.Service.WebSocket;

public static class WebSocketService
{
    public static async Task SendAsync(this System.Net.WebSockets.WebSocket socket, RealtimeEnvelope env)
    {
        if (socket.State != WebSocketState.Open)
            return;

        var data = MessagePackSerializer.Serialize(env);

        await socket.SendAsync(
            data,
            WebSocketMessageType.Binary,
            true,
            CancellationToken.None);
    }
}