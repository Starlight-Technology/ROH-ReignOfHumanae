//-----------------------------------------------------------------------
// <copyright file="PlayerMongoContext.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using MongoDB.Driver;

using ROH.Context.Player.Mongo.Entities;
using ROH.Context.Player.Mongo.Interface;

namespace ROH.Context.Player.Mongo;

public class PlayerMongoContext : IPlayerMongoContext
{
    readonly IMongoDatabase _database;

    public PlayerMongoContext()
    {
        string? connectionString = Environment.GetEnvironmentVariable("ROH_MONGO_PLAYER_CONNECTION_STRING");
        MongoClient client = new MongoClient(connectionString);
        _database = client.GetDatabase("ROHPlayerPosition");
    }

    public IMongoCollection<PlayerPosition> PlayerPositionCollection => _database.GetCollection<PlayerPosition>(
        "PlayerPositionCollection");

    public IMongoCollection<PlayerPositionGeo> PlayerPositionGeoCollection => _database.GetCollection<PlayerPositionGeo>(
        "player_positions");
}