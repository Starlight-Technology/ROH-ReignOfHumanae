using Grpc.Core;

using MongoDB.Bson;

using ROH.Context.Player.Mongo.Interface;
using ROH.Protos.PlayerPosition;
using ROH.Service.Exception.Interface;

namespace ROH.Service.Player.Grpc.Player;
public class SavePosition(IPositionRepository repository, IExceptionHandler handler) : PlayerService.PlayerServiceBase
{
    public override async Task<SaveResponse> SavePlayerData(PlayerRequest request, ServerCallContext context)
    {
        try
        {
            var position = new Context.Player.Mongo.Entities.PlayerPosition
            {
                Id = ObjectId.GenerateNewId(),
                PlayerId = request.PlayerId,
                PositionX = request.Position.X,
                PositionY = request.Position.Y,
                PositionZ = request.Position.Z,
                RotationX = request.Rotation.X,
                RotationY = request.Rotation.Y,
                RotationZ = request.Rotation.Z,
                RotationW = request.Rotation.W,
            };

            await repository.SavePlayerPositionAsync(position, context.CancellationToken).ConfigureAwait(false);

            return new SaveResponse { Success = true };
        }
        catch (System.Exception ex)
        {
            handler.HandleException(ex);
            return new SaveResponse { Success = false };
        }
    }
}
