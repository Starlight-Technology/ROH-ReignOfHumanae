using ROH.Context.Player.Redis.Entities;
using ROH.Context.Player.Redis.Interface;

using StackExchange.Redis;

using System;
using System.Collections.Generic;
using System.Text;

namespace ROH.Context.Player.Redis.Repository;


public class PositionRepository(IPlayerRedisContext context)
: IPositionRepository
{

    public async Task SavePlayerPosition(PlayerPositionRedis position, CancellationToken cancellationToken)
    {
        var key = context.PlayerStateKey(position.PlayerId);

        await context.Database.HashSetAsync(key, new HashEntry[]
        {
            new("x", position.PositionX),
            new("y", position.PositionY),
            new("z", position.PositionZ),
            new("rotX", position.RotationX),
            new("rotY", position.RotationY),
            new("rotZ", position.RotationZ),
            new("rotW", position.RotationW),
            new("anim", (int)position.PlayerAnimationState),
            new("ts", position.Timestamp)
        });

        await context.Database.KeyExpireAsync(
            key,
            TimeSpan.FromSeconds(10)
        );

        // GEO
        await context.Database.GeoAddAsync(
            context.PlayersGeoKey,
            position.PositionX,
            position.PositionZ,
            position.PlayerId
        );
    }
}
