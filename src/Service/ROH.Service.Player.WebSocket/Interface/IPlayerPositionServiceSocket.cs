using ROH.Contracts.GRPC.Player.PlayerPosition;

using System.Collections.Concurrent;

namespace ROH.Service.Player.WebSocket.Interface;

public interface IPlayerPositionServiceSocket
{
    Task<SaveResponse> HandlePlayerPosition(byte[] payload, System.Net.WebSockets.WebSocket socket);
    Task<ConcurrentDictionary<string, System.Net.WebSockets.WebSocket>> GetPlayersClient();
    Task NewPlayerClient(string guid, System.Net.WebSockets.WebSocket socket);
}
