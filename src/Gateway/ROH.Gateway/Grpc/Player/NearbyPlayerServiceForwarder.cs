using Grpc.Core;

using ROH.Contracts.GRPC.Player.NearbyPlayer;

namespace ROH.Gateway.Grpc.Player;

public class NearbyPlayerServiceForwarder : NearbyPlayerService.NearbyPlayerServiceBase
{
    private readonly NearbyPlayerService.NearbyPlayerServiceClient _nearbyPlayerService;

    public NearbyPlayerServiceForwarder( NearbyPlayerService.NearbyPlayerServiceClient nearbyPlayerService)
    {
        _nearbyPlayerService = nearbyPlayerService;
    }

    public override async Task<NearbyPlayersResponse> GetNearbyPlayers(NearbyPlayersRequest request, ServerCallContext context)
    {
        // Redireciona para o microserviço gRPC real
        return await _nearbyPlayerService.GetNearbyPlayersAsync(request).ConfigureAwait(true);
    }
}
