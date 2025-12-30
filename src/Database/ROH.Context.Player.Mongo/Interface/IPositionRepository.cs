using ROH.Context.Player.Mongo.Entities;

using System.Numerics;

namespace ROH.Context.Player.Mongo.Interface;

public interface IPositionRepository
{
    Task<List<PlayerPosition>> GetAllPlayersAsync(CancellationToken cancellationToken = default);

    Task SavePlayerPositionAsync(PlayerPosition data, CancellationToken cancellationToken = default);

    Task<List<PlayerPosition>> GetPlayersNearbyAsync(
    string playerId,
    Vector3 position,
    double radiusMeters = 100f,
    float heightTolerance = 5f,
    CancellationToken cancellationToken = default);
}