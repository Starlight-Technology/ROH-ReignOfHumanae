using Grpc.Core;

using ROH.Context.Player.Mongo.Interface;
using ROH.Contracts.GRPC.Player.NearbyPlayer;
using ROH.Service.Exception.Interface;

namespace ROH.Service.Player.Grpc.Player;

public class NearbyPlayers(Context.Player.Redis.Interface.IPositionRepository positionRepositoryRedis, IExceptionHandler exceptionHandler) : NearbyPlayerService.NearbyPlayerServiceBase
{
    public override async Task<NearbyPlayersResponse> GetNearbyPlayers(
        NearbyPlayersRequest request,
        ServerCallContext context)
    {
        try
        {
            const int maxPlayers = 32;

            var result = await positionRepositoryRedis.GetNearbyPlayersAsync(request.PlayerId, request.Radius, maxPlayers, context.CancellationToken).ConfigureAwait(true);

            return new NearbyPlayersResponse
            {
                Players = { 
                    result.Select(p => new PlayerInfo
                    {
                        PlayerId = p.PlayerId,
                        X = p.PositionX,
                        Y = p.PositionY,
                        Z = p.PositionZ,
                        RotX = p.RotationX,
                        RotY = p.RotationY,
                        RotZ = p.RotationZ,
                        RotW = p.RotationW
                    }) 
                }
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
