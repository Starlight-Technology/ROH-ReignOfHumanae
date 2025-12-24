using Grpc.Core;

using MongoDB.Bson;

using ROH.Context.Player.Mongo.Interface;
using ROH.Context.Player.Redis.Entities;
using ROH.Contracts.GRPC.Player.PlayerPosition;
using ROH.Service.Exception.Interface;

namespace ROH.Service.Player.Grpc.Player;
public class SavePosition(IPositionRepository repository, Context.Player.Redis.Interface.IPositionRepository positionRepository,IExceptionHandler handler) : PlayerService.PlayerServiceBase
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

            var redisPosition = new PlayerPositionRedis
            {
                PlayerId = request.PlayerId,
                PositionX = request.Position.X,
                PositionY = request.Position.Y,
                PositionZ = request.Position.Z,
                RotationX = request.Rotation.X,
                RotationY = request.Rotation.Y,
                RotationZ = request.Rotation.Z,
                RotationW = request.Rotation.W,
                PlayerAnimationState = (int)request.AnimationSate,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            await positionRepository.SavePlayerPosition(redisPosition, context.CancellationToken).ConfigureAwait(false);

            return new SaveResponse { Success = true };
        }
        catch (System.Exception ex)
        {
            handler.HandleException(ex);
            return new SaveResponse { Success = false };
        }
    }
}
