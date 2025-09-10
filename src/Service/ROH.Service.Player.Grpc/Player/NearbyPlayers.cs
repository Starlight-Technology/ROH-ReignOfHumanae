using ROH.Context.Player.Mongo.Interface;
using ROH.Protos.NearbyPlayer;
using Grpc.Core;
using ROH.Service.Exception.Interface;

namespace ROH.Service.Player.Grpc.Player;
public class NearbyPlayers (IPositionRepository positionRepository, IExceptionHandler exceptionHandler) : NearbyPlayerService.NearbyPlayerServiceBase
{
    public override async Task<ROH.Protos.NearbyPlayer.NearbyPlayersResponse> GetNearbyPlayers(
        NearbyPlayersRequest request,
        ServerCallContext context)
    {
        try
        {
            var vectorPosition = new System.Numerics.Vector3(request.X, request.Y, request.Z);
            var result = await positionRepository.GetPlayersNearbyAsync(request.PlayerId, vectorPosition, request.Radius).ConfigureAwait(true);

            return new NearbyPlayersResponse
            {
                Players = { result.Select(p => new PlayerInfo
            {
                PlayerId = p.PlayerId,
                X = p.PositionX,
                Y = p.PositionY,
                Z = p.PositionZ,
                RotX = p.RotationX,
                RotY = p.RotationY,
                RotZ = p.RotationZ,
                RotW = p.RotationW
            }) }
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
