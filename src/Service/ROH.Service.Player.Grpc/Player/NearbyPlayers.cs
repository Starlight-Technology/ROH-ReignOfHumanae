using Grpc.Core;

using ROH.Contracts.GRPC.Player.NearbyPlayer;
using ROH.Service.Exception.Interface;
using ROH.Service.Player.Grpc.Persistence;

namespace ROH.Service.Player.Grpc.Player;

public class NearbyPlayers(Context.Player.Redis.Interface.IPositionRepository positionRepositoryRedis, IPlayersPersistenceService playersPersistenceService, IExceptionHandler exceptionHandler) : NearbyPlayerService.NearbyPlayerServiceBase
{
    public override async Task<NearbyPlayersResponse> GetNearbyPlayers(
        NearbyPlayersRequest request,
        ServerCallContext context)
    {
        try
        {
            const int maxPlayers = 32;

            var result = await playersPersistenceService.GetNearbyPlayerAsync(request.PlayerId, request.Radius, maxPlayers, context.CancellationToken).ConfigureAwait(true);

            return new NearbyPlayersResponse
            {
                Players = {result},
                MainPlayer = request.PlayerId,
            };
        }
        catch (System.Exception ex)
        {
            exceptionHandler.HandleException(ex);
            return new NearbyPlayersResponse
            {
                Players = { }
            };
        }
    }
}
