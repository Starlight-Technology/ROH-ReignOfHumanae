using MongoDB.Driver;

using ROH.Context.Player.Mongo.Entities;
using ROH.Context.Player.Mongo.Interface;

namespace ROH.Context.Player.Mongo.Repository;

public class ChatRepository(IPlayerMongoContext context) : IChatRepository
{
    private readonly IMongoCollection<ChatMessageEntity> _collection = context.ChatMessagesCollection;

    public async Task<long> DeleteOlderThanAsync(
        DateTime cutoffUtc,
        CancellationToken cancellationToken = default)
    {
        DeleteResult result = await _collection
            .DeleteManyAsync(
                Builders<ChatMessageEntity>.Filter.Lt(message => message.CreatedAtUtc, cutoffUtc),
                cancellationToken)
            .ConfigureAwait(false);

        return result.DeletedCount;
    }

    public async Task<IReadOnlyList<ChatMessageEntity>> GetRecentAsync(
        string channel,
        int limit,
        CancellationToken cancellationToken = default)
    {
        List<ChatMessageEntity> messages = await _collection
            .Find(Builders<ChatMessageEntity>.Filter.Eq(message => message.Channel, channel))
            .SortByDescending(message => message.CreatedAtUtc)
            .Limit(limit)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        messages.Reverse();
        return messages;
    }

    public Task InsertAsync(ChatMessageEntity message, CancellationToken cancellationToken = default)
    {
        if (message.Id == MongoDB.Bson.ObjectId.Empty)
            message.Id = MongoDB.Bson.ObjectId.GenerateNewId();

        return _collection.InsertOneAsync(message, cancellationToken: cancellationToken);
    }
}
