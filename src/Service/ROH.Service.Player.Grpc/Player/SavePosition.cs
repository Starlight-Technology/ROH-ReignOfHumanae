//-----------------------------------------------------------------------
// <copyright file="SavePosition.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Grpc.Core;

using MongoDB.Bson;

using ROH.Context.Player.Mongo.Interface;
using ROH.Contracts.GRPC.Player.PlayerPosition;
using ROH.Service.Exception.Interface;
using ROH.Service.Player.Grpc.Interface;
using ROH.Service.Player.Grpc.Persistence;
using ROH.StandardModels.Character.Position;

using System.Numerics;

namespace ROH.Service.Player.Grpc.Player;

public class SavePosition(
    IPositionRepository repository,
    IPlayersPersistenceService playersPersistenceService,
    IExceptionHandler handler,
    IPlayerValidPositionService positionService) : PlayerService.PlayerServiceBase, ISavePosition
{
    async Task SavePositionPersistence(PlayerRequest request, ServerCallContext context)
    {
        Context.Player.Mongo.Entities.PlayerPosition position = new Context.Player.Mongo.Entities.PlayerPosition
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

        await repository.SavePlayerPositionAsync(position, context.CancellationToken).ConfigureAwait(true);
        await playersPersistenceService.SavePlayerPosition(request, context.CancellationToken).ConfigureAwait(true);
    }

    public override async Task<SaveResponse> SavePlayerData(PlayerRequest request, ServerCallContext context)
    {
        try
        {
            PlayerState? lastPlayerPosition = await playersPersistenceService.GetPlayerState(request.PlayerId);

            if (lastPlayerPosition is not null)
            {
                Vector3 lastPositionVector = new Vector3(
                    lastPlayerPosition.PositionX,
                    lastPlayerPosition.PositionY,
                    lastPlayerPosition.PositionZ);

                Vector3 currentPositionVector = new Vector3(request.Position.X, request.Position.Y, request.Position.Z);

                PlayerPositionInput playerPositionInput = new PlayerPositionInput(
                    new Guid(request.PlayerId),
                    lastPositionVector,
                    currentPositionVector,
                    lastPlayerPosition.Timestamp,
                    DateTime.UtcNow);

                PlayerPositionValidationResult isPositionValid = positionService.Validate(playerPositionInput);

                if (isPositionValid != PlayerPositionValidationResult.Valid)
                    return new SaveResponse { PositionValid = (uint)isPositionValid, Success = false };
            }

            await SavePositionPersistence(request, context);

            return new SaveResponse { PositionValid = (uint)PlayerPositionValidationResult.Valid, Success = true };
        }
        catch (System.Exception ex)
        {
            handler.HandleException(ex);
            return new SaveResponse { Success = false };
        }
    }
}