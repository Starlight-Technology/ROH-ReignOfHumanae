using MongoDB.Driver;

using ROH.Context.Player.Mongo.Entities;
using ROH.Context.Player.Mongo.Interface;

using System.Numerics;

namespace ROH.Context.Player.Mongo.Repository;

public class PositionRepository(IPlayerMongoContext context) : IPositionRepository
{
    private readonly IMongoCollection<PlayerPosition> _collection = context.PlayerPositionCollection;

    public async Task SavePlayerPositionAsync(PlayerPosition data, CancellationToken cancellationToken = default)
    {
        await _collection.ReplaceOneAsync(
        filter: Builders<PlayerPosition>.Filter.Eq(p => p.PlayerId, data.PlayerId),
        replacement: data,
        options: new ReplaceOptions { IsUpsert = true },
        cancellationToken: cancellationToken).ConfigureAwait(false);

    }

    public async Task<List<PlayerPosition>> GetAllPlayersAsync(CancellationToken cancellationToken = default)
    {
        return await _collection.Find(_ => true).ToListAsync(cancellationToken).ConfigureAwait(true);
    }
}
