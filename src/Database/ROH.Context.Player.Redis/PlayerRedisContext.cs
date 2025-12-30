//-----------------------------------------------------------------------
// <copyright file="PlayerRedisContext.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ROH.Context.Player.Redis.Interface;

using StackExchange.Redis;

namespace ROH.Context.Player.Redis;

public class PlayerRedisContext : IPlayerRedisContext
{
    readonly ConnectionMultiplexer _connection;

    public PlayerRedisContext()
    {
        string connectionString =
            Environment.GetEnvironmentVariable("ROH_REDIS_PLAYER_CONNECTION_STRING") ?? "localhost:6379";

        _connection = ConnectionMultiplexer.Connect(connectionString);
    }

    public string PlayerStateKey(string playerId) => $"player:state:{playerId}";

    public IDatabase Database => _connection.GetDatabase();

    public string PlayersGeoKey => "players:geo";
}