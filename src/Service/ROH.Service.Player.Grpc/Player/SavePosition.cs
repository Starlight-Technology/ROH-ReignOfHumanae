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
            AccountId = request.AccountId,
            CharacterId = request.PlayerId,
            Id = ObjectId.GenerateNewId(),
            PlayerId = request.PlayerId,
            PositionX = request.Position.X,
            PositionY = request.Position.Y,
            PositionZ = request.Position.Z,
            RotationX = request.Rotation.X,
            RotationY = request.Rotation.Y,
            RotationZ = request.Rotation.Z,
            RotationW = request.Rotation.W,
            Timestamp = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            WorldId = request.WorldId
        };

        await repository.SavePlayerPositionAsync(position, context.CancellationToken).ConfigureAwait(true);
    }

    public override async Task<SaveResponse> RemovePlayerData(RemovePlayerRequest request, ServerCallContext context)
    {
        await playersPersistenceService.RemovePlayer(request.PlayerId).ConfigureAwait(false);
        return new SaveResponse { Success = true };
    }

    public override async Task<SaveResponse> SavePlayerData(PlayerRequest request, ServerCallContext context)
    {
        try
        {
            PlayerState? lastPlayerPosition = await playersPersistenceService.GetPlayerState(request.PlayerId);
            DateTime nowUtc = DateTime.UtcNow;
            Vector3 currentPositionVector = new(request.Position.X, request.Position.Y, request.Position.Z);
            Vector3 lastPositionVector = lastPlayerPosition is null
                ? currentPositionVector
                : new Vector3(
                    lastPlayerPosition.PositionX,
                    lastPlayerPosition.PositionY,
                    lastPlayerPosition.PositionZ);

            PlayerPositionInput playerPositionInput = new(
                new Guid(request.PlayerId),
                lastPositionVector,
                currentPositionVector,
                new Vector4(request.Rotation.X, request.Rotation.Y, request.Rotation.Z, request.Rotation.W),
                lastPlayerPosition?.Timestamp ?? nowUtc.AddMilliseconds(-1),
                nowUtc);

            PlayerPositionValidationResult isPositionValid = positionService.Validate(playerPositionInput);

            if (isPositionValid != PlayerPositionValidationResult.Valid)
                return new SaveResponse { PositionValid = (uint)isPositionValid, Success = false };

            await playersPersistenceService.SavePlayerPosition(request, context.CancellationToken).ConfigureAwait(true);

            if (request.PersistPosition)
                await SavePositionPersistence(request, context).ConfigureAwait(true);

            return new SaveResponse { PositionValid = (uint)PlayerPositionValidationResult.Valid, Success = true };
        }
        catch (System.Exception ex)
        {
            handler.HandleException(ex);
            return new SaveResponse { Success = false };
        }
    }
}
