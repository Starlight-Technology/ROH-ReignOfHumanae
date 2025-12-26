using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace ROH.Gateway.WebSocketGateway;

public interface IRealtimeConnectionManager
{
    Task<ConcurrentDictionary<string, WebSocket>> GetPlayersClient();
    Task HandleClientAsync(HttpContext ctx, WebSocket socket);
}
