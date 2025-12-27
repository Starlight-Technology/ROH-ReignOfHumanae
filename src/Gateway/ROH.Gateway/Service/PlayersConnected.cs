namespace ROH.Gateway.Service;

using MessagePack;

using ROH.Contracts.GRPC.Player.NearbyPlayer;
using ROH.Contracts.GRPC.Worker.PlayerSocket;
using ROH.Contracts.WebSocket;
using ROH.Contracts.WebSocket.Player;
using ROH.Gateway.WebSocketGateway;

using System.Net.Sockets;
using System.Threading.Tasks;

public class PlayersConnected(IRealtimeConnectionManager realtimeConnection) : PlayerConnectedService.PlayerConnectedServiceBase
{
    public async override Task<Contracts.GRPC.Worker.PlayerSocket.PlayersConnected> GetConnectedPlayers(Contracts.GRPC.Worker.PlayerSocket.Default request, global::Grpc.Core.ServerCallContext context)
    {

        var connectedPlayers = await realtimeConnection.GetPlayersClient();

        var response = new Google.Protobuf.Collections.RepeatedField<PlayerConnected>();

        foreach (var player in connectedPlayers)
        {
            response.Add(new PlayerConnected() { Id = player.Key });
        }

        return new Contracts.GRPC.Worker.PlayerSocket.PlayersConnected()
        {
            PlayersId = { response }
        };

    }

    public async override Task<Contracts.GRPC.Worker.PlayerSocket.Default> SendNearbyPlayers(NearbyPlayersResponse response, global::Grpc.Core.ServerCallContext context)
    {

        var connectedPlayers = await realtimeConnection.GetPlayersClient();

        if (connectedPlayers.TryGetValue(response.MainPlayer, out var socket))
        {
            var message = new NearbyPlayersMessage
            {
                Players = [.. response.Players.Select(p => new NearbyPlayerMessage
                {
                    PlayerId = p.PlayerId,
                    X = p.X,
                    Y = p.Y,
                    Z = p.Z,
                    RotX = p.RotX,
                    RotY = p.RotY,
                    RotZ = p.RotZ,
                    RotW = p.RotW,
                    ModelName = p.ModelName,
                    AnimationState = (int)p.AnimationState
                })]
            };

            await WebSocketResponse.SendAsync(
                socket,
                new RealtimeEnvelope
                {
                    Type = "NearbyPlayers",
                    Payload = MessagePackSerializer.Serialize(message)
                });
        }

        return new Contracts.GRPC.Worker.PlayerSocket.Default() { A = true };
    }
}
