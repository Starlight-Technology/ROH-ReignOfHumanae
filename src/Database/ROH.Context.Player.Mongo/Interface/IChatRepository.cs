using ROH.Context.Player.Mongo.Entities;

namespace ROH.Context.Player.Mongo.Interface;

public interface IChatRepository
{
    Task<long> DeleteOlderThanAsync(DateTime cutoffUtc, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ChatMessageEntity>> GetRecentAsync(
        string channel,
        int limit,
        CancellationToken cancellationToken = default);

    Task InsertAsync(ChatMessageEntity message, CancellationToken cancellationToken = default);
}
