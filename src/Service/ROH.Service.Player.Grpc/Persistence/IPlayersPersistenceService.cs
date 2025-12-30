//-----------------------------------------------------------------------
// <copyright file="IPlayersPersistenceService.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Contracts.GRPC.Player.NearbyPlayer;
using ROH.Contracts.GRPC.Player.PlayerPosition;
using ROH.StandardModels.Character.Position;

namespace ROH.Service.Player.Grpc.Persistence;

public interface IPlayersPersistenceService
{
    Task<ICollection<PlayerInfo>> GetNearbyPlayerAsync(
        string playerId,
        float radius,
        int maxPlayers,
        CancellationToken cancellationToken);

    Task<PlayerState?> GetPlayerState(string guid);

    Task SavePlayerPosition(PlayerRequest player, CancellationToken token);
}