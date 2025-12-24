using MessagePack;

using ROH.Contracts.WebSocket.Player;

using System.Net.WebSockets;

namespace ROH.Gateway.WebSocketGateway;

public class WebSocketResponse
{
    public static async Task SendAsync(WebSocket socket, RealtimeEnvelope env)
    {
        var data = MessagePackSerializer.Serialize(env);

        await socket.SendAsync(
            data,
            WebSocketMessageType.Binary,
            true,
            CancellationToken.None);
    }
}
