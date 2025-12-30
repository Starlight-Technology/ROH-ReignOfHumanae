//-----------------------------------------------------------------------
// <copyright file="IPlayerRedisContext.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using StackExchange.Redis;

namespace ROH.Context.Player.Redis.Interface;

public interface IPlayerRedisContext
{
    string PlayerStateKey(string playerId);

    IDatabase Database { get; }

    string PlayersGeoKey { get; }
}