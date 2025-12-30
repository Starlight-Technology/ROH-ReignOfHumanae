using ROH.Context.Player.Redis.Entities;

namespace ROH.Context.Player.Redis.Interface;

public interface IPositionRepository
{
    Task SavePlayerPosition(PlayerPositionRedis position,
                            CancellationToken cancellationToken);

    Task<PlayerPositionRedis?> GetPlayerPosition(string playerId,
                                                 CancellationToken cancellationToken);

    Task<IReadOnlyList<PlayerPositionRedis>> GetNearbyPlayersAsync(string playerId,
                                                                   double radiusMeters,
                                                                   int maxResults,
                                                                   CancellationToken cancellationToken);
}