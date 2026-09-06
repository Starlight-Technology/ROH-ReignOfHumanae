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
    private readonly IMongoDatabase _database;

    public PlayerMongoContext()
    {
        string connectionString =
            Environment.GetEnvironmentVariable("ROH_MONGO_PLAYER_CONNECTION_STRING") ?? "mongodb://localhost:27017";
        MongoClient client = new(connectionString);
        _database = client.GetDatabase("ROHPlayerPosition");
    }

    public IMongoCollection<ChatMessageEntity> ChatMessagesCollection => _database.GetCollection<ChatMessageEntity>(
        "chat_messages");

    public IMongoCollection<PlayerPosition> PlayerPositionCollection => _database.GetCollection<PlayerPosition>(
        "PlayerPositionCollection");

    public IMongoCollection<PlayerPositionGeo> PlayerPositionGeoCollection => _database.GetCollection<PlayerPositionGeo>(
        "player_positions");
}
