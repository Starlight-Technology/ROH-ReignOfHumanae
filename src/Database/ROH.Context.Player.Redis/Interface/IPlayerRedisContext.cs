using StackExchange.Redis;

namespace ROH.Context.Player.Redis.Interface;

public interface IPlayerRedisContext
{
    IDatabase Database { get; }

    string PlayerStateKey(string playerId);
    string PlayersGeoKey { get; }
}
