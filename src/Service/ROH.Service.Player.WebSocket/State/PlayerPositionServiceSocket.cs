using Grpc.Net.Client;

using MessagePack;

using ROH.Contracts.GRPC.Player.PlayerPosition;
using ROH.Contracts.WebSocket.Player;
using ROH.Service.Player.WebSocket.Interface;
using ROH.Utils.ApiConfiguration;

using System.Collections.Concurrent;

using static ROH.Utils.ApiConfiguration.ApiConfigReader;

namespace ROH.Service.Player.WebSocket.State;

public class PlayerPositionServiceSocket()
: IPlayerPositionServiceSocket
{
    private Contracts.GRPC.Player.PlayerPosition.PlayerService.PlayerServiceClient _savePlayerPositionApi;
    private readonly ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _playerConnections = new();

    public async Task<ConcurrentDictionary<string, System.Net.WebSockets.WebSocket>> GetPlayersClient() => _playerConnections;

    public Task NewPlayerClient(string guid, System.Net.WebSockets.WebSocket socket)
    {
        _playerConnections[guid] = socket;

        return Task.CompletedTask;
    }

    public async Task<SaveResponse> HandlePlayerPosition(byte[] payload, System.Net.WebSockets.WebSocket socket)
    {
        var msg = MessagePackSerializer.Deserialize<PlayerPositionMessage>(payload);

        await NewPlayerClient(msg.PlayerId, socket);

        ApiConfigReader _apiConfig = new();
        Dictionary<ApiUrl, Uri> _apiUrl = _apiConfig.GetApiUrl();
        GrpcChannel channel = GrpcChannel.ForAddress(
        _apiUrl.GetValueOrDefault(ApiUrl.PlayerState) ?? new Uri(string.Empty),
        new GrpcChannelOptions
        {
            HttpHandler =
                new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                }
        });

        _savePlayerPositionApi = new(channel);

        var response = await _savePlayerPositionApi.SavePlayerDataAsync(
            new PlayerRequest
            {
                PlayerId = msg.PlayerId,
                Position = new Position
                {
                    X = msg.X,
                    Y = msg.Y,
                    Z = msg.Z
                },
                Rotation = new Rotation
                {
                    X = msg.RotX,
                    Y = msg.RotY,
                    Z = msg.RotZ,
                    W = msg.RotW
                },
                AnimationSate = (uint)msg.AnimationState
            }
        );

        return response;
    }
}