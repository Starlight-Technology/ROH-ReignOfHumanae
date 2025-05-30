using Grpc.Core;

using ROH.Protos.Player;

namespace ROH.Gateway.Grpc.Player;

public class PlayerServiceForwarder : PlayerService.PlayerServiceBase
{
    private readonly PlayerService.PlayerServiceClient _realService;

    public PlayerServiceForwarder(PlayerService.PlayerServiceClient realService)
    {
        _realService = realService;
    }

    public override async Task<SaveResponse> SavePlayerData(PlayerRequest request, ServerCallContext context)
    {
        // Redireciona para o microserviço gRPC real
        return await _realService.SavePlayerDataAsync(request).ConfigureAwait(true);
    }
}

