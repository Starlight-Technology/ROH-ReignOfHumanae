using MongoDB.Bson;
using MongoDB.Driver;

using ROH.Context.Player.Mongo.Entities;
using ROH.Context.Player.Mongo.Interface;
using ROH.Context.Player.Mongo.Mappers;

using System.Numerics;

namespace ROH.Context.Player.Mongo.Repository;

public class PositionRepository : IPositionRepository
{
    private readonly IMongoCollection<PlayerPositionGeo> _collection;

    public PositionRepository(IPlayerMongoContext context)
    {
        _collection = context.PlayerPositionGeoCollection;
    }

    public async Task SavePlayerPositionAsync(
        PlayerPosition data,
        CancellationToken cancellationToken = default)
    {
        var geo = data.ToGeo();

        var filter = Builders<PlayerPositionGeo>.Filter
            .Eq(p => p.PlayerId, geo.PlayerId);

        var update = Builders<PlayerPositionGeo>.Update
            .Set(p => p.Position, geo.Position)
            .Set(p => p.PositionY, geo.PositionY)
            .Set(p => p.RotationX, geo.RotationX)
            .Set(p => p.RotationY, geo.RotationY)
            .Set(p => p.RotationZ, geo.RotationZ)
            .Set(p => p.RotationW, geo.RotationW)
            .Set(p => p.Timestamp, geo.Timestamp)
            // Só será aplicado se o documento NÃO existir
            .SetOnInsert(p => p.PlayerId, geo.PlayerId);

        await _collection.UpdateOneAsync(
            filter,
            update,
            new UpdateOptions { IsUpsert = true },
            cancellationToken
        ).ConfigureAwait(false);
    }

    public async Task<List<PlayerPosition>> GetAllPlayersAsync(
        CancellationToken cancellationToken = default)
    {
        var all = await _collection
            .Find(_ => true)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return all.Select(p => p.ToLegacy()).ToList();
    }

    public async Task<List<PlayerPosition>> GetPlayersNearbyAsync(
        string playerId,
        Vector3 position,
        double radiusMeters = 1000000.0,
        float heightTolerance = 50f,
        CancellationToken cancellationToken = default)
    {
        var (lng, lat) = WorldProjection.Project(position.X, position.Z);

        var pipeline = new[]
        {
            new BsonDocument("$geoNear",
                new BsonDocument
                {
                    {
                        "near", new BsonDocument
                        {
                            { "type", "Point" },
                            { "coordinates", new BsonArray
                                {
                                    lng,
                                    lat
                                }
                            }
                        }
                    },
                    { "distanceField", "distance" },
                    { "maxDistance", 100000 },
                    { "spherical", true },
                    {
                        "query", new BsonDocument("PlayerId",
                            new BsonDocument("$ne", playerId))
                    }
                }
            ),

            new BsonDocument("$match",
                new BsonDocument("PositionY",
                    new BsonDocument
                    {
                        { "$gte", position.Y - 50 },
                        { "$lte", position.Y + 50 }
                    }
                )
            )
        };

        var geoResults = await _collection
            .AggregateAsync<PlayerPositionGeo>(pipeline, null, cancellationToken)
            .Result
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return [.. geoResults.Select(p => p.ToLegacy())];
    }
}