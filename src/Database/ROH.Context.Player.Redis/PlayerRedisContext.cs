using ROH.Context.Player.Redis.Interface;

using StackExchange.Redis;

namespace ROH.Context.Player.Redis;

public class PlayerRedisContext : IPlayerRedisContext
{
    private readonly ConnectionMultiplexer _connection;

    public PlayerRedisContext()
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ROH_REDIS_PLAYER_CONNECTION_STRING")
            ?? "localhost:6379";

        _connection = ConnectionMultiplexer.Connect(connectionString);
    }

    public IDatabase Database => _connection.GetDatabase();

    public string PlayersGeoKey => "players:geo";

    public string PlayerStateKey(string playerId)
        => $"player:state:{playerId}";
}
