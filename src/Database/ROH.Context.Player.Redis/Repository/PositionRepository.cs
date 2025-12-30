using ROH.Context.Player.Redis.Entities;
using ROH.Context.Player.Redis.Interface;

using StackExchange.Redis;

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
            TimeSpan.FromSeconds(60)
        );
    }

    public async Task<PlayerPositionRedis?> GetPlayerPosition(
    string playerId,
    CancellationToken cancellationToken)
    {
        var key = context.PlayerStateKey(playerId);

        if (!await context.Database.KeyExistsAsync(key))
            return null;

        var entries = await context.Database.HashGetAllAsync(key);

        if (entries.Length == 0)
            return null;

        var dict = entries.ToDictionary(
            x => x.Name.ToString(),
            x => x.Value
        );

        return new PlayerPositionRedis
        {
            PlayerId = playerId,

            PositionX = (float)dict["x"],
            PositionY = (float)dict["y"],
            PositionZ = (float)dict["z"],

            RotationX = (float)dict["rotX"],
            RotationY = (float)dict["rotY"],
            RotationZ = (float)dict["rotZ"],
            RotationW = (float)dict["rotW"],

            PlayerAnimationState = (int)dict["anim"],

            Timestamp = (long)dict["ts"]
        };
    }

    public async Task<IReadOnlyList<PlayerPositionRedis>> GetNearbyPlayersAsync(
    string playerId,
    double radiusMeters,
    int maxResults,
    CancellationToken cancellationToken)
    {
        var nearby = await context.Database.GeoRadiusAsync(
            context.PlayersGeoKey,
            playerId,
            radiusMeters,
            GeoUnit.Meters,
            count: maxResults,
            order: Order.Ascending
        );

        if (nearby.Length == 0)
            return Array.Empty<PlayerPositionRedis>();

        var batch = context.Database.CreateBatch();
        var tasks = new Dictionary<string, Task<HashEntry[]>>();

        foreach (var geo in nearby)
        {
            var id = geo.Member.ToString();
            if (id == playerId)
                continue;

            var key = context.PlayerStateKey(id);
            tasks[id] = batch.HashGetAllAsync(key);
        }

        batch.Execute();

        await Task.WhenAll(tasks.Values);

        var result = new List<PlayerPositionRedis>();

        foreach (var (id, task) in tasks)
        {
            var entries = task.Result;
            if (entries.Length == 0)
                continue;

            var dict = entries.ToDictionary(
                x => x.Name.ToString(),
                x => x.Value
            );

            result.Add(new PlayerPositionRedis
            {
                PlayerId = id,
                PositionX = (float)dict["x"],
                PositionY = (float)dict["y"],
                PositionZ = (float)dict["z"],

                RotationX = (float)dict["rotX"],
                RotationY = (float)dict["rotY"],
                RotationZ = (float)dict["rotZ"],
                RotationW = (float)dict["rotW"],

                PlayerAnimationState = (int)dict["anim"],
                Timestamp = (long)dict["ts"]
            });
        }

        return result;
    }
}