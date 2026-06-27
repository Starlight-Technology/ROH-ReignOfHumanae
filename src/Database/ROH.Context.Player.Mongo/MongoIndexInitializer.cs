using MongoDB.Driver;

using ROH.Context.Player.Mongo.Entities;
using ROH.Context.Player.Mongo.Interface;

namespace ROH.Context.Player.Mongo;

public class MongoIndexInitializer(IPlayerMongoContext context) : IMongoIndexInitializer
{
    public async Task EnsureIndexesAsync(CancellationToken cancellationToken = default)
    {
        CreateIndexModel<PlayerPositionGeo>[] positionIndexes =
        [
            new(
                Builders<PlayerPositionGeo>.IndexKeys.Ascending(position => position.CharacterId),
                new CreateIndexOptions
                {
                    Name = "ux_character_id",
                    Sparse = true,
                    Unique = true
                }),
            new(
                Builders<PlayerPositionGeo>.IndexKeys.Geo2DSphere(position => position.Position),
                new CreateIndexOptions { Name = "ix_position_2dsphere" })
        ];

        await context.PlayerPositionGeoCollection.Indexes
            .CreateManyAsync(positionIndexes, cancellationToken)
            .ConfigureAwait(false);

        CreateIndexModel<ChatMessageEntity>[] chatIndexes =
        [
            new(
                Builders<ChatMessageEntity>.IndexKeys
                    .Ascending(message => message.Channel)
                    .Descending(message => message.CreatedAtUtc),
                new CreateIndexOptions { Name = "ix_channel_created_at" }),
            new(
                Builders<ChatMessageEntity>.IndexKeys.Descending(message => message.CreatedAtUtc),
                new CreateIndexOptions { Name = "ix_created_at" }),
            new(
                Builders<ChatMessageEntity>.IndexKeys
                    .Ascending(message => message.SenderCharacterId)
                    .Descending(message => message.CreatedAtUtc),
                new CreateIndexOptions { Name = "ix_sender_created_at" })
        ];

        await context.ChatMessagesCollection.Indexes
            .CreateManyAsync(chatIndexes, cancellationToken)
            .ConfigureAwait(false);
    }
}
