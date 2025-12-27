using ROH.Contracts.GRPC.Player.NearbyPlayer;
using ROH.Contracts.GRPC.Player.PlayerPosition;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ROH.Service.Player.Grpc.Persistence;

public class PlayersPersistenceService : IPlayersPersistenceService
{
    private readonly ConcurrentDictionary<string, PlayerState> players = new();

    public Task SavePlayerPosition(PlayerRequest player, CancellationToken token)
    {
        players.AddOrUpdate(
            player.PlayerId,
            _ => CreateState(player),
            (_, __) => CreateState(player)
        );

        return Task.CompletedTask;
    }

    public Task<ICollection<PlayerInfo>> GetNearbyPlayerAsync(
        string playerId,
        float radius,
        int maxPlayers,
        CancellationToken cancellationToken)
    {
        var result = new List<PlayerInfo>(maxPlayers);
        while (!cancellationToken.IsCancellationRequested)
        {
            if (!players.TryGetValue(playerId, out var mainPlayer))
                return Task.FromResult<ICollection<PlayerInfo>>(result);

            float radiusSquared = radius * radius;

            foreach (var kv in players)
            {
                if(kv.Value.Timestamp < DateTime.UtcNow.AddSeconds(-60))
                {
                    players.TryRemove(kv);
                    continue;
                }

                var p = kv.Value;

                if (p.PlayerId == playerId)
                    continue;

                float dx = p.PositionX - mainPlayer.PositionX;
                float dy = p.PositionY - mainPlayer.PositionY;
                float dz = p.PositionZ - mainPlayer.PositionZ;

                float distanceSquared = dx * dx + dy * dy + dz * dz;

                if (distanceSquared > radiusSquared)
                    continue;

                result.Add(new PlayerInfo
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

    private static PlayerState CreateState(PlayerRequest player)
    {
        return new PlayerState
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
    }

}

public class PlayerState
{
    public string PlayerId { get; set; } = "";

    public float PositionX { get; set; }
    public float PositionY { get; set; }
    public float PositionZ { get; set; }

    public float RotationX { get; set; }
    public float RotationY { get; set; }
    public float RotationZ { get; set; }
    public float RotationW { get; set; }

    public int AnimationState { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}