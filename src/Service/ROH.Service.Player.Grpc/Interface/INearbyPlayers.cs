using Grpc.Core;

using ROH.Contracts.GRPC.Player.NearbyPlayer;

namespace ROH.Service.Player.Grpc.Interface;

public interface INearbyPlayers
{
    Task<NearbyPlayersResponse> GetNearbyPlayers(NearbyPlayersRequest request, ServerCallContext context);
}