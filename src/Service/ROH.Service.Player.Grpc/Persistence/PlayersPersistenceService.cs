//-----------------------------------------------------------------------
// <copyright file="PlayersPersistenceService.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Contracts.GRPC.Player.NearbyPlayer;
using ROH.Contracts.GRPC.Player.PlayerPosition;
using ROH.StandardModels.Character.Position;

using System.Collections.Concurrent;

namespace ROH.Service.Player.Grpc.Persistence;

public class PlayersPersistenceService : IPlayersPersistenceService
{
    readonly ConcurrentDictionary<string, PlayerState> players = new();

    static PlayerState CreateState(PlayerRequest player) => new PlayerState
    {
        PlayerId = player.PlayerId,

        PositionX = player.Position.X,
        PositionY = player.Position.Y,
        PositionZ = player.Position.Z,

        RotationX = player.Rotation.X,
        RotationY = player.Rotation.Y,
        RotationZ = player.Rotation.Z,
        RotationW = player.Rotation.W,

        AnimationState = (int)player.AnimationSate,
        Timestamp = DateTime.UtcNow
    };

    public Task<ICollection<PlayerInfo>> GetNearbyPlayerAsync(
        string playerId,
        float radius,
        int maxPlayers,
        CancellationToken cancellationToken)
    {
        List<PlayerInfo> result = new List<PlayerInfo>(maxPlayers);
        while (!cancellationToken.IsCancellationRequested)
        {
            if (!players.TryGetValue(playerId, out PlayerState? mainPlayer))
                return Task.FromResult<ICollection<PlayerInfo>>(result);

            float radiusSquared = radius * radius;

            foreach (KeyValuePair<string, PlayerState> kv in players)
            {
                if (kv.Value.Timestamp < DateTime.UtcNow.AddSeconds(-60))
                {
                    players.TryRemove(kv);
                    continue;
                }

                PlayerState p = kv.Value;

                if (p.PlayerId == playerId)
                    continue;

                float dx = p.PositionX - mainPlayer.PositionX;
                float dy = p.PositionY - mainPlayer.PositionY;
                float dz = p.PositionZ - mainPlayer.PositionZ;

                float distanceSquared = (dx * dx) + (dy * dy) + (dz * dz);

                if (distanceSquared > radiusSquared)
                    continue;

                result.Add(
                    new PlayerInfo
                    {
                        PlayerId = p.PlayerId,

                        X = p.PositionX,
                        Y = p.PositionY,
                        Z = p.PositionZ,

                        RotX = p.RotationX,
                        RotY = p.RotationY,
                        RotZ = p.RotationZ,
                        RotW = p.RotationW,

                        AnimationState = (uint)p.AnimationState,
                    });

                if (result.Count >= maxPlayers)
                    break;
            }
            break;
        }
        return Task.FromResult<ICollection<PlayerInfo>>(result);
    }

    public async Task<PlayerState?> GetPlayerState(string guid)
    {
        if (players.TryGetValue(guid, out PlayerState? player))
        {
            return player;
        }
        await Task.CompletedTask;
        return null;
    }

    public Task SavePlayerPosition(PlayerRequest player, CancellationToken token)
    {
        players.AddOrUpdate(player.PlayerId, _ => CreateState(player), (_, __) => CreateState(player));

        return Task.CompletedTask;
    }
}