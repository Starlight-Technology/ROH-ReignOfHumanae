using Grpc.Core;

using Microsoft.Extensions.Logging;

using ROH.Context.Player.Mongo.Interface;
using ROH.Protos.Player;
using ROH.Service.Exception.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Service.Player.Characters;
public class PositionService : PlayerService.PlayerServiceBase
{
    private readonly IPositionRepository _repository;
    private readonly IExceptionHandler _exceptionHandler;

    public PositionService(IPositionRepository repository, IExceptionHandler handler)
    {
        _repository = repository;
        _exceptionHandler = handler;
    }

    public override async Task<SaveResponse> SavePlayerData(PlayerRequest request, ServerCallContext context)
    {
        try
        {
            var position = new Context.Player.Mongo.Entities.PlayerPosition
            {
                PlayerId = request.PlayerId,
                PositionX = request.Position.X,
                PositionY = request.Position.Y,
                PositionZ = request.Position.Z,
                RotationX = request.Rotation.X,
                RotationY = request.Rotation.Y,
                RotationZ = request.Rotation.Z,
                RotationW = request.Rotation.W
            };

            await _repository.SavePlayerPositionAsync(position, context.CancellationToken).ConfigureAwait(false);

            return new SaveResponse { Success = true };
        }
        catch (System.Exception ex)
        {
            _exceptionHandler.HandleException(ex);
            return new SaveResponse { Success = false };
        }
    }
}
