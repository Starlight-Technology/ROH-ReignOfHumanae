//-----------------------------------------------------------------------
// <copyright file="IPositionRepository.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Context.Player.Redis.Entities;

namespace ROH.Context.Player.Redis.Interface;

public interface IPositionRepository
{
    Task<IReadOnlyList<PlayerPositionRedis>> GetNearbyPlayersAsync(
        string playerId,
        double radiusMeters,
        int maxResults,
        CancellationToken cancellationToken);

    Task<PlayerPositionRedis?> GetPlayerPosition(string playerId, CancellationToken cancellationToken);

    Task SavePlayerPosition(PlayerPositionRedis position, CancellationToken cancellationToken);
}