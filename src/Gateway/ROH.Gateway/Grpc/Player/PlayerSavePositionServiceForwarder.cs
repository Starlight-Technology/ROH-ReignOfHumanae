using Grpc.Core;

using ROH.Protos.NearbyPlayer;
using ROH.Protos.PlayerPosition;

namespace ROH.Gateway.Grpc.Player;

public class PlayerSavePositionServiceForwarder : PlayerService.PlayerServiceBase
{
    private readonly PlayerService.PlayerServiceClient _playerServiceClient;

    public PlayerSavePositionServiceForwarder(PlayerService.PlayerServiceClient playerService)
    {
        _playerServiceClient = playerService;
    }

    public override async Task<SaveResponse> SavePlayerData(PlayerRequest request, ServerCallContext context)
    {
        // Redireciona para o microserviço gRPC real
        return await _playerServiceClient.SavePlayerDataAsync(request).ConfigureAwait(true);
    }

}

