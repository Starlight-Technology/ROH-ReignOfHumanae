using MongoDB.Driver;

using ROH.Context.Player.Mongo.Entities;
using ROH.Context.Player.Mongo.Interface;

namespace ROH.Context.Player.Mongo;

public class PlayerMongoContext : IPlayerMongoContext
{
    private readonly IMongoDatabase _database;

    public PlayerMongoContext()
    {
        var connectionString = Environment.GetEnvironmentVariable("ROH_MONGO_PLAYER_CONNECTION_STRING");
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("ROHPlayerPosition");
    }

    public IMongoCollection<PlayerPosition> PlayerPositionCollection => _database.GetCollection<PlayerPosition>("PlayerPositionCollection");
    public IMongoCollection<PlayerPositionGeo> PlayerPositionGeoCollection => _database.GetCollection<PlayerPositionGeo>("player_positions");

}
