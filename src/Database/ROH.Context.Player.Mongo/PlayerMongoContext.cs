namespace ROH.Context.Player.Mongo;

using MongoDB.Driver;

using ROH.Context.Player.Mongo.Entities;

public class PlayerMongoContext
{
    private readonly IMongoDatabase _database;

    public PlayerMongoContext()
    {
        var connectionString = Environment.GetEnvironmentVariable("PLAYER_MONGO_CONNECTION_STRING");
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("GameDatabase");
    }

    public IMongoCollection<PlayerPosition> PlayerPositionCollection => _database.GetCollection<PlayerPosition>("PlayerPositionCollection");
}
