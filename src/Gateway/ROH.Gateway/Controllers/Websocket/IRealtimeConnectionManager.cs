using ROH.Contracts.WebSocket;

using System.Collections.Concurrent;

namespace ROH.Service.WebSocket;

public interface IRealtimeConnectionManager
{
    Task HandleClientAsync(HttpContext ctx, System.Net.WebSockets.WebSocket socket);
}
