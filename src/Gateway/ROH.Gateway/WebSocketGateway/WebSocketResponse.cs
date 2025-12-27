using MessagePack;

using ROH.Contracts.WebSocket;

using System.Net.WebSockets;

namespace ROH.Gateway.WebSocketGateway;

public static class WebSocketResponse
{
    public static async Task SendAsync(WebSocket socket, RealtimeEnvelope env)
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
